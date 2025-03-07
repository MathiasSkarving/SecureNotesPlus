using System.Security.Cryptography;
using System.IO;
using System;
using System.Text;

namespace SecureNotesPlus;

public class EncryptionService
{
    public void EncryptFile(string password, string fileName, string encryptionText)
    {
        try
        {
            using (FileStream fileStream = new(@"..\\..\\..\\Notes\" + fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] key = Encoding.UTF8.GetBytes(password);
                    byte[] resultKey = new byte[16];
                    Array.Copy(key, resultKey, Math.Min(key.Length, 16));

                    aes.Key = resultKey;

                    byte[] iv = aes.IV;
                    fileStream.Write(iv, 0, iv.Length);

                    using (CryptoStream cryptoStream = new(
                               fileStream,
                               aes.CreateEncryptor(),
                               CryptoStreamMode.Write))
                    {
                        // By default, the StreamWriter uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamWriter(cryptoStream, Encoding.Unicode).
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            encryptWriter.WriteLine(encryptionText);
                        }
                    }
                }
            }

            Console.WriteLine("The file was encrypted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The encryption failed. {ex}");
        }
    }

    public string DecryptFile(string password, string fileName)
    {
        try
        {
            using (FileStream fileStream = new(@"..\\..\\..\\Notes\" + fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] iv = new byte[aes.IV.Length];
                    int numBytesToRead = aes.IV.Length;
                    int numBytesRead = 0;
                    // Læs IV fra fil
                    while (numBytesToRead > 0)
                    {
                        int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    byte[] key = Encoding.UTF8.GetBytes(password);
                    byte[] resultKey = new byte[16];
                    Array.Copy(key, resultKey, Math.Min(key.Length, 16));

                    key = resultKey;

                    using (CryptoStream cryptoStream = new(
                               fileStream,
                               aes.CreateDecryptor(key, iv),
                               CryptoStreamMode.Read))
                    {
                        // By default, the StreamReader uses UTF-8 encoding.
                        // To change the text encoding, pass the desired encoding as the second parameter.
                        // For example, new StreamReader(cryptoStream, Encoding.Unicode).
                        using (StreamReader decryptReader = new(cryptoStream))
                        {
                            string decryptedMessage = decryptReader.ReadToEnd();
                            return decryptedMessage;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
        }
        return "";
    }
}
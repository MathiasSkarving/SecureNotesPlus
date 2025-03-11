using System.Security.Cryptography;
using System.IO;
using System;
using System.Text;

namespace SecureNotesPlus;

public class EncryptionService
{
    public bool EncryptFile(string password, string filePath, string encryptionText)
    {
        if(password == "") return false;
        try
        {
            using (FileStream fileStream = new(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
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
                        using (StreamWriter encryptWriter = new(cryptoStream))
                        {
                            encryptWriter.WriteLine(encryptionText);
                        }
                    }
                }
            }

            Console.WriteLine("The file was encrypted.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The encryption failed. {ex}");
        }
        return false;
    }

    public (bool,string) DecryptFile(string password, string filePath)
    {
        if(password == "") return (false, "");
        try
        {
            using (FileStream fileStream = new(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
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
                            if (decryptedMessage == "")
                            {
                                return (false, decryptedMessage);
                            }
                            else
                            {
                                return (true, decryptedMessage);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
        }
        return (false, null);
    }
}
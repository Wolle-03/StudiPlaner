using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace StudiPlaner.Core;

public static class SaveFile
{
    public static void Save<T>(T data, string name, string password)
    {
        try
        {
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner"));
            using FileStream fileStream = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner", name + ".profile"), FileMode.Create);
            using Aes aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            fileStream.Write(aes.IV, 0, aes.IV.Length);
            using CryptoStream cryptoStream = new(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter encryptWriter = new(cryptoStream);
            encryptWriter.WriteLine(JsonSerializer.Serialize(data));
            Console.WriteLine("Saved successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed saving. {e}");
        }
    }

    public static T? Load<T>(string name, string password)
    {
        if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner", name + ".profile")))
            return default;
        try
        {
            using FileStream fileStream = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner", name + ".profile"), FileMode.Open);
            using Aes aes = Aes.Create();
            byte[] iv = new byte[aes.IV.Length];
            int numBytesToRead = aes.IV.Length;
            int numBytesRead = 0;
            while (numBytesToRead > 0)
            {
                int n = fileStream.Read(iv, numBytesRead, numBytesToRead);
                if (n == 0) break;
                numBytesRead += n;
                numBytesToRead -= n;
            }
            using CryptoStream cryptoStream = new(
               fileStream,
               aes.CreateDecryptor(SHA256.HashData(Encoding.UTF8.GetBytes(password)), iv),
               CryptoStreamMode.Read);
            using StreamReader decryptReader = new(cryptoStream);
            string s = decryptReader.ReadToEnd();
            T? data = JsonSerializer.Deserialize<T>(s);
            Console.WriteLine($"Loading successfully");
            return data;
        }
        catch (Exception)
        {
            return default;
        }
    }
}
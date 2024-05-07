using StudiPlaner.Core.Data;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace StudiPlaner.Core;

public static class SaveFile
{
    public static void Save(string path, string file, Profile data, string password)
    {
        try
        {
            Directory.CreateDirectory(path);
            using FileStream fileStream = new(Path.Combine(path, file), FileMode.Create);
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

    public static void Save(Profile data, string password) =>
        Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner"), "profile", data, password);

    public static async Task<Profile?> AsyncLoad(string path, string file, string password)
    {
        try
        {
            using FileStream fileStream = new(Path.Combine(path, file), FileMode.Open);
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
            Profile profile = JsonSerializer.Deserialize<Profile>(await decryptReader.ReadToEndAsync());
            Console.WriteLine($"Loading successfully");

            return profile;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The decryption failed. {ex}");
            return null;
        }
    }

    public static Task<Profile?> AsyncLoad(string password) =>
        AsyncLoad(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "StudiPlaner"), "profile", password);
}
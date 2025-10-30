using System.Security.Cryptography;
using System.Text;

public class EncryptionService
{
    private readonly string _key;
    private readonly string _iv;

    public EncryptionService(string key, string iv)
    {
        _key = key;
        _iv = iv;
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = Encoding.UTF8.GetBytes(_iv);

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = Encoding.UTF8.GetBytes(_iv);

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(cipherText));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        {
            return sr.ReadToEnd();
        }
    }

    // Converte uma string para Base64
    public string ConvertToBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
            throw new ArgumentException("A string de entrada não pode ser nula ou vazia.");

        byte[] byteArray = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(byteArray);
    }

    // Converte uma string Base64 de volta para texto
    public string ConvertFromBase64(string base64Input)
    {
        if (string.IsNullOrEmpty(base64Input))
            throw new ArgumentException("A string Base64 de entrada não pode ser nula ou vazia.");

        byte[] byteArray = Convert.FromBase64String(base64Input);
        return Encoding.UTF8.GetString(byteArray);
    }
}

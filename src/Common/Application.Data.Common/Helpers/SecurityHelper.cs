using System.Security.Cryptography;
using System.Text;
using VistaLOS.Application.Common.Exceptions;

namespace VistaLOS.Application.Data.Common.Helpers;

public class SecurityHelper
{
    public static string Encrypt(string key, string data)
    {
        try {
            var keys = GetHashKeys(key);
            return EncryptStringToBytes_Aes(data, keys[0], keys[1]);
        }
        catch (Exception ex) {
            throw new CoreException("Encryption failed", ex);
        }
    }

    public static string Decrypt(string key, string data)
    {
        try {
            var keys = GetHashKeys(key);
            return DecryptStringFromBytes_Aes(data, keys[0], keys[1]);
        }
        catch (Exception ex) {
            throw new CoreException("Decryption failed", ex);
        }
    }

    private static byte[][] GetHashKeys(string key)
    {
        var result = new byte[2][];
        var enc = Encoding.UTF8;

        var sha256 = SHA256.Create();

        var rawKey = enc.GetBytes(key);
        var rawIV = enc.GetBytes(key);

        var hashKey = sha256.ComputeHash(rawKey);
        var hashIV = sha256.ComputeHash(rawIV);

        Array.Resize(ref hashIV, 16);

        result[0] = hashKey;
        result[1] = hashIV;

        return result;
    }

    //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
    private static string EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        byte[] encrypted;

        using (var aesAlg = Aes.Create()) {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream()) {
                using (var csEncrypt =
                        new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                    using (var swEncrypt = new StreamWriter(csEncrypt)) {
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }
        return Convert.ToBase64String(encrypted);
    }

    //source: https://msdn.microsoft.com/de-de/library/system.security.cryptography.aes(v=vs.110).aspx
    private static string DecryptStringFromBytes_Aes(string cipherTextString, byte[] Key, byte[] IV)
    {
        var cipherText = Convert.FromBase64String(cipherTextString);

        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        string plaintext;

        using (var aesAlg = Aes.Create()) {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(cipherText)) {
                using (var csDecrypt =
                        new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                    using (var srDecrypt = new StreamReader(csDecrypt)) {
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }
}

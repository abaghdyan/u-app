using System.Security.Cryptography;
using System.Text;

namespace VistaLOS.Application.Common.Extensions;

public static class StringExtensions
{
    public static string BuildSha256(this string password)
    {
        using (var sha256Hash = SHA256.Create()) {
            // ComputeHash - returns byte array
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Convert byte array to a string
            var builder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i++) {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
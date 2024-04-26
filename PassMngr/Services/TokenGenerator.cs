using System;
using System.Security.Cryptography;
using System.Text;

public class TokenGenerator
{
    public static string GenerateToken(int id, string email)
    {
        // Concatenate id and email into a single string
        string combinedString = $"{id}-{email}";

        // Compute hash of the combined string
        string token = ComputeHash(combinedString);

        return token;
    }

    private static string ComputeHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
     
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

}
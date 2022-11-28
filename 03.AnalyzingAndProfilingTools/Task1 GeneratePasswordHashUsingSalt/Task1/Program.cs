using System.Security.Cryptography;

namespace MyProject;
class Program
{
    static void Main(string[] args)
    {
        var password = "test12345";
        var passwordHash = GeneratePasswordHashUsingSalt(password);

        Console.WriteLine(passwordHash);
    }
    public static string GeneratePasswordHashUsingSalt(string passwordText)
    {
        var iterate = 10000;
        var salt = new byte[16];
        
        using (var rng = RandomNumberGenerator.Create()) {
            rng.GetBytes(salt);
        }

        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
        byte[] hash = pbkdf2.GetBytes(20);

        var hashBytes = new List<byte>(salt);
        hashBytes.AddRange(hash);

        var passwordHash = Convert.ToBase64String(hashBytes.ToArray());

        return passwordHash;

    }
}



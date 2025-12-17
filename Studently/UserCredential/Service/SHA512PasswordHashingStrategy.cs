using System.Security.Cryptography;
using System.Text;

namespace Studently.UserCredential.Service;

public class SHA512PasswordHashingStrategy : IPasswordHashingStrategy
{
    private const int SaltLength = 16; // 16 bytes = 128 bits
    private const int HashLength = 64; // SHA-512 produces 64 bytes = 512 bits

    public string AlgorithmName => "SHA-512";

    public string Hash(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password cannot be null or empty", nameof(password));
        }

        // Generate random salt
        byte[] salt = GenerateSalt();

        // Hash password with salt
        byte[] hash = HashPasswordWithSalt(password, salt);

        // Combine salt + hash
        byte[] hashBytes = new byte[SaltLength + HashLength];
        Array.Copy(salt, 0, hashBytes, 0, SaltLength);
        Array.Copy(hash, 0, hashBytes, SaltLength, HashLength);

        // Return as Base64 string
        return Convert.ToBase64String(hashBytes);
    }

    public bool Verify(string password, string hash)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }

        if (string.IsNullOrEmpty(hash))
        {
            return false;
        }

        try
        {
            // Decode the Base64 hash
            byte[] hashBytes = Convert.FromBase64String(hash);

            // Validate hash length
            if (hashBytes.Length != SaltLength + HashLength)
            {
                return false;
            }

            // Extract salt from hash
            byte[] salt = new byte[SaltLength];
            Array.Copy(hashBytes, 0, salt, 0, SaltLength);

            // Extract stored hash
            byte[] storedHash = new byte[HashLength];
            Array.Copy(hashBytes, SaltLength, storedHash, 0, HashLength);

            // Hash the provided password with the extracted salt
            byte[] computedHash = HashPasswordWithSalt(password, salt);

            // Compare hashes using constant-time comparison
            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
        catch (FormatException)
        {
            // Invalid Base64 string
            return false;
        }
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[SaltLength];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    private static byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        // Combine salt and password
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
        byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];

        Array.Copy(salt, 0, saltedPassword, 0, salt.Length);
        Array.Copy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        // Hash using SHA-512
        using (var sha512 = SHA512.Create())
        {
            return sha512.ComputeHash(saltedPassword);
        }
    }
}

using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using TodoApi.Models;

namespace TodoApi.Utils;

public sealed class PasswordHasher : IPasswordHasher<User>
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100000;
    HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

    public string HashPassword(User user, string password)
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pdkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithm);
        byte[] hash = pdkdf2.GetBytes(HashSize);

        var result = new byte[SaltSize + HashSize];
        Buffer.BlockCopy(salt, 0, result, 0, SaltSize);
        Buffer.BlockCopy(hash, 0, result, SaltSize, HashSize);

        return Convert.ToBase64String(result);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string providedPassword)
    {
        return VerifyHashedPassword(user, user.Password, providedPassword);
    }

    public PasswordVerificationResult VerifyHashedPassword(
        User user,
        string hashedPassword,
        string providedPassword
    )
    {
        ArgumentNullException.ThrowIfNull(hashedPassword);
        ArgumentNullException.ThrowIfNull(providedPassword);

        byte[] decodedHashPassword = Convert.FromBase64String(hashedPassword);

        if (decodedHashPassword.Length == 0)
            return PasswordVerificationResult.Failed;

        if (VerifyPassword(user, hashedPassword, providedPassword))
        {
            return PasswordVerificationResult.Success;
        }

        return PasswordVerificationResult.Failed;
    }

    private bool VerifyPassword(User user, string hashedPassword, string providedPassword)
    {
        var stored = Convert.FromBase64String(hashedPassword);
        var salt = new byte[SaltSize];
        Buffer.BlockCopy(stored, 0, salt, 0, SaltSize);

        var hash = new byte[HashSize];
        Buffer.BlockCopy(stored, SaltSize, hash, 0, HashSize);

        using var pdkdf2 = new Rfc2898DeriveBytes(
            providedPassword,
            salt,
            Iterations,
            HashAlgorithm
        );
        byte[] hashToCompare = pdkdf2.GetBytes(HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }
}

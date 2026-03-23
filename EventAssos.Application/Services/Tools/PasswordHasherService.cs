﻿using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using EventAssos.Application.Interfaces.Services.Tools;

namespace EventAssos.Application.Services.Tools
{   
    public class PasswordHasherService : IPasswordHasherService
    {
        // Paramètres Argon2id recommandés par l'OWASP pour un bon équilibre sécurité/performance
        private const int SaltSize = 16;              // Taille du sel en bytes
        private const int HashSize = 32;              // Taille du hash final en bytes
        private const int Iterations = 4;             // Nombre de passes sur la mémoire
        private const int MemorySize = 65536;         // 64 MB de RAM utilisée
        private const int DegreeOfParallelism = 2;    // Nombre de threads parallèles

        public string HashPassword(PasswordHash password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = HashPasswordWithSalt(password, salt);

            byte[] combined = new byte[SaltSize + HashSize];

            Array.Copy(salt, 0, combined, 0, SaltSize);
            Array.Copy(hash, 0, combined, SaltSize, HashSize);

            return Convert.ToBase64String(combined);
        }

        private byte[] HashPasswordWithSalt(PasswordHash password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password.ToString()))
            {
                Salt = salt,
                Iterations = Iterations,
                MemorySize = MemorySize,
                DegreeOfParallelism = DegreeOfParallelism
            };

            return argon2.GetBytes(HashSize);
        }

        public bool VerifyPassword(PasswordHash password, PasswordHash storedPassword)
        {
            byte[] hashWithSalt = Convert.FromBase64String(storedPassword.ToString());

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashWithSalt, 0, salt, 0, SaltSize);

            byte[] storedHash = new byte[HashSize];
            Array.Copy(hashWithSalt, SaltSize, storedHash, 0, HashSize);

            byte[] computedHash = HashPasswordWithSalt(password, salt);

            return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
        }
    }
}
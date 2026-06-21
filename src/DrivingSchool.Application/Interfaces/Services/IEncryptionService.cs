namespace DrivingSchool.Application.Interfaces.Services;

/// <summary>
/// Provides AES-256-GCM authenticated encryption for chat messages.
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Derives a 256-bit encryption key from the master key and chat identifier
    /// using HKDF-SHA256 (RFC 5869). The derived key is unique per chat.
    /// </summary>
    /// <param name="masterKey">The application master key (hex-encoded, 32 bytes).</param>
    /// <param name="chatId">The chat identifier used as HKDF context info.</param>
    /// <returns>A 32-byte derived key.</returns>
    byte[] DeriveKey(string masterKey, Guid chatId);

    /// <summary>
    /// Encrypts the plaintext string using AES-256-GCM
    /// with a freshly generated random 96-bit initialisation vector.
    /// </summary>
    /// <param name="plaintext">The plain text to encrypt.</param>
    /// <param name="key">The 32-byte encryption key.</param>
    /// <returns>
    /// An <see cref="EncryptedData"/> record containing the Base64-encoded
    /// ciphertext, IV, and GCM authentication tag.
    /// </returns>
    EncryptedData Encrypt(string plaintext, byte[] key);

    /// <summary>
    /// Decrypts ciphertext and verifies the GCM authentication tag.
    /// </summary>
    /// <param name="data">The encrypted payload.</param>
    /// <param name="key">The 32-byte decryption key.</param>
    /// <returns>The original plaintext string.</returns>
    /// <exception cref="System.Security.Cryptography.CryptographicException">
    /// Thrown when authentication tag verification fails, indicating the ciphertext
    /// has been tampered with.
    /// </exception>
    string Decrypt(EncryptedData data, byte[] key);
}

/// <summary>
/// Carries the output of an AES-256-GCM encryption operation.
/// All fields are Base64-encoded strings.
/// </summary>
/// <param name="CipherText">The encrypted content.</param>
/// <param name="IV">The 96-bit (12-byte) random initialisation vector.</param>
/// <param name="AuthTag">The 128-bit GCM authentication tag.</param>
public sealed record EncryptedData(string CipherText, string IV, string AuthTag);
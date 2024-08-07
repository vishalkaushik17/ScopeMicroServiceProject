using System.Security.Cryptography;
using System.Text;

namespace GenericFunction
{

    public class StringEncryptionService
    {
        //private byte[] IV =
        //{
        //    0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
        //    0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        //};

        //private byte[] DeriveKeyFromPassword(string password)
        //{
        //    var emptySalt = Array.Empty<byte>();
        //    var iterations = 1000;
        //    var desiredKeyLength = 16; // 16 bytes equal 128 bits.
        //    var hashMethod = HashAlgorithmName.SHA384;
        //    return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
        //        emptySalt,
        //        iterations,
        //        hashMethod,
        //        desiredKeyLength);
        //}

        //public async Task<byte[]> EncryptAsync(string clearText, string passphrase)
        //{
        //    using Aes aes = Aes.Create();
        //    aes.Key = DeriveKeyFromPassword(passphrase);
        //    aes.IV = IV;

        //    using MemoryStream output = new();
        //    using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);

        //    await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
        //    await cryptoStream.FlushFinalBlockAsync();

        //    return output.ToArray();
        //}

        //public async Task<string> DecryptAsync(byte[] encrypted, string passphrase)
        //{
        //    using Aes aes = Aes.Create();
        //    aes.Key = DeriveKeyFromPassword(passphrase);
        //    aes.IV = IV;

        //    using MemoryStream input = new(encrypted);
        //    using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);

        //    using MemoryStream output = new();
        //    await cryptoStream.CopyToAsync(output);

        //    return Encoding.Unicode.GetString(output.ToArray());
        //}
        public virtual string Encrypt(string clearText, string EncryptionKey)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public virtual string Decrypt(string cipherText, string EncryptionKey)
        {
            //string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Mode = CipherMode.CBC;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}

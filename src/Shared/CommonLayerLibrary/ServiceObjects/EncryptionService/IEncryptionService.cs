namespace GenericFunction.ServiceObjects.EncryptionService;

public interface IEncryptionService
{
    //string Encrypt(string clearText, string passphrase);
    //string Decrypt(string encrypted, string passphrase);
    private static readonly StringEncryptionService encryptionService = new StringEncryptionService();

    public static string Decrypt(string encrypted, string passphrase)
    {
        return encryptionService.Decrypt(encrypted, passphrase);
    }

    public static string Encrypt(string clearText, string passphrase)
    {
        return encryptionService.Encrypt(clearText, passphrase);
    }
}
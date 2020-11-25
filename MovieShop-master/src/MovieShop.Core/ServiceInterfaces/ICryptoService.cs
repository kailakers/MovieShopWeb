namespace MovieShop.Core.ServiceInterfaces
{
    public interface ICryptoService
    {
        string CreateSalt();
        string HashPassword(string password, string salt);
    }
}
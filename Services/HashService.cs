namespace csdottraining.Services
{
  public class HashService : IHashService
  {
    public string EncryptPassword(string password)
      => BCrypt.Net.BCrypt.HashPassword(password);

    public bool VerifyPassoword(string password, string hash)
      => BCrypt.Net.BCrypt.Verify(password, hash);
  }

  public interface IHashService
  {
    string EncryptPassword(string password);
    bool VerifyPassoword(string password, string hash);
  }
}

namespace csdottraining.Models 
{
  public class JwtSettings : IJwtSettings
  {
    public string Secret { get; set; }
    public int Expires { get; set; }
  }

   public interface IJwtSettings
  {
    string Secret { get; set; }
    int Expires { get; set; }
  }

}

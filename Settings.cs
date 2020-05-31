namespace csdottraining
{
  public class Settings : ISettings
  {
    public string UsersCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
    public string Secret { get; set; }
    public int Expires { get; set; }
  }
}

public interface ISettings
  {
    string UsersCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    string Secret { get; set; }
    int Expires { get; set; }
  }
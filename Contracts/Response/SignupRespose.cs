using System;

namespace csdottraining.Contracts
{
  public class SignupResponse 
  {
    public string id  { get; set; }
    public string access_token { get; set; }
    public DateTime last_login { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

  }
}
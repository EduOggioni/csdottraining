using FluentValidation;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace csdottraining.Models 
{
  public class User 
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string id  { get; set; }

    [BsonElement("Name")]
    [JsonProperty("name")]
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public List<Phone> phones { get; set; }
  }
  public class UserValidator : AbstractValidator<User> 
  {
    public UserValidator()
    {
      RuleFor(user => user.username).Length(2, 10);
      RuleFor(user => user.email).EmailAddress();
      RuleFor(user => user.password).NotEmpty().NotNull();
      RuleForEach(x => x.phones).SetValidator(new PhoneValidator());
    } 
  }
}
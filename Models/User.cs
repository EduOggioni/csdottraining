using FluentValidation;
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
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public List<Phone> phones { get; set; }
    public string access_token { get; set; }
    public BsonDateTime last_login { get; set; }
    public BsonDateTime creation_date { get; set; }
    public BsonDateTime update_date { get; set; }

  }
  public class UserValidator : AbstractValidator<User> 
  {
    public UserValidator()
    {
      RuleFor(user => user.name).Length(2, 10);
      RuleFor(user => user.email).EmailAddress().NotNull();
      RuleFor(user => user.password).NotEmpty().NotNull();
      RuleForEach(x => x.phones).SetValidator(new PhoneValidator());
    } 
  }
}
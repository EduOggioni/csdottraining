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
    public string username { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public List<Phone> phones { get; set; }
  }
}
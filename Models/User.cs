using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System;

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
    public DateTime last_login { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }

  }
}
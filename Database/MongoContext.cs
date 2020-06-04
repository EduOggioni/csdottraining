using csdottraining.Models;
using MongoDB.Driver;
using System;

namespace csdottraining.Database
{
    public class MongoContext
    {
      private readonly IMongoDatabase _database = null;
      private readonly ISettings _settings;

      public MongoContext(ISettings settings)
      {
        try
        {
          var client = new MongoClient(settings.ConnectionString);
          _settings = settings;

          if(client != null)
            _database = client.GetDatabase(settings.DatabaseName);
          
        }
        catch (Exception ex)
        {
            throw new Exception("Connection error", ex);
        }
      }

      public IMongoCollection<User> User
      {
          get => _database.GetCollection<User>(_settings.UsersCollectionName);
      }
  }
}
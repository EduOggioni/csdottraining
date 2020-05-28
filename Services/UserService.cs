using csdottraining.Models;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace csdottraining.Services
{
  public class UserService : IUserService
  {
    private readonly IMongoCollection<User> _users;
    private IHashService _hashService;

    public UserService(IUsersDatabaseSettings settings, IHashService hashService)
    {
      var client = new MongoClient(settings.ConnectionString);
      var database = client.GetDatabase(settings.DatabaseName);

      _hashService = hashService;
      _users = database.GetCollection<User>(settings.UsersCollectionName);
    }

    public async Task<User> GetUserAtBase(string email, string password)
    {
      var user = await GetUserByEmailAsync(email);

      if(!_hashService.VerifyPassoword(password, user.password)) {
        return null;
      }

      var dateTime = DateTime.UtcNow;
      var update = Builders<User>.Update
        .SetOnInsert(user => user.last_login, dateTime)
        .SetOnInsert(user => user.update_date, dateTime);
      
      return await _users.FindOneAndUpdateAsync(user => user.email == email, update);
    }

    public async Task<User> GetUserByIdAsync(string id) =>
       await _users.Find<User>(user => user.id == id).FirstOrDefaultAsync();

    public async Task<User> GetUserByEmailAsync(string email) =>
       await _users.Find<User>(user => user.email == email).FirstOrDefaultAsync();

    public async Task<User> CreateAsync(User user, string token)
    {
      var dateTime = DateTime.UtcNow;
      
      user.password = _hashService.EncryptPassword(user.password);
      user.access_token = token;
      user.creation_date = dateTime;
      user.update_date = dateTime;

      await _users.InsertOneAsync(user);
      return user;
    }

  }

  public interface IUserService
    {
        Task<User> CreateAsync(User user, string token);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserAtBase(string email, string password);
    }
}
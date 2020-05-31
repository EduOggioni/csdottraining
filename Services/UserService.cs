using csdottraining.Models;
using MongoDB.Driver;
using System.Threading.Tasks;
using System;

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

    public async Task<User> GetUserByIdAsync(string id) =>
       await _users.Find<User>(user => user.id == id).FirstOrDefaultAsync();

    public async Task<User> GetUserAsync(string email) =>
       await _users.Find<User>(user => user.email == email).FirstOrDefaultAsync();
    
    public async Task<User> GetUserAsync(string email, string password)
    {
      var user = await _users.Find<User>(user => user.email == email).FirstOrDefaultAsync();

      if(!_hashService.VerifyPassoword(password, user.password)) {
        return null;
      }

      return user;
    }

    public async Task<User> CreateAsync(User user)
    { 
      user.password = _hashService.EncryptPassword(user.password);
      await _users.InsertOneAsync(user);

      return user;
    }

    public async Task<User> UpdateAsync(User user) =>
     await _users.FindOneAndReplaceAsync(dbUser => dbUser.id == user.id, user);
  }

  public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(string email, string password);
        Task<User> UpdateAsync(User user);

    }
}
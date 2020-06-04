using MongoDB.Driver;
using csdottraining.Models;
using csdottraining.Database;
using System.Threading.Tasks;

namespace csdottraining.Repository
{
    public class UserRepository : IUserRepository
    {
      private readonly IMongoCollection<User> _user;
      
      public UserRepository(MongoContext context)
      {
        _user = context.User;
      }
      
       public async Task<User> FindById(string id) =>
       await _user.Find<User>(user => user.id == id).FirstOrDefaultAsync();

      public async Task<User> FindByEmail(string email) =>
        await _user.Find<User>(user => user.email == email).FirstOrDefaultAsync();
      
      public async Task InsertOneAsync(User user) =>
        await _user.InsertOneAsync(user);

      public async Task UpdateOneAsync(User user) =>
        await _user.ReplaceOneAsync(dbUser => dbUser.id == user.id, user);
      }

      public interface IUserRepository
      {
        Task<User> FindById(string id);
        Task<User> FindByEmail(string id);
        Task InsertOneAsync(User user);
        Task UpdateOneAsync(User user);
      }
}
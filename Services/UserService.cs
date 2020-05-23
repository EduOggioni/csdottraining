using csdottraining.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csdottraining.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IUsersDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
        }

        public async Task<List<User>> GetUsersAsync() =>
           await _users.Find(user => true).ToListAsync();

        public async Task<User> GetUsersAsync(string id) =>
           await _users.Find<User>(user => user.id == id).FirstOrDefaultAsync();

        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }

        public async void UpdateAsync(string id, User userIn) =>
            await _users.ReplaceOneAsync(user => user.id == id, userIn);

        public async void RemoveAsync(User userIn) =>
            await _users.DeleteOneAsync(user => user.id == userIn.id);

        public async void RemoveAsync(string id) => 
            await _users.DeleteOneAsync(user => user.id == id);
    }
}
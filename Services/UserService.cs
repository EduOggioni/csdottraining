using System;
using csdottraining.Models;
using csdottraining.Repository;
using System.Threading.Tasks;

namespace csdottraining.Services
{
  public class UserService : IUserService
  {
    private IUserRepository _repo;
    private IHashService _hashService;
    private ITokenService _tokenService;

    public UserService(IUserRepository repo,IHashService hashService, ITokenService tokenService)
    {
      _repo = repo;
      _hashService = hashService;
      _tokenService = tokenService;
    }

    public async Task<User> GetUserByIdAsync(string id) =>
       await _repo.FindById(id);

    public async Task<User> GetUserAsync(string email) =>
       await _repo.FindByEmail(email);
    
    public async Task<User> GetUserAsync(string email, string password)
    {
      var user = await _repo.FindByEmail(email);

      if(user == null) return null;
      
      if(!_hashService.VerifyPassoword(password, user.password)) {
        return null;
      }

      return user;
    }

    public async Task<User> CreateAsync(User user)
    { 
      var dateTime = DateTime.UtcNow;

      user.last_login = dateTime;
      user.created_at = dateTime;
      user.access_token = _tokenService.GenerateToken(user.email);
      user.password = _hashService.EncryptPassword(user.password);

      await _repo.InsertOneAsync(user);

      return user;
    }

    public async Task UpdateAsync(User user) {
      var dateTime = DateTime.UtcNow;

      user.last_login = dateTime;
      user.updated_at = dateTime;
      user.access_token = _tokenService.GenerateToken(user.email);

      await _repo.UpdateOneAsync(user);
      
    }
  }

  public interface IUserService
    {
        Task<User> CreateAsync(User user);
        Task<User> GetUserByIdAsync(string id);
        Task<User> GetUserAsync(string email);
        Task<User> GetUserAsync(string email, string password);
        Task UpdateAsync(User user);

    }
}
using server.Dtos;
using server.Entities;

namespace server.Repositories;

public interface IUserRepository
{
    Task SignUpAsync(User user);
    Task<User?> FindUserAsync(string userName);
    Task<AuthDto> ExternalSignIn(User user);
    Task<string> CreateUniqueUsernameFromEmail(string email);
    Task DeleteAsync(int id);
    Task<User?> FindUserByIdAsync (int id);
   
}   

using System.Text;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Dtos;
using server.Entities;
using server.Utilities;

namespace server.Repositories;

public class UserRepository : IUserRepository
{
  private readonly FinanceContext dbContext;

  public UserRepository(FinanceContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public async Task SignUpAsync(User user)
  {
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
  }
  public async Task<User?> FindUserAsync(string userName)
  {
    return await dbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(userName));
  }

public async Task<AuthDto> ExternalSignIn(User user)
{
    if (dbContext == null)
    {
        throw new ArgumentNullException(nameof(dbContext), "DbContext is not initialized");
    }
    var dbUser = await dbContext.Users
        .FirstOrDefaultAsync(u =>
            u.ExternalId != null && u.ExternalId.Equals(user.ExternalId) &&
            u.ExternalType != null && u.ExternalType.Equals(user.ExternalType)
        );

    if (dbUser == null)
    {
        user.Username = await CreateUniqueUsernameFromEmail(user.Email ?? "default@domain.com");
        await SignUpAsync(user);

        // Retrieve the newly signed-up user from the database
        dbUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
        if (dbUser != null)
        {
            // Generate a new token for the newly signed-up user
            return new AuthDto(dbUser.Username, JwtGenerator.GenerateUserToken(dbUser.Username));
        }
    }
    // Generate the token for the existing user
    return dbUser != null
        ? new AuthDto(dbUser.Username, JwtGenerator.GenerateUserToken(dbUser.Username))
        : new AuthDto("defaultUsername", "defaultToken");
}

 

  public async Task<string> CreateUniqueUsernameFromEmail(string email)
  {
    var username = email.Split("@")[0];
    var uniqueUsername = new StringBuilder(username);
    var rnd = new Random();

    while (await dbContext.Users.AnyAsync(u => u.Username.Equals(uniqueUsername.ToString())))
    {
      var rand = rnd.Next(1000);
      uniqueUsername.Append(rand);
    }
    return uniqueUsername.ToString();
  }

  public async Task DeleteAsync(int id)
  {
    await dbContext.Users.Where(user => user.Id == id).ExecuteDeleteAsync();
  }

  public async Task<User?> FindUserByIdAsync(int id)
  {
    return await dbContext.Users.FindAsync(id);
  }
}
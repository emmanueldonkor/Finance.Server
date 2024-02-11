using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Entities;
using server.Exceptions;
using server.Repositories;
using server.Utilities;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IPasswordHasher passwordHasher;

    public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(CreateUserDto createUserDto)
    {
        User user =
            new()
            {
                Username = createUserDto.Username,
                Password = createUserDto.Password,
                Email = createUserDto.Email
            };
        var checkUser = await userRepository.FindUserAsync(user.Username);
        if (checkUser is not null)
        {
            throw new UserNameAlreadyExistExceptions("Username already exists");
        }

        user.Password = passwordHasher.HashPassword(user.Password);
        await userRepository.SignUpAsync(user);

        AuthDto authDto = new(user.Username, JwtGenerator.GenerateUserToken(user.Username));
        return Ok(authDto);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(UserSignInDto userSignInDto)
    {
        User user = new() { Username = userSignInDto.Username, Password = userSignInDto.Password };

        try
        {
            var existingUser = await userRepository.FindUserAsync(user.Username);
            if (
                existingUser is null
                || existingUser.Password is null
                || passwordHasher.VerifyHashedPassword(existingUser.Password, user.Password)
                    == PasswordVerificationResult.Failed
            )
            {
                throw new InvalidUsernamePasswordException("Invalid username or passoword");
            }
            AuthDto authDto = new(user.Username, JwtGenerator.GenerateUserToken(user.Username));
            return Ok(authDto);
        }
        catch (InvalidUsernamePasswordException e)
        {
            return StatusCode(401, e.Message);
        }
    }

    [HttpPost("google")]
    public async Task<ActionResult> GoogleSignIn(string token)
    {
        var payload = await ValidateAsync(
            token,
            new ValidationSettings
            {
                Audience = new[] { Environment.GetEnvironmentVariable("CLIENT_ID") }
            }
        );

        User user =
            new()
            {
                Username = "",
                Password = "*",
                Email = payload.Email,
                ExternalId = payload.Subject,
                ExternalType = "GOOGLE"
            };
        var checkUser = await userRepository.FindUserAsync(user.Username);
        if (checkUser is not null)
        {
            throw new UserNameAlreadyExistExceptions("Username already exists");
        }
        var result = await userRepository.ExternalSignIn(user);

        return Created("", result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await userRepository.FindUserByIdAsync(id);
        if (user is not null)
        {
            await userRepository.DeleteAsync(id);
        }
        return NoContent();
    }
}

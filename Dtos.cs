namespace server.Dtos;

public record AuthDto(string Username, string Token);

public record CreateUserDto(string Username, string Password, string Email);

public record  UserSignInDto(string Username, string Password);

public record ExpenseDto(int Id, string Description, decimal Amount, DateTime? CreatedAt);

public record CreateExpenseDto(string Description, decimal Amount);

public record UpdateExpenseDto(string Description, decimal Amount);

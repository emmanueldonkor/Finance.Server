using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Entities;

namespace server.Repositories;

public class ExpenseRepository : IExpenseRepository
{
  private readonly FinanceContext dbContext;
  private readonly IHttpContextAccessor httpContextAccessor;

  public ExpenseRepository(FinanceContext dbContext, IHttpContextAccessor httpContextAccessor)
  {
    this.dbContext = dbContext;
    this.httpContextAccessor = httpContextAccessor;
  }

  public async Task CreateAsync(Expense expense)
  {
    var userName = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
    expense.User = user;
    dbContext.Add(expense);
    await dbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync(int id)
  {
    var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

    var expense = await dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.User == user);
    if (expense is not null)
    {
      dbContext.Expenses.Remove(expense);
      await dbContext.SaveChangesAsync();
     
    }
  }

  public async Task<IEnumerable<Expense>> GetAllAsync()
  {
    var username = httpContextAccessor?.HttpContext?.User.Identity?.Name;
    var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

    if (user is not null)
    {
      var expenses = await dbContext.Expenses
          .Where(e => e.User == user)
          .AsNoTracking()
          .ToListAsync();

      return expenses;
    }
    else
    {
      return Enumerable.Empty<Expense>();
    }

  }

  public async Task<Expense?> GetAsync(int id)
  {

    var username = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    var user = await dbContext.Users.FirstAsync(u => u.Username == username);
    return await dbContext.Expenses.FirstOrDefaultAsync(e => e.Id == id && e.User == user);
  }

  public async Task UpdateAsync(Expense updatedExpense)
  {
    var userName = httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    var user = await dbContext.Users
                                .FirstOrDefaultAsync(u => u.Username == userName) ?? throw new Exception("The user is not available");
    var expense = await dbContext.Expenses
                                .Where(e => e.User!.Id == user.Id && e.Id == updatedExpense.Id)
                                .FirstOrDefaultAsync();

    if (expense is not null)
    {
      expense.Amount = updatedExpense.Amount;
      expense.Description = updatedExpense.Description;
      dbContext.Expenses.Update(expense);
      await dbContext.SaveChangesAsync();
    }
  }
}


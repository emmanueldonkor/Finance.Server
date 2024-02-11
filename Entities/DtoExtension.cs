using server.Dtos;

namespace server.Entities;

public static class DtoExtension
{
    public static ExpenseDto AsDto(this Expense expense)
    {
        return new ExpenseDto(
        expense.Id,
        expense.Description,
        expense.Amount,
        expense.CreatedAt
        );
    }
}
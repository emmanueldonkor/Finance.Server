using System.Runtime.CompilerServices;
using server.Dtos;
using server.Entities;

namespace server.Repositories;

public interface IExpenseRepository
{
     Task<IEnumerable<Expense>> GetAllAsync();
     Task<Expense?> GetAsync(int id);
     Task CreateAsync(Expense expense);
     Task UpdateAsync(Expense updatedExpense);
     Task DeleteAsync(int id); 
}
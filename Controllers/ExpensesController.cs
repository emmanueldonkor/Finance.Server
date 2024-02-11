
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Entities;
using server.Repositories;

namespace server.Controllers;
[Authorize]
[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
  private readonly IExpenseRepository expenseRepository;

  public ExpensesController(IExpenseRepository expenseRepository)
  {
    this.expenseRepository = expenseRepository;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllExpenses() => Ok((await expenseRepository.GetAllAsync()).Select(expense => expense.AsDto()));

  [HttpGet("{id}", Name = "GetExpenses")]
  public async Task<IActionResult> GetExpenses(int id)
  {
    var expense = await expenseRepository.GetAsync(id);
    return expense is not null ? Ok(expense.AsDto()) : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> CreateExpense(CreateExpenseDto createExpenseDto)
  {
    Expense newExpense = new()
    {
      Description = createExpenseDto.Description,
      Amount = createExpenseDto.Amount
    };
    await expenseRepository.CreateAsync(newExpense);
    return CreatedAtRoute("GetExpenses", new { id = newExpense.Id }, newExpense.AsDto());
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteExpense(int id)
  {
    var expense = await expenseRepository.GetAsync(id);
    if (expense is not null)
    {
      await expenseRepository.DeleteAsync(id);
    }
    return NoContent();
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateExpense(UpdateExpenseDto updateExpenseDto, int id)
  {
    var existingExpense = await expenseRepository.GetAsync(id);
    if (existingExpense is null)
    {
      return NotFound();
    }
    existingExpense.Description = updateExpenseDto.Description;
    existingExpense.Amount = updateExpenseDto.Amount;
    await expenseRepository.UpdateAsync(existingExpense);
    return NoContent();
  }
}
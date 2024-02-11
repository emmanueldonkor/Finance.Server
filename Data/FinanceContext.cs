using System.Reflection;
using Microsoft.EntityFrameworkCore;
using server.Entities;

namespace server.Data;

public class FinanceContext : DbContext
{
    public FinanceContext(DbContextOptions<FinanceContext> options)
        : base(options) { }

    public DbSet<Expense> Expenses => Set<Expense>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}

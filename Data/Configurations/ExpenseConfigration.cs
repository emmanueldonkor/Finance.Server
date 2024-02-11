using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Entities;

namespace server.Data.Configurations;

public class ExpenseConfigration : IEntityTypeConfiguration<Expense>
{
 public void Configure (EntityTypeBuilder<Expense> builder)
 {
    builder.Property(expense => expense.Amount)
                    .HasPrecision(5,2);
 }
}


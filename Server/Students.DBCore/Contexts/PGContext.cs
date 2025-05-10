using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Students.DBCore.Contexts;

public class PgContext : StudentContext
{
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseNpgsql(
      $"Host=localhost;Port=5432;Database=cifralabs_studentDb;Username=postgres;Password=123;",
      o => o.CommandTimeout(60));
#if DEBUG
    optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
#endif
  }

  public PgContext()
  {
      //this.Database.EnsureDeleted();
      //this.Database.EnsureCreated();
  }
}
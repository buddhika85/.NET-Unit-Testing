using System.Reflection;
using MatchMaker.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MatchMaker.Api.Data;

public class MatchMakerDbContext : DbContext
{
    public MatchMakerDbContext(DbContextOptions<MatchMakerDbContext> options)
        : base(options)
    {
    }

    public DbSet<GameMatch> Matches => Set<GameMatch>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }    
}
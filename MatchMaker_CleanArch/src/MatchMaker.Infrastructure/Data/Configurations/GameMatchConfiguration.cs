using MatchMaker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MatchMaker.Infrastructure.Data.Configurations;

public class GameMatchConfiguration : IEntityTypeConfiguration<GameMatch>
{
    public void Configure(EntityTypeBuilder<GameMatch> builder)
    {
        builder.Property(game => game.State)
               .HasConversion<string>();
    }
}
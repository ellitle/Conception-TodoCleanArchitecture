using CleanTodo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShipConfiguration : IEntityTypeConfiguration<Ship>
{
    public void Configure(EntityTypeBuilder<Ship> builder)
    {
        builder.ToTable("PirateShips", table =>
        {
            table.HasCheckConstraint("CK_PirateShips_Name", "CHAR_LENGTH(TRIM(`name`)) BETWEEN 2 AND 100");
            table.HasCheckConstraint("CK_PirateShips_Captain", "CHAR_LENGTH(TRIM(`captain`)) BETWEEN 2 AND 50");
            table.HasCheckConstraint("CK_PirateShips_GoldCargo", "`goldCargo` BETWEEN 0 AND 1000000");
            table.HasCheckConstraint("CK_PirateShips_CrewSize", "`crewSize` BETWEEN 1 AND 500");
            table.HasCheckConstraint("CK_PirateShips_Status", "BINARY `status` IN ('docked', 'sailing')");
            table.HasCheckConstraint("CK_PirateShips_CreatedBy", "CHAR_LENGTH(TRIM(`createdBy`)) > 0");
            table.HasCheckConstraint("CK_PirateShips_LastModified", "`lastModified` >= `createdAt`");
        });

        builder.HasKey(ship => ship.Id);
        builder.Property(ship => ship.Id).HasColumnName("id");

        builder.Property(ship => ship.Name).HasColumnName("name").IsRequired().HasMaxLength(100);

        builder.Property(ship => ship.Captain).HasColumnName("captain").IsRequired().HasMaxLength(50);

        builder.Property(ship => ship.GoldCargo).HasColumnName("goldCargo").HasPrecision(35, 28).IsRequired();

        builder.Property(ship => ship.CrewSize).HasColumnName("crewSize").IsRequired();

        builder.Property(ship => ship.Status).HasColumnName("status")
            .IsConcurrencyToken()
            .HasMaxLength(7).HasDefaultValue("docked").IsRequired();

        builder.Property(ship => ship.CreatedBy).HasColumnName("createdBy").IsRequired().HasMaxLength(200);

        builder.Property(ship => ship.CreatedAt).HasColumnName("createdAt")
            .HasDefaultValueSql("UTC_TIMESTAMP(6)").IsRequired();

        builder.Property(ship => ship.LastModified).HasColumnName("lastModified")
            .HasDefaultValueSql("UTC_TIMESTAMP(6)").IsRequired();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gimnasio.Core.Entities;

namespace Gimnasio.Infrastructure.Data.Configurations
{
    public class MembresiaConfiguration : IEntityTypeConfiguration<Membresia>
    {
        public void Configure(EntityTypeBuilder<Membresia> builder)
        {
            builder.ToTable("Membresia");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Descripcion)
                .HasMaxLength(20);

            builder.Property(e => e.Precio)
                .HasColumnType("decimal(10,2)");

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);
        }
    }
}
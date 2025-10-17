using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gimnasio.Core.Entities;

namespace Gimnasio.Infrastructure.Data.Configurations
{
    public class UsuarioMembresiaConfiguration : IEntityTypeConfiguration<UsuarioMembresia>
    {
        public void Configure(EntityTypeBuilder<UsuarioMembresia> builder)
        {
            builder.ToTable("Usuario_Membresias");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.PrecioPagado)
                .HasColumnType("decimal(10,2)");

            // Relaciones
            builder.HasOne(um => um.Usuario)
                .WithMany(u => u.UsuarioMembresia)
                .HasForeignKey(um => um.UsuarioId);

            builder.HasOne(um => um.Membresia)
                .WithMany(m => m.UsuarioMembresia)
                .HasForeignKey(um => um.MembresiaId);
        }
    }
}
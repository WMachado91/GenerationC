namespace GenC.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbContextGenC : DbContext
    {
        public DbContextGenC()
            : base("name=DbContextGenC")
        {
        }

        public virtual DbSet<Agendamentos> Agendamentos { get; set; }
        public virtual DbSet<Usuarios> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.Endereco)
                .IsUnicode(false);

            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.CEP)
                .IsUnicode(false);

            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.Cidade)
                .IsUnicode(false);

            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Agendamentos>()
                .Property(e => e.Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.CPF)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .Property(e => e.Senha)
                .IsUnicode(false);

            modelBuilder.Entity<Usuarios>()
                .HasMany(e => e.Agendamentos)
                .WithOptional(e => e.Usuarios)
                .HasForeignKey(e => e.IdUsuario);
        }
    }
}

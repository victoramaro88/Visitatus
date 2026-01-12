using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace API_Visitatus.Models
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cargo> Cargos { get; set; } = null!;
        public virtual DbSet<CargosRito> CargosRitos { get; set; } = null!;
        public virtual DbSet<Cidade> Cidades { get; set; } = null!;
        public virtual DbSet<Estado> Estados { get; set; } = null!;
        public virtual DbSet<GestaoAdministrativa> GestaoAdministrativas { get; set; } = null!;
        public virtual DbSet<GestaoCargo> GestaoCargos { get; set; } = null!;
        public virtual DbSet<Grau> Graus { get; set; } = null!;
        public virtual DbSet<Loja> Lojas { get; set; } = null!;
        public virtual DbSet<Perfil> Perfils { get; set; } = null!;
        public virtual DbSet<PerfilUsuario> PerfilUsuarios { get; set; } = null!;
        public virtual DbSet<Permissao> Permissaos { get; set; } = null!;
        public virtual DbSet<PermissaoPerfil> PermissaoPerfils { get; set; } = null!;
        public virtual DbSet<Potencium> Potencia { get; set; } = null!;
        public virtual DbSet<Presenca> Presencas { get; set; } = null!;
        public virtual DbSet<Rito> Ritos { get; set; } = null!;
        public virtual DbSet<Sessao> Sessaos { get; set; } = null!;
        public virtual DbSet<TemplateCertificadoLoja> TemplateCertificadoLojas { get; set; } = null!;
        public virtual DbSet<TemplateCertificadoPresenca> TemplateCertificadoPresencas { get; set; } = null!;
        public virtual DbSet<TemplateConvite> TemplateConvites { get; set; } = null!;
        public virtual DbSet<TemplateLoja> TemplateLojas { get; set; } = null!;
        public virtual DbSet<TipoSessao> TipoSessaos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<UsuarioLogin> UsuarioLogins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("V1s1tAtu5D3v");

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(e => e.CarCodi)
                    .HasName("PK__Cargos__2586F702A06A1C99");

                entity.Property(e => e.CarCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<CargosRito>(entity =>
            {
                entity.HasKey(e => new { e.CarCodi, e.RitCodi })
                    .HasName("PK__CargosRi__19D736334C0F100C");

                entity.HasOne(d => d.CarCodiNavigation)
                    .WithMany(p => p.CargosRitos)
                    .HasForeignKey(d => d.CarCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CargosRit__carCo__3A81B327");

                entity.HasOne(d => d.RitCodiNavigation)
                    .WithMany(p => p.CargosRitos)
                    .HasForeignKey(d => d.RitCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CargosRit__ritCo__3B75D760");
            });

            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.HasKey(e => e.CidCodi)
                    .HasName("PK__Cidade__3B0495D57793A825");

                entity.Property(e => e.CidCodi).ValueGeneratedNever();

                entity.HasOne(d => d.EstCodiNavigation)
                    .WithMany(p => p.Cidades)
                    .HasForeignKey(d => d.EstCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CidEst");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.EstCodi)
                    .HasName("PK__Estado__19C4B0C8AFA0BB9B");

                entity.Property(e => e.EstCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<GestaoAdministrativa>(entity =>
            {
                entity.HasKey(e => e.GstAdmCodi)
                    .HasName("PK__GestaoAd__2667E1A9BF30E517");

                entity.Property(e => e.GstAdmCodi).ValueGeneratedNever();

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.GestaoAdministrativas)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_gstAdmLj");
            });

            modelBuilder.Entity<GestaoCargo>(entity =>
            {
                entity.HasKey(e => new { e.GstAdmCodi, e.CarCodi, e.UsuCodi })
                    .HasName("PK__GestaoCa__C245BB6032726367");

                entity.HasOne(d => d.CarCodiNavigation)
                    .WithMany(p => p.GestaoCargos)
                    .HasForeignKey(d => d.CarCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GestaoCar__carCo__3E52440B");

                entity.HasOne(d => d.GstAdmCodiNavigation)
                    .WithMany(p => p.GestaoCargos)
                    .HasForeignKey(d => d.GstAdmCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GestaoCar__gstAd__3F466844");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.GestaoCargos)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GestaoCar__usuCo__403A8C7D");
            });

            modelBuilder.Entity<Grau>(entity =>
            {
                entity.HasKey(e => e.GraCodi)
                    .HasName("PK__Grau__3B5A73BB0BA7C4C8");

                entity.Property(e => e.GraCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<Loja>(entity =>
            {
                entity.HasKey(e => e.LojCodi)
                    .HasName("PK__Loja__8D1A09377199FA67");

                entity.Property(e => e.LojCodi).ValueGeneratedNever();

                entity.HasOne(d => d.CidCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.CidCodi)
                    .HasConstraintName("fk_CidLoj");

                entity.HasOne(d => d.PotCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.PotCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PotLoj");

                entity.HasOne(d => d.RitCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.RitCodi)
                    .HasConstraintName("fk_RitLoj");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.PerCodi)
                    .HasName("PK__Perfil__AD5324568757DD47");

                entity.Property(e => e.PerCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<PerfilUsuario>(entity =>
            {
                entity.HasKey(e => e.PeUcodi)
                    .HasName("PK__PerfilUs__CC751F42A5FA87DE");

                entity.Property(e => e.PeUcodi).ValueGeneratedNever();

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.PerfilUsuarios)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PeULoj");

                entity.HasOne(d => d.PerCodiNavigation)
                    .WithMany(p => p.PerfilUsuarios)
                    .HasForeignKey(d => d.PerCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PeUPer");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.PerfilUsuarios)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PeUUsu");
            });

            modelBuilder.Entity<Permissao>(entity =>
            {
                entity.HasKey(e => e.PemCodi)
                    .HasName("PK__Permissa__43834BB773CB30A1");

                entity.Property(e => e.PemCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<PermissaoPerfil>(entity =>
            {
                entity.HasKey(e => new { e.PerCodi, e.PemCodi })
                    .HasName("PK__Permissa__C96B10ED70D94CDD");

                entity.HasOne(d => d.PemCodiNavigation)
                    .WithMany(p => p.PermissaoPerfils)
                    .HasForeignKey(d => d.PemCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissao__pemCo__46E78A0C");

                entity.HasOne(d => d.PerCodiNavigation)
                    .WithMany(p => p.PermissaoPerfils)
                    .HasForeignKey(d => d.PerCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissao__perCo__47DBAE45");
            });

            modelBuilder.Entity<Potencium>(entity =>
            {
                entity.HasKey(e => e.PotCodi)
                    .HasName("PK__Potencia__BE45A4E57FA3280B");

                entity.Property(e => e.PotCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<Presenca>(entity =>
            {
                entity.HasKey(e => new { e.UsuCodi, e.SesCodi, e.LojCodi })
                    .HasName("Presenca_PK");

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.Presencas)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Presenca_Loja_FK");

                entity.HasOne(d => d.SesCodiNavigation)
                    .WithMany(p => p.Presencas)
                    .HasForeignKey(d => d.SesCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Presenca__sesCod__48CFD27E");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.Presencas)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Presenca__usuCod__49C3F6B7");
            });

            modelBuilder.Entity<Rito>(entity =>
            {
                entity.HasKey(e => e.RitCodi)
                    .HasName("PK__Rito__C51C1312BB07A593");

                entity.Property(e => e.RitCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<Sessao>(entity =>
            {
                entity.HasKey(e => e.SesCodi)
                    .HasName("PK__Sessao__DC2A2665D8166928");

                entity.Property(e => e.SesCodi).ValueGeneratedNever();

                entity.HasOne(d => d.GraCodiNavigation)
                    .WithMany(p => p.Sessaos)
                    .HasForeignKey(d => d.GraCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SessGra");

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.Sessaos)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SessLoj");

                entity.HasOne(d => d.TiScodiNavigation)
                    .WithMany(p => p.Sessaos)
                    .HasForeignKey(d => d.TiScodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_SessTiS");
            });

            modelBuilder.Entity<TemplateCertificadoLoja>(entity =>
            {
                entity.HasKey(e => new { e.TmpCrtPreCodi, e.LojCodi })
                    .HasName("PK__Template__5D9E926A0B29E10E");

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.TemplateCertificadoLojas)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateC__lojCo__4E88ABD4");

                entity.HasOne(d => d.TmpCrtPreCodiNavigation)
                    .WithMany(p => p.TemplateCertificadoLojas)
                    .HasForeignKey(d => d.TmpCrtPreCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateC__tmpCr__4F7CD00D");
            });

            modelBuilder.Entity<TemplateCertificadoPresenca>(entity =>
            {
                entity.HasKey(e => e.TmpCrtPreCodi)
                    .HasName("PK__Template__354F32F9CF093C59");

                entity.Property(e => e.TmpCrtPreCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<TemplateConvite>(entity =>
            {
                entity.HasKey(e => e.TmpCvtCodi)
                    .HasName("PK__Template__DC58E92A3F849EEF");

                entity.Property(e => e.TmpCvtCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<TemplateLoja>(entity =>
            {
                entity.HasKey(e => new { e.TmpCvtCodi, e.LojCodi })
                    .HasName("PK__Template__B48949B948303863");

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.TemplateLojas)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateL__lojCo__5070F446");

                entity.HasOne(d => d.TmpCvtCodiNavigation)
                    .WithMany(p => p.TemplateLojas)
                    .HasForeignKey(d => d.TmpCvtCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TemplateL__tmpCv__5165187F");
            });

            modelBuilder.Entity<TipoSessao>(entity =>
            {
                entity.HasKey(e => e.TiScodi)
                    .HasName("PK__TipoSess__EC813CE5916466C3");

                entity.Property(e => e.TiScodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuCodi)
                    .HasName("PK__Usuario__7A35B9D7F7D72F86");

                entity.Property(e => e.UsuCodi).ValueGeneratedNever();
            });

            modelBuilder.Entity<UsuarioLogin>(entity =>
            {
                entity.HasKey(e => e.UsLcodi)
                    .HasName("PK__UsuarioL__F112B8E1F2F473F6");

                entity.Property(e => e.UsLcodi).ValueGeneratedNever();

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.UsuarioLogins)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UsuLogin");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

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

        public virtual DbSet<Cidade> Cidades { get; set; } = null!;
        public virtual DbSet<Estado> Estados { get; set; } = null!;
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
        public virtual DbSet<TipoSessao> TipoSessaos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;
        public virtual DbSet<UsuarioLogin> UsuarioLogins { get; set; } = null!;
        public virtual DbSet<UsuarioLoja> UsuarioLojas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=visitatus.com.br, 11433;Initial Catalog=DB_Visitatus_DEV;User ID=V1s1tAtu5D3v;Password=&i329cQs7");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("V1s1tAtu5D3v");

            modelBuilder.Entity<Cidade>(entity =>
            {
                entity.HasKey(e => e.CidCodi)
                    .HasName("PK__Cidade__3B0495D5F0AEDC95");

                entity.ToTable("Cidade", "dbo");

                entity.Property(e => e.CidCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("cidCodi");

                entity.Property(e => e.CidNome)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("cidNome");

                entity.Property(e => e.CidStat).HasColumnName("cidStat");

                entity.Property(e => e.EstCodi).HasColumnName("estCodi");

                entity.HasOne(d => d.EstCodiNavigation)
                    .WithMany(p => p.Cidades)
                    .HasForeignKey(d => d.EstCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CidEst");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.HasKey(e => e.EstCodi)
                    .HasName("PK__Estado__19C4B0C8AF499D1B");

                entity.ToTable("Estado", "dbo");

                entity.Property(e => e.EstCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("estCodi");

                entity.Property(e => e.EstNome)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("estNome");

                entity.Property(e => e.EstSigl)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("estSigl");

                entity.Property(e => e.EstStat).HasColumnName("estStat");
            });

            modelBuilder.Entity<Grau>(entity =>
            {
                entity.HasKey(e => e.GraCodi)
                    .HasName("PK__Grau__3B5A73BB15AFB354");

                entity.ToTable("Grau", "dbo");

                entity.Property(e => e.GraCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("graCodi");

                entity.Property(e => e.GraNome)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("graNome");

                entity.Property(e => e.GraStat).HasColumnName("graStat");
            });

            modelBuilder.Entity<Loja>(entity =>
            {
                entity.HasKey(e => e.LojCodi)
                    .HasName("PK__Loja__8D1A0937F85DD824");

                entity.ToTable("Loja", "dbo");

                entity.Property(e => e.LojCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("lojCodi");

                entity.Property(e => e.CidCodi).HasColumnName("cidCodi");

                entity.Property(e => e.LojBair)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("lojBair");

                entity.Property(e => e.LojLogo)
                    .IsUnicode(false)
                    .HasColumnName("lojLogo");

                entity.Property(e => e.LojLogr)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("lojLogr");

                entity.Property(e => e.LojNome)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("lojNome");

                entity.Property(e => e.LojNumL)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("lojNumL");

                entity.Property(e => e.LojNume)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("lojNume");

                entity.Property(e => e.LojStat).HasColumnName("lojStat");

                entity.Property(e => e.PotCodi).HasColumnName("potCodi");

                entity.Property(e => e.RitCodi).HasColumnName("ritCodi");

                entity.HasOne(d => d.CidCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.CidCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CidLoj");

                entity.HasOne(d => d.PotCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.PotCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_PotLoj");

                entity.HasOne(d => d.RitCodiNavigation)
                    .WithMany(p => p.Lojas)
                    .HasForeignKey(d => d.RitCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_RitLoj");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.PerCodi)
                    .HasName("PK__Perfil__AD532456D337B4A3");

                entity.ToTable("Perfil", "dbo");

                entity.Property(e => e.PerCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("perCodi");

                entity.Property(e => e.PerNome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("perNome");

                entity.Property(e => e.PerStat).HasColumnName("perStat");
            });

            modelBuilder.Entity<PerfilUsuario>(entity =>
            {
                entity.HasKey(e => e.PeUcodi)
                    .HasName("PK__PerfilUs__CC751F423682D083");

                entity.ToTable("PerfilUsuario", "dbo");

                entity.Property(e => e.PeUcodi)
                    .ValueGeneratedNever()
                    .HasColumnName("peUCodi");

                entity.Property(e => e.PeUstat).HasColumnName("peUStat");

                entity.Property(e => e.PerCodi).HasColumnName("perCodi");

                entity.Property(e => e.UsuCodi).HasColumnName("usuCodi");

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
                    .HasName("PK__Permissa__43834BB73474005B");

                entity.ToTable("Permissao", "dbo");

                entity.Property(e => e.PemCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("pemCodi");

                entity.Property(e => e.PemNome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("pemNome");

                entity.Property(e => e.PemStat).HasColumnName("pemStat");
            });

            modelBuilder.Entity<PermissaoPerfil>(entity =>
            {
                entity.HasKey(e => new { e.PerCodi, e.PemCodi })
                    .HasName("PK__Permissa__C96B10ED68BEFF69");

                entity.ToTable("PermissaoPerfil", "dbo");

                entity.Property(e => e.PerCodi).HasColumnName("perCodi");

                entity.Property(e => e.PemCodi).HasColumnName("pemCodi");

                entity.Property(e => e.PapAtvo).HasColumnName("papAtvo");

                entity.Property(e => e.PepStat).HasColumnName("pepStat");

                entity.HasOne(d => d.PemCodiNavigation)
                    .WithMany(p => p.PermissaoPerfils)
                    .HasForeignKey(d => d.PemCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissao__pemCo__1DE57479");

                entity.HasOne(d => d.PerCodiNavigation)
                    .WithMany(p => p.PermissaoPerfils)
                    .HasForeignKey(d => d.PerCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Permissao__perCo__1CF15040");
            });

            modelBuilder.Entity<Potencium>(entity =>
            {
                entity.HasKey(e => e.PotCodi)
                    .HasName("PK__Potencia__BE45A4E5B4AA1A3D");

                entity.ToTable("Potencia", "dbo");

                entity.Property(e => e.PotCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("potCodi");

                entity.Property(e => e.PotLogo)
                    .IsUnicode(false)
                    .HasColumnName("potLogo");

                entity.Property(e => e.PotNome)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("potNome");

                entity.Property(e => e.PotRegu).HasColumnName("potRegu");

                entity.Property(e => e.PotStat).HasColumnName("potStat");
            });

            modelBuilder.Entity<Presenca>(entity =>
            {
                entity.HasKey(e => new { e.UsuCodi, e.SesCodi })
                    .HasName("PK__Presenca__37F71BB1FCF643DC");

                entity.ToTable("Presenca", "dbo");

                entity.Property(e => e.UsuCodi).HasColumnName("usuCodi");

                entity.Property(e => e.SesCodi).HasColumnName("sesCodi");

                entity.Property(e => e.PreAtiv).HasColumnName("preAtiv");

                entity.HasOne(d => d.SesCodiNavigation)
                    .WithMany(p => p.Presencas)
                    .HasForeignKey(d => d.SesCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Presenca__sesCod__44FF419A");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.Presencas)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Presenca__usuCod__440B1D61");
            });

            modelBuilder.Entity<Rito>(entity =>
            {
                entity.HasKey(e => e.RitCodi)
                    .HasName("PK__Rito__C51C1312CE185351");

                entity.ToTable("Rito", "dbo");

                entity.Property(e => e.RitCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("ritCodi");

                entity.Property(e => e.RitLogo)
                    .IsUnicode(false)
                    .HasColumnName("ritLogo");

                entity.Property(e => e.RitNome)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("ritNome");

                entity.Property(e => e.RitStat).HasColumnName("ritStat");
            });

            modelBuilder.Entity<Sessao>(entity =>
            {
                entity.HasKey(e => e.SesCodi)
                    .HasName("PK__Sessao__DC2A26657C26DAE0");

                entity.ToTable("Sessao", "dbo");

                entity.Property(e => e.SesCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("sesCodi");

                entity.Property(e => e.GraCodi).HasColumnName("graCodi");

                entity.Property(e => e.LojCodi).HasColumnName("lojCodi");

                entity.Property(e => e.SesDesc)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("sesDesc");

                entity.Property(e => e.SesDtHr)
                    .HasColumnType("datetime")
                    .HasColumnName("sesDtHr");

                entity.Property(e => e.SesLibe).HasColumnName("sesLibe");

                entity.Property(e => e.SesStat).HasColumnName("sesStat");

                entity.Property(e => e.TiScodi).HasColumnName("tiSCodi");

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

            modelBuilder.Entity<TipoSessao>(entity =>
            {
                entity.HasKey(e => e.TiScodi)
                    .HasName("PK__TipoSess__EC813CE59C1C3418");

                entity.ToTable("TipoSessao", "dbo");

                entity.Property(e => e.TiScodi)
                    .ValueGeneratedNever()
                    .HasColumnName("tiSCodi");

                entity.Property(e => e.TiSnome)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("tiSNome");

                entity.Property(e => e.TiSstat).HasColumnName("tiSStat");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.UsuCodi)
                    .HasName("PK__Usuario__7A35B9D75E37AE40");

                entity.ToTable("Usuario", "dbo");

                entity.Property(e => e.UsuCodi)
                    .ValueGeneratedNever()
                    .HasColumnName("usuCodi");

                entity.Property(e => e.UsuEmai)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("usuEmai");

                entity.Property(e => e.UsuNasc)
                    .HasColumnType("date")
                    .HasColumnName("usuNasc");

                entity.Property(e => e.UsuNcel)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("usuNCel");

                entity.Property(e => e.UsuNcim)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("usuNCIM");

                entity.Property(e => e.UsuNome)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("usuNome");

                entity.Property(e => e.UsuStat).HasColumnName("usuStat");
            });

            modelBuilder.Entity<UsuarioLogin>(entity =>
            {
                entity.HasKey(e => e.UsLcodi)
                    .HasName("PK__UsuarioL__F112B8E1AFA2A1BC");

                entity.ToTable("UsuarioLogin", "dbo");

                entity.Property(e => e.UsLcodi)
                    .ValueGeneratedNever()
                    .HasColumnName("usLCodi");

                entity.Property(e => e.UsLpass)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("usLPass");

                entity.Property(e => e.UsLstat).HasColumnName("usLStat");

                entity.Property(e => e.UsLuser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("usLUser");

                entity.Property(e => e.UsuCodi).HasColumnName("usuCodi");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.UsuarioLogins)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UsuLogin");
            });

            modelBuilder.Entity<UsuarioLoja>(entity =>
            {
                entity.HasKey(e => new { e.UsuCodi, e.LojCodi })
                    .HasName("PK__UsuarioL__12E41944FBD2F5B8");

                entity.ToTable("UsuarioLoja", "dbo");

                entity.Property(e => e.UsuCodi).HasColumnName("usuCodi");

                entity.Property(e => e.LojCodi).HasColumnName("lojCodi");

                entity.Property(e => e.UsLstat).HasColumnName("usLStat");

                entity.HasOne(d => d.LojCodiNavigation)
                    .WithMany(p => p.UsuarioLojas)
                    .HasForeignKey(d => d.LojCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsuarioLo__lojCo__30F848ED");

                entity.HasOne(d => d.UsuCodiNavigation)
                    .WithMany(p => p.UsuarioLojas)
                    .HasForeignKey(d => d.UsuCodi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UsuarioLo__usuCo__300424B4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

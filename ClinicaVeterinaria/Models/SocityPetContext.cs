using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.Models;

public partial class SocityPetContext : DbContext
{
    public SocityPetContext()
    {
    }

    public SocityPetContext(DbContextOptions<SocityPetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Animali> Animalis { get; set; }

    public virtual DbSet<Armadietti> Armadiettis { get; set; }

    public virtual DbSet<Cassetti> Cassettis { get; set; }

    public virtual DbSet<Dittafornitrice> Dittafornitrices { get; set; }

    public virtual DbSet<Prodotti> Prodottis { get; set; }

    public virtual DbSet<ProdottiInCassetto> ProdottiInCassettos { get; set; }

    public virtual DbSet<Ricettemediche> Ricettemediches { get; set; }

    public virtual DbSet<Ricoveri> Ricoveris { get; set; }

    public virtual DbSet<Ruoli> Ruolis { get; set; }

    public virtual DbSet<Utenti> Utentis { get; set; }

    public virtual DbSet<Vendite> Vendites { get; set; }

    public virtual DbSet<Visite> Visites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-QEIG4Q2\\SQLEXPRESS;Database=SocityPet;TrustServerCertificate=true;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animali>(entity =>
        {
            entity.HasKey(e => e.Idanimale).HasName("PK__ANIMALI__4C2D2C9D6CDDF7E4");

            entity.ToTable("ANIMALI");

            entity.Property(e => e.ColoreMantello).HasMaxLength(50);
            entity.Property(e => e.Datanascita).HasColumnType("datetime");
            entity.Property(e => e.Dataregistrazione).HasColumnType("datetime");
            entity.Property(e => e.FotoAnimale).HasMaxLength(255);
            entity.Property(e => e.NomeAnimale)
                .HasMaxLength(50)
                .HasColumnName("NomeANIMALE");
            entity.Property(e => e.NumMicrochip).HasMaxLength(50);
            entity.Property(e => e.Tipologia).HasMaxLength(50);

            entity.HasOne(d => d.IdUtenteNavigation).WithMany(p => p.Animalis)
                .HasForeignKey(d => d.IdUtente)
                .HasConstraintName("FK__ANIMALI__IdUtent__3C69FB99");
        });

        modelBuilder.Entity<Armadietti>(entity =>
        {
            entity.HasKey(e => e.IdArmadietto).HasName("PK__ARMADIET__ACE9365F12173C90");

            entity.ToTable("ARMADIETTI");

            entity.Property(e => e.Descrizione).HasMaxLength(100);
        });

        modelBuilder.Entity<Cassetti>(entity =>
        {
            entity.HasKey(e => e.IdCassetto).HasName("PK__CASSETTI__96096D42CA96182C");

            entity.ToTable("CASSETTI");

            entity.Property(e => e.Descrizione).HasMaxLength(100);

            entity.HasOne(d => d.IdArmadiettoNavigation).WithMany(p => p.Cassettis)
                .HasForeignKey(d => d.IdArmadietto)
                .HasConstraintName("FK__CASSETTI__IdArma__4E88ABD4");
        });

        modelBuilder.Entity<Dittafornitrice>(entity =>
        {
            entity.HasKey(e => e.IdDittaFornitrice).HasName("PK__DITTAFOR__87D1DCA18720EE2A");

            entity.ToTable("DITTAFORNITRICE");

            entity.Property(e => e.Indirizzo).HasMaxLength(100);
            entity.Property(e => e.NomeDitta).HasMaxLength(50);
            entity.Property(e => e.RecapitoDitta).HasMaxLength(100);
        });

        modelBuilder.Entity<Prodotti>(entity =>
        {
            entity.HasKey(e => e.IdProdotto).HasName("PK__PRODOTTI__C3D15F947D14710F");

            entity.ToTable("PRODOTTI");

            entity.HasIndex(e => e.Nomeprodotto, "UQ__PRODOTTI__8906935F92620055").IsUnique();

            entity.Property(e => e.FotoProdotto).HasMaxLength(255);
            entity.Property(e => e.Nomeprodotto).HasMaxLength(50);
            entity.Property(e => e.PossibiliUsi).HasMaxLength(500);
            entity.Property(e => e.Prezzo).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdDittaFornitriceNavigation).WithMany(p => p.Prodottis)
                .HasForeignKey(d => d.IdDittaFornitrice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prodotti_DittaFornitrice");

            entity.HasMany(d => d.IdRicettaMedicas).WithMany(p => p.IdProdottos)
                .UsingEntity<Dictionary<string, object>>(
                    "ProdottiRicette",
                    r => r.HasOne<Ricettemediche>().WithMany()
                        .HasForeignKey("IdRicettaMedica")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProdottiRicette_RicetteMediche"),
                    l => l.HasOne<Prodotti>().WithMany()
                        .HasForeignKey("IdProdotto")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ProdottiRicette_Prodotti"),
                    j =>
                    {
                        j.HasKey("IdProdotto", "IdRicettaMedica");
                        j.ToTable("ProdottiRicette");
                    });
        });

        modelBuilder.Entity<ProdottiInCassetto>(entity =>
        {
            entity.HasKey(e => e.IdProdottoInCassetto).HasName("PK__PRODOTTI__61A996C23016FF6D");

            entity.ToTable("PRODOTTI_IN_CASSETTO");

            entity.HasOne(d => d.IdCassettoNavigation).WithMany(p => p.ProdottiInCassettos)
                .HasForeignKey(d => d.IdCassetto)
                .HasConstraintName("FK__PRODOTTI___IdCas__52593CB8");

            entity.HasOne(d => d.IdProdottoNavigation).WithMany(p => p.ProdottiInCassettos)
                .HasForeignKey(d => d.IdProdotto)
                .HasConstraintName("FK__PRODOTTI___IdPro__5165187F");
        });

        modelBuilder.Entity<Ricettemediche>(entity =>
        {
            entity.HasKey(e => e.IdricettaMedica).HasName("PK__RICETTEM__6C10F3DD306BA524");

            entity.ToTable("RICETTEMEDICHE");

            entity.Property(e => e.DataPrescrizione).HasColumnType("datetime");
            entity.Property(e => e.Descrizione).HasMaxLength(500);

            entity.HasOne(d => d.IdUtenteNavigation).WithMany(p => p.Ricettemediches)
                .HasForeignKey(d => d.IdUtente)
                .HasConstraintName("FK__RICETTEME__IdUte__4316F928");

            entity.HasOne(d => d.IdVisitaNavigation).WithMany(p => p.Ricettemediches)
                .HasForeignKey(d => d.IdVisita)
                .HasConstraintName("FK__RICETTEME__IdVis__4222D4EF");
        });

        modelBuilder.Entity<Ricoveri>(entity =>
        {
            entity.HasKey(e => e.IdRicovero).HasName("PK__RICOVERI__FD10E7D667BECD15");

            entity.ToTable("RICOVERI");

            entity.Property(e => e.DataFinericovero).HasColumnType("datetime");
            entity.Property(e => e.DataInizioRicovero).HasColumnType("datetime");
            entity.Property(e => e.Dataregistrazionericovero)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsRicoveroAttivo).HasDefaultValue(true);
            entity.Property(e => e.PrezzoGiornalieroRicovero).HasColumnType("money");
            entity.Property(e => e.PrezzoTotaleRicovero)
                .HasComputedColumnSql("(datediff(day,[DataInizioRicovero],isnull([DataFinericovero],getdate()))*[PrezzoGiornalieroRicovero])", false)
                .HasColumnType("money");

            entity.HasOne(d => d.IdanimaleNavigation).WithMany(p => p.Ricoveris)
                .HasForeignKey(d => d.Idanimale)
                .HasConstraintName("FK__RICOVERI__Idanim__5AEE82B9");
        });

        modelBuilder.Entity<Ruoli>(entity =>
        {
            entity.HasKey(e => e.IdRuolo).HasName("PK__RUOLI__5F37071D5EDCA2F8");

            entity.ToTable("RUOLI");

            entity.Property(e => e.NomeRuolo).HasMaxLength(50);
        });

        modelBuilder.Entity<Utenti>(entity =>
        {
            entity.HasKey(e => e.IdUtente).HasName("PK__UTENTI__0816694B3D6C8C3A");

            entity.ToTable("UTENTI");

            entity.Property(e => e.Cognome).HasMaxLength(50);
            entity.Property(e => e.FotoUtente).HasMaxLength(255);
            entity.Property(e => e.Nome).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(255);

            entity.HasOne(d => d.IdRuoloNavigation).WithMany(p => p.Utentis)
                .HasForeignKey(d => d.IdRuolo)
                .HasConstraintName("FK__UTENTI__IdRuolo__398D8EEE");
        });

        modelBuilder.Entity<Vendite>(entity =>
        {
            entity.HasKey(e => e.IdVendita).HasName("PK__VENDITE__F4B898ADEBA46730");

            entity.ToTable("VENDITE");

            entity.Property(e => e.Cf)
                .HasMaxLength(16)
                .HasColumnName("CF");

            entity.HasOne(d => d.IdProdottoNavigation).WithMany(p => p.Vendites)
                .HasForeignKey(d => d.IdProdotto)
                .HasConstraintName("FK__VENDITE__IdProdo__5535A963");

            entity.HasOne(d => d.IdUtenteNavigation).WithMany(p => p.Vendites)
                .HasForeignKey(d => d.IdUtente)
                .HasConstraintName("FK__VENDITE__IdUtent__5629CD9C");

            entity.HasOne(d => d.IdricettaMedicaNavigation).WithMany(p => p.Vendites)
                .HasForeignKey(d => d.IdricettaMedica)
                .HasConstraintName("FK__VENDITE__Idricet__571DF1D5");
        });

        modelBuilder.Entity<Visite>(entity =>
        {
            entity.HasKey(e => e.IdVisita).HasName("PK__VISITE__020AC827685D9E76");

            entity.ToTable("VISITE");

            entity.Property(e => e.Anamnesi).HasMaxLength(500);
            entity.Property(e => e.DataVisita).HasColumnType("datetime");
            entity.Property(e => e.DescrizioneCura).HasMaxLength(500);
            entity.Property(e => e.PrezzoVisita).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdAnimaleNavigation).WithMany(p => p.Visites)
                .HasForeignKey(d => d.IdAnimale)
                .HasConstraintName("FK__VISITE__IdAnimal__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

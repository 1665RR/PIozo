using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace OZO.Models
{
    public partial class PI09Context : DbContext
    {
        public PI09Context()
        {
        }

        public PI09Context(DbContextOptions<PI09Context> options)
            : base(options)
        {
        }

        public virtual DbSet<NatjecajPartner> NatjecajPartner { get; set; }
        public virtual DbSet<Natječaji> Natječaji { get; set; }
        public virtual DbSet<Obrazovanje> Obrazovanje { get; set; }
        public virtual DbSet<Oprema> Oprema { get; set; }
        public virtual DbSet<OpremaIzvještaji> OpremaIzvještaji { get; set; }
        public virtual DbSet<Osoba> Osoba { get; set; }
        public virtual DbSet<Partner> Partner { get; set; }
        public virtual DbSet<PosaoOprema> PosaoOprema { get; set; }
        public virtual DbSet<Poslovi> Poslovi { get; set; }
        public virtual DbSet<PosloviIzvjestaji> PosloviIzvjestaji { get; set; }
        public virtual DbSet<ReferentniTip> ReferentniTip { get; set; }
        public virtual DbSet<Tvrtka> Tvrtka { get; set; }
        public virtual DbSet<Usluge> Usluge { get; set; }
        public virtual DbSet<UslugePartner> UslugePartner { get; set; }
        public virtual DbSet<ViewPartner> VwPartner { get; set; }
        public virtual DbSet<VwPoslovniPartner> VwPoslovniPartner { get; set; }
        public virtual DbSet<Zaposlenici> Zaposlenici { get; set; }
        public virtual DbSet<ZaposleniciZanimanja> ZaposleniciZanimanja { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NatjecajPartner>(entity =>
            {
                entity.HasKey(e => e.IdNatjecajPartner);

                entity.ToTable("NATJECAJ_PARTNER");

                entity.Property(e => e.IdNatjecajPartner)
                    .HasColumnName("ID_NATJECAJ_PARTNER")
                    .ValueGeneratedNever();

                entity.Property(e => e.IdNatječaji).HasColumnName("ID_NATJEČAJI");

                entity.Property(e => e.IdPartnera).HasColumnName("ID_PARTNERA");

                entity.HasOne(d => d.IdNatječajiNavigation)
                    .WithMany(p => p.NatjecajPartner)
                    .HasForeignKey(d => d.IdNatječaji)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NATJECAJ_PARTNER_NATJEČAJI");

                entity.HasOne(d => d.IdPartneraNavigation)
                    .WithMany(p => p.NatjecajPartner)
                    .HasForeignKey(d => d.IdPartnera)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NATJECAJ_PARTNER_PARTNER");
            });

            modelBuilder.Entity<Natječaji>(entity =>
            {
                entity.HasKey(e => e.IdNatječaji)
                    .HasName("PK_Natječaji");

                entity.ToTable("NATJEČAJI");

                entity.Property(e => e.IdNatječaji).HasColumnName("ID_NATJEČAJI");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .IsRequired()
                    .HasColumnName("OPIS")
                    .HasMaxLength(50);

                entity.Property(e => e.VremenskiRok)
                    .HasColumnName("VREMENSKI_ROK")
                    .HasColumnType("date");

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Natječaji)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .HasConstraintName("FK_Natječaji_REFERENTNI_TIP");
            });

            modelBuilder.Entity<Obrazovanje>(entity =>
            {
                entity.HasKey(e => e.IdObrazovanje);

                entity.ToTable("OBRAZOVANJE");

                entity.Property(e => e.IdObrazovanje).HasColumnName("ID_OBRAZOVANJE");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.NazivŠkole)
                    .HasColumnName("NAZIV_ŠKOLE")
                    .HasMaxLength(50);

                entity.Property(e => e.PoloženiTečaji)
                    .HasColumnName("POLOŽENI_TEČAJI")
                    .HasMaxLength(50);

                entity.Property(e => e.StručnaSprema)
                    .HasColumnName("STRUČNA_SPREMA")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdZaposleniciNavigation)
                    .WithMany(p => p.Obrazovanje)
                    .HasForeignKey(d => d.IdZaposlenici)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OBRAZOVANJE_ZAPOSLENICI");
            });

            modelBuilder.Entity<Oprema>(entity =>
            {
                entity.HasKey(e => e.IdOprema);

                entity.ToTable("OPREMA");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.Dostupnost).HasColumnName("DOSTUPNOST");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .IsRequired()
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.SlikaOpreme).HasColumnName("Slika_Opreme");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("STATUS")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Oprema)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OPREMA_REFERENTNI_TIP");
            });

            modelBuilder.Entity<OpremaIzvještaji>(entity =>
            {
                entity.HasKey(e => e.IdOpremaIzvještaji);

                entity.ToTable("OPREMA_IZVJEŠTAJI");

                entity.Property(e => e.IdOpremaIzvještaji).HasColumnName("ID_OPREMA_IZVJEŠTAJI");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.Sadržaj).HasColumnName("SADRŽAJ");

                entity.HasOne(d => d.IdOpremaNavigation)
                    .WithMany(p => p.OpremaIzvještaji)
                    .HasForeignKey(d => d.IdOprema)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OPREMA_IZVJEŠTAJI_OPREMA");
            });

           modelBuilder.Entity<Osoba>(entity =>
            {
                entity.HasKey(e => e.IdOsobe);

                entity.ToTable("OSOBA");

                entity.Property(e => e.IdOsobe)
                    .HasColumnName("ID_OSOBE")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ImeOsobe)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PrezimeOsobe)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdOsobeNavigation)
                    .WithOne(p => p.Osoba)
                    .HasForeignKey<Osoba>(d => d.IdOsobe)
                    .HasConstraintName("FK_OSOBA");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasKey(e => e.IdPartnera);

                entity.ToTable("PARTNER");

                entity.Property(e => e.IdPartnera)
                    .HasColumnName("ID_PARTNERA")
                    .ValueGeneratedOnAdd();


                entity.Property(e => e.AdrIsporuke)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.AdrPartnera)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IdTvrtke).HasColumnName("ID_TVRTKE");

                entity.Property(e => e.Mbr)
                    .IsRequired()
                    .HasColumnName("mbr")
                    .HasMaxLength(50);

                entity.Property(e => e.TipPartnera)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdTvrtkeNavigation)
                    .WithMany(p => p.Partner)
                    .HasForeignKey(d => d.IdTvrtke)
                    .HasConstraintName("FK_PARTNER_TVRTKA");
            });

            modelBuilder.Entity<PosaoOprema>(entity =>
            {
                entity.HasKey(e => e.IdPosaoOprema);

                entity.ToTable("POSAO_OPREMA");

                entity.Property(e => e.IdPosaoOprema).HasColumnName("ID_POSAO_OPREMA");

                entity.Property(e => e.IdOprema).HasColumnName("ID_OPREMA");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.HasOne(d => d.IdOpremaNavigation)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.IdOprema)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSAO_OPREMA_OPREMA");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.PosaoOprema)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSAO_OPREMA_POSLOVI");
            });

            modelBuilder.Entity<Poslovi>(entity =>
            {
                entity.HasKey(e => e.IdPoslovi);

                entity.ToTable("POSLOVI");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.IdNatječaji).HasColumnName("ID_NATJEČAJI");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.Property(e => e.Mjesto)
                    .HasColumnName("MJESTO")
                    .HasMaxLength(50);

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.Property(e => e.VrijemeTrajanja)
                    .HasColumnName("VRIJEME_TRAJANJA")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdNatječajiNavigation)
                    .WithMany(p => p.Poslovi)
                    .HasForeignKey(d => d.IdNatječaji)
                    .HasConstraintName("FK_POSLOVI_Natječaji");

                entity.HasOne(d => d.IdUslugeNavigation)
                    .WithMany(p => p.Poslovi)
                    .HasForeignKey(d => d.IdUsluge)
                    .HasConstraintName("FK_POSLOVI_USLUGE");
            });

            modelBuilder.Entity<PosloviIzvjestaji>(entity =>
            {
                entity.HasKey(e => e.IdPosloviIzvještaji)
                    .HasName("PK_POSLOVI_IZVJEŠTAJI");

                entity.ToTable("POSLOVI_IZVJESTAJI");

                entity.Property(e => e.IdPosloviIzvještaji).HasColumnName("ID_POSLOVI_IZVJEŠTAJI");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.Sadržaj).HasColumnName("SADRŽAJ");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.PosloviIzvjestaji)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_POSLOVI_IZVJEŠTAJI_POSLOVI");
            });

            modelBuilder.Entity<ReferentniTip>(entity =>
            {
                entity.HasKey(e => e.IdReferentniTip);

                entity.ToTable("REFERENTNI_TIP");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tvrtka>(entity =>
            {
                entity.HasKey(e => e.IdTvrtke);

                entity.ToTable("TVRTKA");

                entity.Property(e => e.IdTvrtke)
                    .HasColumnName("ID_TVRTKE")
                    .ValueGeneratedOnAdd();


                entity.Property(e => e.MbrTvrtke)
                    .IsRequired()
                    .HasColumnName("mbrTvrtke")
                    .HasMaxLength(50);

                entity.Property(e => e.NazivTvrtke)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Usluge>(entity =>
            {
                entity.HasKey(e => e.IdUsluge);

                entity.ToTable("USLUGE");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.Property(e => e.Cijena)
                    .HasColumnName("CIJENA")
                    .HasColumnType("money");

                entity.Property(e => e.IdReferentniTip).HasColumnName("ID_REFERENTNI_TIP");

                entity.Property(e => e.NazivUsluge)
                    .IsRequired()
                    .HasColumnName("NAZIV_USLUGE")
                    .HasMaxLength(50);

                entity.Property(e => e.Opis)
                    .HasColumnName("OPIS")
                    .HasMaxLength(50);

                entity.Property(e => e.VremenskiRok)
                    .HasColumnName("VREMENSKI_ROK")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.IdReferentniTipNavigation)
                    .WithMany(p => p.Usluge)
                    .HasForeignKey(d => d.IdReferentniTip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USLUGE_REFERENTNI_TIP");
            });

            modelBuilder.Entity<UslugePartner>(entity =>
            {
                entity.HasKey(e => e.IdUslugePartner)
                    .HasName("PK_USLUGE_POSLODAVAC");

                entity.ToTable("USLUGE_PARTNER");

                entity.Property(e => e.IdUslugePartner).HasColumnName("ID_USLUGE_PARTNER");

                entity.Property(e => e.IdPartnera).HasColumnName("ID_PARTNERA");

                entity.Property(e => e.IdUsluge).HasColumnName("ID_USLUGE");

                entity.HasOne(d => d.IdUslugeNavigation)
                    .WithMany(p => p.UslugePartner)
                    .HasForeignKey(d => d.IdUsluge)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_USLUGE_POSLODAVAC_USLUGE");
            });

            modelBuilder.Entity<ViewPartner>(entity =>
            {
                 entity.HasNoKey();

                entity.ToView("vw_Partner");

                entity.Property(e => e.IdPartnera).HasColumnName("ID_PARTNERA");

                entity.Property(e => e.Mbr)
                    .IsRequired()
                    .HasColumnName("mbr")
                    .HasMaxLength(50);

                entity.Property(e => e.Naziv).HasMaxLength(101);

                entity.Property(e => e.TipPartnera)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VwPoslovniPartner>(entity =>
            {
                 entity.HasNoKey();

                entity.ToView("vw_PoslovniPartner");

                entity.Property(e => e.IdPartnera).HasColumnName("ID_PARTNERA");

                entity.Property(e => e.Mbr)
                    .IsRequired()
                    .HasColumnName("mbr")
                    .HasMaxLength(50);

                entity.Property(e => e.Naziv).HasMaxLength(101);

                entity.Property(e => e.NazivTvrtke).HasMaxLength(50);

                entity.Property(e => e.TipPartnera)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Zaposlenici>(entity =>
            {
                entity.HasKey(e => e.IdZaposlenici);

                entity.ToTable("ZAPOSLENICI");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.DatumRođenja)
                    .HasColumnName("DATUM_ROĐENJA")
                    .HasColumnType("date");

                entity.Property(e => e.IdPoslovi).HasColumnName("ID_POSLOVI");

                entity.Property(e => e.Ime)
                    .HasColumnName("IME")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Prezime)
                    .HasColumnName("PREZIME")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.TrošakZaposlenika)
                    .HasColumnName("TROŠAK_ZAPOSLENIKA")
                    .HasColumnType("money");

                entity.HasOne(d => d.IdPosloviNavigation)
                    .WithMany(p => p.Zaposlenici)
                    .HasForeignKey(d => d.IdPoslovi)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZAPOSLENICI_POSLOVI");
            });

            modelBuilder.Entity<ZaposleniciZanimanja>(entity =>
            {
                entity.HasKey(e => e.IdZaposleniciZanimanja);

                entity.ToTable("ZAPOSLENICI_ZANIMANJA");

                entity.Property(e => e.IdZaposleniciZanimanja).HasColumnName("ID_ZAPOSLENICI_ZANIMANJA");

                entity.Property(e => e.IdZaposlenici).HasColumnName("ID_ZAPOSLENICI");

                entity.Property(e => e.Naziv)
                    .HasColumnName("NAZIV")
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdZaposleniciNavigation)
                    .WithMany(p => p.ZaposleniciZanimanja)
                    .HasForeignKey(d => d.IdZaposlenici)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZAPOSLENICI_ZANIMANJA_ZAPOSLENICI");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

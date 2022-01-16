﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;

namespace Projekat_WEB.Migrations
{
    [DbContext(typeof(AgencijaContext))]
    [Migration("20211230183036_V1")]
    partial class V1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Models.Dan", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Dan");
                });

            modelBuilder.Entity("Models.Majstor", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("JMBG")
                        .HasColumnType("int");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Majstor");
                });

            modelBuilder.Entity("Models.Posao", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Naziv")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Nedelja")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Posao");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DanID")
                        .HasColumnType("int");

                    b.Property<int>("Honorar")
                        .HasColumnType("int");

                    b.Property<int?>("MajstorID")
                        .HasColumnType("int");

                    b.Property<int?>("PosaoID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DanID");

                    b.HasIndex("MajstorID");

                    b.HasIndex("PosaoID");

                    b.ToTable("Spoj");
                });

            modelBuilder.Entity("Models.Spoj", b =>
                {
                    b.HasOne("Models.Dan", "Dan")
                        .WithMany("MajstoriPoslovi")
                        .HasForeignKey("DanID");

                    b.HasOne("Models.Majstor", "Majstor")
                        .WithMany("MajstorPosao")
                        .HasForeignKey("MajstorID");

                    b.HasOne("Models.Posao", "Posao")
                        .WithMany("PosaoMajstor")
                        .HasForeignKey("PosaoID");

                    b.Navigation("Dan");

                    b.Navigation("Majstor");

                    b.Navigation("Posao");
                });

            modelBuilder.Entity("Models.Dan", b =>
                {
                    b.Navigation("MajstoriPoslovi");
                });

            modelBuilder.Entity("Models.Majstor", b =>
                {
                    b.Navigation("MajstorPosao");
                });

            modelBuilder.Entity("Models.Posao", b =>
                {
                    b.Navigation("PosaoMajstor");
                });
#pragma warning restore 612, 618
        }
    }
}

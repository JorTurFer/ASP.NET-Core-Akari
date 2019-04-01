﻿// <auto-generated />
using System;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Akari_Net.Core.Migrations
{
    [DbContext(typeof(PacientesDbContext))]
    partial class PacientesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.8-servicing-32085")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.CalendarEvent", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .HasMaxLength(300);

                    b.Property<DateTime>("End");

                    b.Property<int?>("IdPaciente");

                    b.Property<int?>("IdTipoCita");

                    b.Property<bool>("IsFullDay");

                    b.Property<bool>("IsSMSSended");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("EventID");

                    b.HasIndex("IdPaciente");

                    b.HasIndex("IdTipoCita");

                    b.ToTable("CalendarEvents");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Paciente", b =>
                {
                    b.Property<int>("IdPaciente")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alergias");

                    b.Property<int>("CP")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("DNI");

                    b.Property<string>("Direccion");

                    b.Property<string>("Email");

                    b.Property<string>("Enfermedades");

                    b.Property<int>("IdPais")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<int>("IdProvincia")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<string>("Medicación");

                    b.Property<DateTime?>("Nacimiento");

                    b.Property<string>("Nombre")
                        .IsRequired();

                    b.Property<bool>("RGPD")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Telefono1");

                    b.Property<string>("Telefono2");

                    b.HasKey("IdPaciente");

                    b.HasIndex("IdPais");

                    b.HasIndex("IdProvincia");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Pais", b =>
                {
                    b.Property<int>("IdPais")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nombre");

                    b.HasKey("IdPais");

                    b.ToTable("Paises");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Provincia", b =>
                {
                    b.Property<int>("IdProvincia")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Nombre");

                    b.HasKey("IdProvincia");

                    b.ToTable("Provincias");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.TipoCita", b =>
                {
                    b.Property<int>("IdTipoCita")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("Tipo");

                    b.HasKey("IdTipoCita");

                    b.ToTable("TipoCitas");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.CalendarEvent", b =>
                {
                    b.HasOne("Akari_Net.Core.Areas.Pacientes.Models.Data.Paciente", "Paciente")
                        .WithMany("Citas")
                        .HasForeignKey("IdPaciente");

                    b.HasOne("Akari_Net.Core.Areas.Pacientes.Models.Data.TipoCita", "TipoCita")
                        .WithMany()
                        .HasForeignKey("IdTipoCita");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Paciente", b =>
                {
                    b.HasOne("Akari_Net.Core.Areas.Pacientes.Models.Data.Pais", "Pais")
                        .WithMany("Pacientes")
                        .HasForeignKey("IdPais")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Akari_Net.Core.Areas.Pacientes.Models.Data.Provincia", "Provincia")
                        .WithMany("Pacientes")
                        .HasForeignKey("IdProvincia")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

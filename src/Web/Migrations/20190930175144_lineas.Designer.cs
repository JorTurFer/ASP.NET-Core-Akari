﻿// <auto-generated />
using System;
using Akari_Net.Core.Areas.Pacientes.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Web.Migrations
{
    [DbContext(typeof(PatientsDbContext))]
    [Migration("20190930175144_lineas")]
    partial class lineas
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.CalendarEvent", b =>
                {
                    b.Property<int>("EventID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("CalendarEvents","Patients");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Paciente", b =>
                {
                    b.Property<int>("IdPaciente")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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

                    b.ToTable("Pacientes","Patients");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Pais", b =>
                {
                    b.Property<int>("IdPais")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("IdPais");

                    b.ToTable("Paises","Patients");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.Provincia", b =>
                {
                    b.Property<int>("IdProvincia")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre");

                    b.HasKey("IdProvincia");

                    b.ToTable("Provincias","Patients");
                });

            modelBuilder.Entity("Akari_Net.Core.Areas.Pacientes.Models.Data.TipoCita", b =>
                {
                    b.Property<int>("IdTipoCita")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Color");

                    b.Property<string>("Tipo");

                    b.HasKey("IdTipoCita");

                    b.ToTable("TipoCitas","Patients");
                });

            modelBuilder.Entity("Web.Areas.Pacientes.Data.FacturaLine", b =>
                {
                    b.Property<int>("IdLine")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cantidad");

                    b.Property<int>("IdFactura");

                    b.Property<int>("IdReferencia");

                    b.HasKey("IdLine");

                    b.HasIndex("IdFactura");

                    b.HasIndex("IdReferencia");

                    b.ToTable("FacturasLineas","Facturas");
                });

            modelBuilder.Entity("Web.Areas.Pacientes.Data.FacturasHeader", b =>
                {
                    b.Property<int>("IdFactura")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .IsRequired();

                    b.Property<double>("Descuento");

                    b.Property<DateTime>("Fecha");

                    b.Property<double>("IRPF");

                    b.Property<int>("IdPaciente");

                    b.HasKey("IdFactura");

                    b.HasIndex("IdPaciente");

                    b.ToTable("FacturasHeaders","Facturas");
                });

            modelBuilder.Entity("Web.Areas.Pacientes.Data.Referencia", b =>
                {
                    b.Property<int>("IdReferencia")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Concepto")
                        .IsRequired();

                    b.Property<string>("Identificador")
                        .IsRequired();

                    b.Property<double>("Precio");

                    b.HasKey("IdReferencia");

                    b.ToTable("Referencias","Facturas");
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

            modelBuilder.Entity("Web.Areas.Pacientes.Data.FacturaLine", b =>
                {
                    b.HasOne("Web.Areas.Pacientes.Data.FacturasHeader", "Factura")
                        .WithMany("Lineas")
                        .HasForeignKey("IdFactura")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Web.Areas.Pacientes.Data.Referencia", "Referencia")
                        .WithMany("Lineas")
                        .HasForeignKey("IdReferencia")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Web.Areas.Pacientes.Data.FacturasHeader", b =>
                {
                    b.HasOne("Akari_Net.Core.Areas.Pacientes.Models.Data.Paciente", "Paciente")
                        .WithMany("Facturas")
                        .HasForeignKey("IdPaciente")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

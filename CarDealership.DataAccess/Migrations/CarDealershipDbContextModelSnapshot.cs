﻿// <auto-generated />
using System;
using CarDealership.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    [DbContext(typeof(CarDealershipDbContext))]
    partial class CarDealershipDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoConfigurationEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AutoModelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("BodyTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ColorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("DriveTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EngineId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("AutoModelId");

                    b.HasIndex("BodyTypeId");

                    b.HasIndex("ColorId");

                    b.HasIndex("DriveTypeId");

                    b.HasIndex("EngineId");

                    b.ToTable("AutoConfigurations");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoModelEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BrandId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.ToTable("AutoModels");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.BodyTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("BodyTypes");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.BrandEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.CarEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AutoConfigurationId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("VIN")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AutoConfigurationId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.ColorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.CountryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.DriveTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("DriveTypes");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EngineEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Consumption")
                        .HasColumnType("integer");

                    b.Property<Guid>("EngineTypeId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int>("Power")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("TransmissionTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("EngineTypeId");

                    b.HasIndex("TransmissionTypeId");

                    b.ToTable("Engines");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EngineTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("EngineTypes");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EquipmentEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AutoModelId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ReleaseYear")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AutoModelId");

                    b.ToTable("Equipments");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EquipmentFeatureEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EquipmentId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FeatureId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("EquipmentId");

                    b.HasIndex("FeatureId");

                    b.ToTable("EquipmentFeatures");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.FeatureEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.TransmissionTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TransmissionTypes");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoConfigurationEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.AutoModelEntity", "AutoModel")
                        .WithMany("Configurations")
                        .HasForeignKey("AutoModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.BodyTypeEntity", "BodyType")
                        .WithMany("Configurations")
                        .HasForeignKey("BodyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.ColorEntity", "Color")
                        .WithMany("Configurations")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.DriveTypeEntity", "DriveType")
                        .WithMany("Configurations")
                        .HasForeignKey("DriveTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.EngineEntity", "Engine")
                        .WithMany("Configurations")
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AutoModel");

                    b.Navigation("BodyType");

                    b.Navigation("Color");

                    b.Navigation("DriveType");

                    b.Navigation("Engine");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoModelEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.BrandEntity", "Brand")
                        .WithMany("Models")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.BrandEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.CountryEntity", "Country")
                        .WithMany("Brands")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.CarEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.AutoConfigurationEntity", "AutoConfiguration")
                        .WithMany("Cars")
                        .HasForeignKey("AutoConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AutoConfiguration");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EngineEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.EngineTypeEntity", "EngineType")
                        .WithMany("Engines")
                        .HasForeignKey("EngineTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.TransmissionTypeEntity", "TransmissionType")
                        .WithMany("Engines")
                        .HasForeignKey("TransmissionTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EngineType");

                    b.Navigation("TransmissionType");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EquipmentEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.AutoModelEntity", "AutoModel")
                        .WithMany("Equipments")
                        .HasForeignKey("AutoModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AutoModel");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EquipmentFeatureEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.EquipmentEntity", "Equipment")
                        .WithMany("equipmentFeatures")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.FeatureEntity", "Feature")
                        .WithMany("featureEquipments")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoConfigurationEntity", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoModelEntity", b =>
                {
                    b.Navigation("Configurations");

                    b.Navigation("Equipments");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.BodyTypeEntity", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.BrandEntity", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.ColorEntity", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.CountryEntity", b =>
                {
                    b.Navigation("Brands");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.DriveTypeEntity", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EngineEntity", b =>
                {
                    b.Navigation("Configurations");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EngineTypeEntity", b =>
                {
                    b.Navigation("Engines");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.EquipmentEntity", b =>
                {
                    b.Navigation("equipmentFeatures");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.FeatureEntity", b =>
                {
                    b.Navigation("featureEquipments");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.TransmissionTypeEntity", b =>
                {
                    b.Navigation("Engines");
                });
#pragma warning restore 612, 618
        }
    }
}

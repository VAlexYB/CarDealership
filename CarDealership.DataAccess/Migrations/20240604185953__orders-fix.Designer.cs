﻿// <auto-generated />
using System;
using CarDealership.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CarDealership.DataAccess.Migrations
{
    [DbContext(typeof(CarDealershipDbContext))]
    [Migration("20240604185953__orders-fix")]
    partial class _ordersfix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CarDealership.DataAccess.Entities.Auth.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Value = "User"
                        },
                        new
                        {
                            Id = 2,
                            Value = "Manager"
                        },
                        new
                        {
                            Id = 3,
                            Value = "SeniorManager"
                        },
                        new
                        {
                            Id = 4,
                            Value = "Admin"
                        });
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.Auth.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstCardDigits")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<bool?>("HasLinkedCard")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LastCardDigits")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

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

                    b.Property<Guid>("EquipmentId")
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

                    b.HasIndex("EquipmentId");

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

                    b.HasIndex("BrandId", "Name")
                        .IsUnique();

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

                    b.HasIndex("Value")
                        .IsUnique();

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

                    b.HasIndex("Name")
                        .IsUnique();

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

                    b.HasIndex("VIN")
                        .IsUnique();

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

                    b.HasIndex("Value")
                        .IsUnique();

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

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.DealEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CarId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DealDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Deals");
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

                    b.HasIndex("Value")
                        .IsUnique();

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

                    b.HasIndex("Value")
                        .IsUnique();

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

                    b.HasIndex("FeatureId");

                    b.HasIndex("EquipmentId", "FeatureId")
                        .IsUnique();

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

            modelBuilder.Entity("CarDealership.DataAccess.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AutoConfigurationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CompleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("ManagerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AutoConfigurationId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Orders");
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

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("TransmissionTypes");
                });

            modelBuilder.Entity("RoleEntityUserEntity", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("integer");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uuid");

                    b.HasKey("RolesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("RoleEntityUserEntity");
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

                    b.HasOne("CarDealership.DataAccess.Entities.EquipmentEntity", "Equipment")
                        .WithMany("Configurations")
                        .HasForeignKey("EquipmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AutoModel");

                    b.Navigation("BodyType");

                    b.Navigation("Color");

                    b.Navigation("DriveType");

                    b.Navigation("Engine");

                    b.Navigation("Equipment");
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

            modelBuilder.Entity("CarDealership.DataAccess.Entities.DealEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.CarEntity", "Car")
                        .WithMany()
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.Auth.UserEntity", "Customer")
                        .WithMany("CustomerDeals")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.Auth.UserEntity", "Manager")
                        .WithMany("ManagedDeals")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Car");

                    b.Navigation("Customer");

                    b.Navigation("Manager");
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

            modelBuilder.Entity("CarDealership.DataAccess.Entities.OrderEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.AutoConfigurationEntity", "AutoConfiguration")
                        .WithMany("Orders")
                        .HasForeignKey("AutoConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.Auth.UserEntity", "Customer")
                        .WithMany("CustomerOrders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.Auth.UserEntity", "Manager")
                        .WithMany("ManagedOrders")
                        .HasForeignKey("ManagerId");

                    b.Navigation("AutoConfiguration");

                    b.Navigation("Customer");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("RoleEntityUserEntity", b =>
                {
                    b.HasOne("CarDealership.DataAccess.Entities.Auth.RoleEntity", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CarDealership.DataAccess.Entities.Auth.UserEntity", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.Auth.UserEntity", b =>
                {
                    b.Navigation("CustomerDeals");

                    b.Navigation("CustomerOrders");

                    b.Navigation("ManagedDeals");

                    b.Navigation("ManagedOrders");
                });

            modelBuilder.Entity("CarDealership.DataAccess.Entities.AutoConfigurationEntity", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Orders");
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
                    b.Navigation("Configurations");

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

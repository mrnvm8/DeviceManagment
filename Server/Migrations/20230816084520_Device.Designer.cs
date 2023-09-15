﻿// <auto-generated />
using System;
using DeviceManagment.Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeviceManagment.Server.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230816084520_Device")]
    partial class Device
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.21")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DeviceManagment.Shared.Auth.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("UserId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("DepartmentName")
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("OfficeId")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId");

                    b.HasIndex("OfficeId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Condition")
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("DeviceIMEINo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceSerialNo")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("DeviceTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PucharsedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("PurchasedPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("DeviceId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("DeviceTypeId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("DeviceManagment.Shared.DeviceLoans", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeviceId")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("ReturnToUserId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("UserId");

                    b.ToTable("DeviceLoans");
                });

            modelBuilder.Entity("DeviceManagment.Shared.DeviceType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("DeviceTypes");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<bool>("IsEmployeeActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("WorkEmail")
                        .HasColumnType("varchar(50)");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PersonId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Office", b =>
                {
                    b.Property<int>("OfficeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<string>("OfficeName")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("OfficeId");

                    b.ToTable("Offices");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("PersonId");

                    b.ToTable("People");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Auth.User", b =>
                {
                    b.HasOne("DeviceManagment.Shared.Employee", "Employee")
                        .WithMany("Users")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Department", b =>
                {
                    b.HasOne("DeviceManagment.Shared.Office", "Offices")
                        .WithMany("Departments")
                        .HasForeignKey("OfficeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Offices");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Device", b =>
                {
                    b.HasOne("DeviceManagment.Shared.Department", "Department")
                        .WithMany("Devices")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceManagment.Shared.DeviceType", "DeviceType")
                        .WithMany("Devices")
                        .HasForeignKey("DeviceTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("DeviceType");
                });

            modelBuilder.Entity("DeviceManagment.Shared.DeviceLoans", b =>
                {
                    b.HasOne("DeviceManagment.Shared.Device", "Device")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceManagment.Shared.Employee", "Employee")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceManagment.Shared.Auth.User", "User")
                        .WithMany("DevicesLoans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("Employee");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Employee", b =>
                {
                    b.HasOne("DeviceManagment.Shared.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DeviceManagment.Shared.Person", "Person")
                        .WithMany("Employees")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Auth.User", b =>
                {
                    b.Navigation("DevicesLoans");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Department", b =>
                {
                    b.Navigation("Devices");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Device", b =>
                {
                    b.Navigation("DevicesLoans");
                });

            modelBuilder.Entity("DeviceManagment.Shared.DeviceType", b =>
                {
                    b.Navigation("Devices");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Employee", b =>
                {
                    b.Navigation("DevicesLoans");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Office", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("DeviceManagment.Shared.Person", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
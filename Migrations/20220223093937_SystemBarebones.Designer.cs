﻿// <auto-generated />
using System;
using Inventory_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory_API.Migrations
{
    [DbContext(typeof(RestContext))]
    [Migration("20220223093937_SystemBarebones")]
    partial class SystemBarebones
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inventory_API.Data.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<int?>("ListId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Quantity")
                        .HasColumnType("real");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("categoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ListId");

                    b.HasIndex("categoryId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.List", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("List");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RoomUser", b =>
                {
                    b.Property<int>("RoomsId")
                        .HasColumnType("int");

                    b.Property<int>("SharedWithId")
                        .HasColumnType("int");

                    b.HasKey("RoomsId", "SharedWithId");

                    b.HasIndex("SharedWithId");

                    b.ToTable("RoomUser");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Item", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.Item", null)
                        .WithMany("Items")
                        .HasForeignKey("ItemId");

                    b.HasOne("Inventory_API.Data.Entities.List", null)
                        .WithMany("Items")
                        .HasForeignKey("ListId");

                    b.HasOne("Inventory_API.Data.Entities.Category", "category")
                        .WithMany()
                        .HasForeignKey("categoryId");

                    b.Navigation("category");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.List", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany("Lists")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.User", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany("Friends")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("RoomUser", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.Room", null)
                        .WithMany()
                        .HasForeignKey("RoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("SharedWithId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Item", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.List", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("Lists");
                });
#pragma warning restore 612, 618
        }
    }
}

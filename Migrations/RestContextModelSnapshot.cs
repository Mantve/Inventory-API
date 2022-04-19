﻿// <auto-generated />
using System;
using Inventory_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Inventory_API.Migrations
{
    [DbContext(typeof(RestContext))]
    partial class RestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("AuthorUsername")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Description")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUsername");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Comments")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<int?>("ParentItemId")
                        .HasColumnType("int");

                    b.Property<float>("Quantity")
                        .HasColumnType("real");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.Property<decimal>("Value")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ParentItemId");

                    b.HasIndex("RoomId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.List", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<int>("ItemCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUsername");

                    b.ToTable("Lists");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.ListItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("ParentListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("ParentListId");

                    b.ToTable("ListItems");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorUsername")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Contents")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("MessageType")
                        .HasColumnType("int");

                    b.Property<string>("RecipientUsername")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUsername");

                    b.HasIndex("RecipientUsername");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Reminder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorUsername")
                        .HasColumnType("nvarchar(32)");

                    b.Property<bool>("Expired")
                        .HasColumnType("bit");

                    b.Property<int?>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Reason")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("ReminderTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("RepeatFrequency")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUsername");

                    b.HasIndex("ItemId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AuthorUsername")
                        .IsRequired()
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("AuthorUsername");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Subscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte[]>("Auth")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Endpoint")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("P256dh")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Username");

                    b.ToTable("Subscriptions");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.User", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Username");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RoomUser", b =>
                {
                    b.Property<int>("AccessibleRoomsId")
                        .HasColumnType("int");

                    b.Property<string>("SharedWithUsername")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("AccessibleRoomsId", "SharedWithUsername");

                    b.HasIndex("SharedWithUsername");

                    b.ToTable("RoomUser");
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.Property<string>("FriendOfUsername")
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("FriendsUsername")
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("FriendOfUsername", "FriendsUsername");

                    b.HasIndex("FriendsUsername");

                    b.ToTable("UserUser");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Category", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUsername");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Item", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Inventory_API.Data.Entities.Item", "ParentItem")
                        .WithMany("Items")
                        .HasForeignKey("ParentItemId");

                    b.HasOne("Inventory_API.Data.Entities.Room", "Room")
                        .WithMany("Items")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("ParentItem");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.List", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "Author")
                        .WithMany("Lists")
                        .HasForeignKey("AuthorUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.ListItem", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.HasOne("Inventory_API.Data.Entities.List", "ParentList")
                        .WithMany("Items")
                        .HasForeignKey("ParentListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("ParentList");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Message", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUsername");

                    b.HasOne("Inventory_API.Data.Entities.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientUsername");

                    b.Navigation("Author");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Reminder", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorUsername");

                    b.HasOne("Inventory_API.Data.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId");

                    b.Navigation("Author");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Room", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "Author")
                        .WithMany("CreatedRooms")
                        .HasForeignKey("AuthorUsername")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.Subscription", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", "User")
                        .WithMany("Subscriptions")
                        .HasForeignKey("Username");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RoomUser", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.Room", null)
                        .WithMany()
                        .HasForeignKey("AccessibleRoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("SharedWithUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUser", b =>
                {
                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("FriendOfUsername")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Inventory_API.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("FriendsUsername")
                        .OnDelete(DeleteBehavior.ClientCascade)
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

            modelBuilder.Entity("Inventory_API.Data.Entities.Room", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Inventory_API.Data.Entities.User", b =>
                {
                    b.Navigation("CreatedRooms");

                    b.Navigation("Lists");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}

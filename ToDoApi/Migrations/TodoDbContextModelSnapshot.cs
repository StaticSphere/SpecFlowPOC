﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ToDoApi.Domain;

namespace ToDoApi.Migrations
{
    [DbContext(typeof(TodoDbContext))]
    partial class TodoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ToDoApi.Domain.Entities.TodoItem", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean")
                        .HasColumnName("completed");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)")
                        .HasColumnName("description");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("due_date");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_todo_item");

                    b.ToTable("todo_item");
                });

            modelBuilder.Entity("ToDoApi.Domain.Entities.TodoTag", b =>
                {
                    b.Property<int?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Color")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("color");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("modified_on");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_todo_tag");

                    b.ToTable("todo_tag");
                });

            modelBuilder.Entity("todo_item_tags", b =>
                {
                    b.Property<int>("todo_item_id")
                        .HasColumnType("integer")
                        .HasColumnName("todo_item_id");

                    b.Property<int>("todo_tag_id")
                        .HasColumnType("integer")
                        .HasColumnName("todo_tag_id");

                    b.HasKey("todo_item_id", "todo_tag_id")
                        .HasName("pk_todo_item_tags");

                    b.HasIndex("todo_tag_id")
                        .HasDatabaseName("ix_todo_item_tags_todo_tag_id");

                    b.ToTable("todo_item_tags");
                });

            modelBuilder.Entity("todo_item_tags", b =>
                {
                    b.HasOne("ToDoApi.Domain.Entities.TodoItem", null)
                        .WithMany()
                        .HasForeignKey("todo_item_id")
                        .HasConstraintName("fk_todo_item_tags_todo_item_todo_item_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ToDoApi.Domain.Entities.TodoTag", null)
                        .WithMany()
                        .HasForeignKey("todo_tag_id")
                        .HasConstraintName("fk_todo_item_tags_todo_tags_todo_tag_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

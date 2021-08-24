using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Domain.Configuration
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.ToTable("todo_item");

            builder.Property(x => x.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.HasMany(x => x.Tags)
                .WithMany(x => x.Items)
                .UsingEntity<Dictionary<string, object>>("todo_item_tags",
                    x => x.HasOne<TodoTag>().WithMany().HasForeignKey("todo_tag_id"),
                    x => x.HasOne<TodoItem>().WithMany().HasForeignKey("todo_item_id"));
        }
    }
}

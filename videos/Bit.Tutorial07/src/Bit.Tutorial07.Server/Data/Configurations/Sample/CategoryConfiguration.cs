﻿using Bit.Tutorial07.Server.Models.Sample;

namespace Bit.Tutorial07.Server.Data.Configurations.Sample;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(p => p.Name).IsUnique();

        builder.HasData(
            new() { Id = 1, Name = "Ford", Color = "#FFCD56" },
            new() { Id = 2, Name = "Nissan", Color = "#FF6384" },
            new() { Id = 3, Name = "Benz", Color = "#4BC0C0" },
            new() { Id = 4, Name = "BMW", Color = "#FF9124" },
            new() { Id = 5, Name = "Tesla", Color = "#2B88D8" });
    }
}
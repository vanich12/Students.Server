﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Students.Models.ReferenceModels;

namespace Students.DBCore.Configuration;

internal class StatusRequestConfiguration : IEntityTypeConfiguration<StatusRequest>
{
  public void Configure(EntityTypeBuilder<StatusRequest> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Id)
      .IsRequired()
      .ValueGeneratedOnAdd();
  }
}
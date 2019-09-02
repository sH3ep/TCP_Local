﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TPC.Api.Model;

namespace TPC.Api.Persistence.Configurations
{
    public static class FeatureConfiguration
    {
        public static void Configure(this EntityTypeBuilder<Feature> builder)
        {
            builder.ToTable("Features");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasColumnName("Id").IsRequired();
            builder.Property(x => x.ProjectId).HasColumnName("ProjectId");
            builder.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
            builder.Property(x => x.ModificationDate).HasColumnName("ModificationDate");
            builder.Property(x => x.Title).HasColumnName("Title");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.AssignedUserId).HasColumnName("AssignedUserId");
            builder.Property(x => x.StatusId).HasColumnName("StatusId");
            builder.Property(x => x.PriorityId).HasColumnName("PriorityId");
        }
    }
}

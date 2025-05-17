using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Students.Models.ReferenceModels;
using Students.Models.WebModels;

namespace Students.DBCore.Configuration
{
    public class PendingRequestConfiguration : IEntityTypeConfiguration<PendingRequest>
    {
        public void Configure(EntityTypeBuilder<PendingRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); 

            builder.Property(x => x.Birthday)
                .IsRequired();

            builder.Property(x => x.Education)
                .IsRequired();

            builder.Property(x => x.Agreement)
                .IsRequired();

            builder.Property(x => x.EducationLevel)
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired();

            builder.Property(x => x.Address)
                .IsRequired();

            builder.Property(x => x.Phone)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.Property(x => x.IT_Experience)
                .IsRequired();


            //builder.HasOne(te => te.TypeEducation)
            //    .WithMany()
            //    .HasForeignKey(te => te.TypeEducationId);


            //builder.HasMany(r => r.Requests)
            //    .WithOne(p => p.Person)
            //    .HasForeignKey(p => p.PersonId);
        }
    }
}
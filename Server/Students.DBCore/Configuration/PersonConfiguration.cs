using Microsoft.EntityFrameworkCore;
using Students.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Students.DBCore.Configuration
{
    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Family)
                .IsRequired();

            builder.Property(x => x.BirthDate)
                .IsRequired();

            builder.Property(x => x.Sex)
                .IsRequired();

            builder.Property(x => x.Address)
                .IsRequired();

            builder.Property(x => x.Phone)
                .IsRequired();

            builder.Property(x => x.Email)
                .IsRequired();

            builder.HasIndex(x => x.Phone).IsUnique();

            builder.HasIndex(x => x.Email).IsUnique();

            builder.Property(x => x.IT_Experience)
                .IsRequired();

            builder.Property(x => x.ScopeOfActivityLevelOneId)
                .IsRequired();

            builder.HasOne(te => te.TypeEducation)
                .WithMany()
                .HasForeignKey(te => te.TypeEducationId);


            builder.HasMany(r => r.Requests)
                .WithOne(p => p.Person)
                .HasForeignKey(p => p.PersonId);
        }
    }
}
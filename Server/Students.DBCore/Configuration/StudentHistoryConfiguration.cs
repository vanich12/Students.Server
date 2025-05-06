using Microsoft.EntityFrameworkCore;
using Students.Models.ReferenceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Students.Models;

namespace Students.DBCore.Configuration
{
    internal class StudentHistoryConfiguration: IEntityTypeConfiguration<StudentHistory>
    {
        public void Configure(EntityTypeBuilder<StudentHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ChangeDate).IsRequired();
            builder.HasOne(x => x.Student);
        }
    }
}

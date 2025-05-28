using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Students.Models;

namespace Students.DBCore.Configuration
{
    internal class PersonHistoryConfiguration: IEntityTypeConfiguration<PersonHistory>
    {
        public void Configure(EntityTypeBuilder<PersonHistory> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ChangeDate).IsRequired();
            builder.HasOne(x => x.Person)
                .WithMany()
                .HasForeignKey(x=>x.PersonId);
        }
    }
}

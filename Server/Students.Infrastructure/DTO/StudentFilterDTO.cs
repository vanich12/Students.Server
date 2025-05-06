using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO
{
    public class StudentFilterDto
    {
        [FromQuery(Name = "birthDate")]
        public DateOnly? BirthDate { get; set; }

        [FromQuery(Name = "groupId")]
        public Guid? GroupId { get; set; }

        [FromQuery(Name = "educationProgram")]
        public Guid? ProgramEducationId { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO.FilterDTO
{
    public class RequestFilterDTO
    {
        [FromQuery(Name = "groupId")]
        public Guid? GroupId { get; set; }

        [FromQuery(Name = "withoutGroups")]
        public bool WithoutGroups { get; set; }
        [FromQuery(Name = "notInThisGroup")]
        public bool NotInThisGroup { get; set; }
        [FromQuery(Name = "hasStudent")]
        public bool HasStudent { get; set; }
        [FromQuery(Name = "hasGroup")] public bool HasGroup { get; set; }

        public Guid? EducationProgramId { get; set; }

    }
}

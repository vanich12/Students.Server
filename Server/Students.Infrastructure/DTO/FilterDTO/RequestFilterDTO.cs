using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO.FilterDTO
{
    public class RequestFilterDTO
    {
        [FromQuery(Name = "groupId")]
        public Guid? GroupId { get; set; }

        [FromQuery(Name = "withoutGroups")]
        public bool WithoutGroups { get; set; }
    }
}

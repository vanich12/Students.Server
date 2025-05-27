using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO.FilterDTO
{
    public class PersonFilterDTO
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        [FromQuery(Name = "family")]
        public string? Family { get; set; }
        [FromQuery(Name = "patron")]
        public string? Patron { get; set; }

        [FromQuery(Name = "birthDate")]
        public DateOnly? BirthDate { get; set; }
        [FromQuery(Name = "email")]
        public string? Email { get; set; }
        [FromQuery(Name = "phone")]
        public string? PhoneNumber { get; set; }

        [FromQuery(Name = "matchAny")] 
        public bool MatchAnyCriterion { get; set; } = false;
    }
}
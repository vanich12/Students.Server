using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
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
    }
}
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
{
    public class RequestFilterDTO
    {
        [FromQuery(Name = "groupId")]
        public Guid? GroupId { get; set; }

        [FromQuery(Name = "withoutGroups")]
        public bool WithoutGroups { get; set; }
    }
}

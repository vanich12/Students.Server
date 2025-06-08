using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO.FilterDTO
{
    public class PersonHistoryFilterDTO
    {
        [FromQuery(Name = "personId")]
        public Guid? PersonId { get; set; }

    }
}

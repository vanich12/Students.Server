﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Students.Infrastructure.DTO.FilterDTO
{
    public class PendingRequestFilterDTO
    {
        [FromQuery(Name = "IsArchive")]
        public bool? IsArchive { get; set; }

        [FromQuery(Name = "educationProgram")]
        public Guid? ProgramEducationId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Models;

namespace Students.Application.Services.Interfaces
{
    public interface IStudentService:IGenericService<Student>
    {
        Task<Student> UpdateStudent(Guid studentId, StudentDTO form);
    }
}

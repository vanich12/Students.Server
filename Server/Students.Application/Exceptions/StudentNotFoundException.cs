using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Application.Exceptions
{
    public class StudentNotFoundException : Exception
    {
        public StudentNotFoundException(Guid studentId)
            : base($"Student with ID '{studentId}' not found.")
        {
            StudentId = studentId;
        }

        public Guid StudentId { get; }
    }
}
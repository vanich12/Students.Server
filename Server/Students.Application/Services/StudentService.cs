using Microsoft.Extensions.Logging;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.Interfaces;
using Students.Models;

namespace Students.Application.Services
{
    public class StudentService(
        IStudentRepository studentRepository,
        IPersonRepository personRepository,
        ILogger<Student> logger) : GenericService<Student>(studentRepository, logger), IStudentService
    {
        public async Task<Student> UpdateStudent(Guid studentId, StudentDTO form)
        {
            try
            {
                var student = await studentRepository.FindById(studentId);
                if (student is null)
                    throw new ArgumentException("Error while trying find student by Id for update");

                var person = await personRepository.FindById(student.PersonId.Value);
                person.Name = form.Name;
                person.Family = form.Family;
                person.Patron = form.Patron;
                person.Address = form.Address;
                person.Email = form.Email;
                person.BirthDate = form.BirthDate.Value;
                person.IT_Experience = form.IT_Experience;

                if (person is null)
                    throw new ArgumentException("Error while trying find person by Id for update");
                var newPerson = await personRepository.Patch(student.PersonId.Value, person);

                var newStudent = await studentRepository.Update(studentId, student);

                return newStudent;
            }
            catch (ArgumentException ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                throw;
            }
        }
    }
}
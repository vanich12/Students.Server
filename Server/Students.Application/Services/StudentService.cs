using Microsoft.Extensions.Logging;
using Students.Application.Exceptions;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Infrastructure.Extension.Pagination;
using Students.Infrastructure.Interfaces;
using Students.Infrastructure.Storages;
using Students.Models;

namespace Students.Application.Services
{
    public class StudentService(
        IStudentRepository studentRepository,
        IPersonRepository personRepository,
        IRequestRepository requestRepository,
        ILogger<Student> logger) : GenericService<Student>(studentRepository, logger), IStudentService
    {
        /// <summary>
        /// Список историй обучений студента
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LearningHistoryDTO>> GetLearningHistoryOfStudent(Guid studentId,
            RequestFilterDTO? filter)
        {
            try
            {
                var student = await studentRepository.FindById(studentId);
                if (student is null) throw new StudentNotFoundException(studentId);

                var items = await requestRepository.Get(p => p.StudentId == studentId);

                return items.Select(entity => Mapper.RequestToLearningHistoryDTO(entity).Result).ToList();
            }
            catch (StudentNotFoundException ex)
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
        /// <summary>
        /// Обновление студента в базе
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="form"></param>
        /// <returns></returns>
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
                person.Sex = form.Sex.Value;

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
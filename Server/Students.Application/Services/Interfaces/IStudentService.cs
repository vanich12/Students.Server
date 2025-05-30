using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Students.Infrastructure.DTO;
using Students.Infrastructure.DTO.FilterDTO;
using Students.Models;

namespace Students.Application.Services.Interfaces
{
    public interface IStudentService:IGenericService<Student>
    {
        /// <summary>
        /// Получение истории обучения студента
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<IEnumerable<LearningHistoryDTO>> GetLearningHistoryOfStudent(Guid studentId,
            RequestFilterDTO? filter);
            /// <summary>
            /// Обновление студента
            /// </summary>
            /// <param name="studentId"></param>
            /// <param name="form"></param>
            /// <returns></returns>
        Task<Student> UpdateStudent(Guid studentId, StudentDTO form);
    }
}

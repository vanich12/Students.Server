using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Infrastructure.DTO
{
    public class LearningHistoryDTO
    {
        public Guid Id { get; set; }
        /// <summary>
        ///  Id образовательной программы.
        /// </summary>
        public Guid? EducationProgramId { get; set; }

        /// <summary>
        /// Образовательная программа.
        /// </summary>
        public string? EducationProgram { get; set; }
        /// <summary>
        /// Статус заявки.
        /// </summary>
        public string? StatusRequest { get; set; }

        /// <summary>
        /// Идентификатор статуса студента.
        /// </summary>
        public Guid? StatusRequestId { get; set; }

        /// <summary>
        /// Id группы
        /// </summary>
        public Guid? GroupId { get; set; }
        /// <summary>
        /// Название группы
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Начало обучения.
        /// </summary>
        public DateOnly? GroupStartDate { get; set; }

        /// <summary>
        /// Конец обучения.
        /// </summary>
        public DateOnly? GroupEndDate { get; set; }

    }
}

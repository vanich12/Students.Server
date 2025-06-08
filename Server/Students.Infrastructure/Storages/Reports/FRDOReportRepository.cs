using System.Diagnostics;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.ReferenceModels;
using Students.Models.ReportsModel;

namespace Students.Infrastructure.Storages.Reports;

/// <summary>
/// Отчет ПФДО.
/// </summary>
public class FRDOReportRepository : IReportRepository<FRDOModel>
{
    #region Поля и свойства

    /// <summary>
    /// Репозитории для отчета
    /// </summary>
    private readonly IStudentRepository _studentRepository;

    private readonly IGenericRepository<EducationForm> _educationFormRepository;
    private readonly IGenericRepository<EducationProgram> _educationProgramRepository;
    private readonly IGenericRepository<KindDocumentRiseQualification> _kindDocumentRiseQualificationRepository;
    private readonly IDocumentRiseQualificationRepository _documentRiseQualificationRepository;
    private readonly IGenericRepository<FEAProgram> _FEAProgramFormRepository;
    private readonly IGenericRepository<StatusRequest> _StatusRequestRepository;
    private readonly IGenericRepository<FinancingType> _financingTypeRepository;
    private readonly IGenericRepository<Group> _groupRepository;
    private readonly IPersonRepository _personRepository;
    private readonly IRequestRepository _requestRepository;
    private readonly IGenericRepository<ScopeOfActivity> _scopeOfActivityRepository;
    private readonly IGenericRepository<TypeEducation> _typeEducationRepository;
    private readonly IGenericRepository<StudentStatus> _studentStatusRepository;

    #endregion

    #region Методы

    /// <summary>
    /// Данные для формирования отчета.
    /// </summary>
    /// <returns>Список данных.</returns>
    public async Task<List<FRDOModel>> Get(DateOnly startDate, DateOnly endDate)
    {
        DateTime startDateTime = startDate.ToDateTime(TimeOnly.MinValue);
        DateTime endDateTime = endDate.AddDays(1).ToDateTime(TimeOnly.MinValue);
        var completedStatusId = (await _StatusRequestRepository.GetOne(s => s.Name == "Обучение завершено"))?.Id;
        var completedRequests =
            await _requestRepository.Get(x =>
                completedStatusId.HasValue && x.StatusRequestId == completedStatusId &&
                x.DocumentRiseQualification.Date >= startDateTime && x.DocumentRiseQualification.Date <= endDateTime);
        var typeEducationList = await _typeEducationRepository.GetAll();

        List<FRDOModel> reports = new();
        foreach (var request in completedRequests)
        {
            var student = request.Student;
            if (student is null)
                throw new NullReferenceException("Для составления отчета требуется студент, он равен null");

            var person = await _personRepository.FindById(student!.PersonId!.Value);
            if (person is null)
                throw new NullReferenceException("Для составления отчета требуется персона, она равна null");

            var typeEducation = typeEducationList.FirstOrDefault(x => x.Id == person.TypeEducationId);
            var educationProgram = await _educationProgramRepository.GetOne(x => x.Id == request.EducationProgramId);

            reports.Add(new FRDOModel
            {
                FormEducation = educationProgram?.EducationForm?.Name,
                DocumentNumber = student.DocumentNumber,
                RegistrationNumber = request.RegistrationNumber,
                NameAdditionalProfessionalProgram = educationProgram?.Name,
                RecipientLastName = person.Family,
                SourceFundingForTraining = educationProgram.FinancingType.SourceName,
                DateOfIssueDocument = student.DateTakeDiplom.ToString(),
                RecipientName = person.Name,
                RecipientPatronymic = person.Patron,
                RecipientDateBirth = person.BirthDate,
                RecipientGender = person.Sex.ToString(),
                RecipientSNILS = student.SNILS,
                SurnameIndicatedHE = student.FullNameDocument,
                SeriesHE = student.DocumentSeries,
                NumberHE = student.DocumentNumber,
                LevelEducationHE = typeEducation?.Name,
                NameQualification = student.Speciality,
            });
        }

        return reports;

        //return (from student in listStudents
        //    let typeEducation = typeEducationList.FirstOrDefault(x => x.Id == person.TypeEducationId)
        //  select new FRDOModel
        //  { 
        //    RecipientLastName = person.Family,
        //    RecipientName = person.Name,
        //    RecipientPatronymic = person.Patron,
        //    RecipientDateBirth = person.BirthDate,
        //    RecipientGender = person.Sex.ToString(),
        //    RecipientSNILS = student.SNILS,
        //    SurnameIndicatedHE = student.FullNameDocument,
        //    SeriesHE = student.DocumentSeries,
        //    NumberHE = student.DocumentNumber,
        //    LevelEducationHE = typeEducation.Name,
        //    NameQualification = student.Speciality,
        //  }).ToList();
    }

    #endregion

    #region Конструкторы

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="studentRepository">Репозиторий студентов.</param>
    /// <param name="educationFormRepository">Репозиторий форм обучения.</param>
    /// <param name="educationProgramRepository">Репозиторий образовательных программ.</param>
    /// <param name="kindDocumentRiseQualificationRepository">Репозиторий видов документов повышения квалификации.</param>
    /// <param name="fEAProgramFormRepository">Репозиторий ВЭД программ.</param>
    /// <param name="fStatusRequestRepository">Репозиторий статусов заявок.</param>
    /// <param name="financingTypeRepository">Репозиторий типов финансирования.</param>
    /// <param name="groupRepository">Репозиторий групп.</param>
    /// <param name="requestRepository">Репозиторий заявок.</param>
    /// <param name="scopeOfActivityRepository">Репозиторий сфер деятельности.</param>
    /// <param name="typeEducationRepository">Репозиторий типов образования.</param>
    /// <param name="studentStatusRepository">Репозиторий статусов студента.</param>
    public FRDOReportRepository(
        IStudentRepository studentRepository,
        IGenericRepository<EducationForm> educationFormRepository,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IGenericRepository<KindDocumentRiseQualification> kindDocumentRiseQualificationRepository,
        IGenericRepository<FEAProgram> fEAProgramFormRepository,
        IGenericRepository<StatusRequest> fStatusRequestRepository,
        IGenericRepository<FinancingType> financingTypeRepository,
        IGenericRepository<Group> groupRepository,
        IRequestRepository requestRepository,
        IPersonRepository personRepository,
        IGenericRepository<ScopeOfActivity> scopeOfActivityRepository,
        IGenericRepository<TypeEducation> typeEducationRepository,
        IGenericRepository<StudentStatus> studentStatusRepository
    )
    {
        _studentRepository = studentRepository;
        _educationFormRepository = educationFormRepository;
        _educationProgramRepository = educationProgramRepository;
        _kindDocumentRiseQualificationRepository = kindDocumentRiseQualificationRepository;
        _FEAProgramFormRepository = fEAProgramFormRepository;
        _StatusRequestRepository = fStatusRequestRepository;
        _financingTypeRepository = financingTypeRepository;
        _groupRepository = groupRepository;
        _personRepository = personRepository;
        _requestRepository = requestRepository;
        _scopeOfActivityRepository = scopeOfActivityRepository;
        _typeEducationRepository = typeEducationRepository;
        _studentStatusRepository = studentStatusRepository;
    }

    #endregion
}
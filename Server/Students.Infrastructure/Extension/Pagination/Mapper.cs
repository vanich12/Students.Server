using Students.Infrastructure.DTO;
using Students.Infrastructure.Interfaces;
using Students.Models;
using Students.Models.Enums;
using Students.Models.ReferenceModels;
using Students.Models.WebModels;

namespace Students.Infrastructure.Extension.Pagination;

/// <summary>
/// Класс содержит методы для преобразования данных.
/// </summary>
public static class Mapper
{
    /// <summary>
    /// Преобразование вебхука в заявку на обучение.
    /// </summary>
    /// <param name="form">Данные, полученные из минцифры.</param>
    /// <param name="educationProgramRepository">Репозиторий образовательных программ.</param>
    /// <param name="statusRequestRepository">Репозиторий статусов заявок.</param>
    /// <returns>Заявка.</returns>
    public static async Task<Request> WebhookToRequest(RequestWebhook form,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IGenericRepository<StatusRequest> statusRequestRepository)
    {
        return new Request
        {
            Id = Guid.NewGuid(),
            Email = form.Email,
            Phone = form.Phone,
            EducationProgramId =
                (await educationProgramRepository.GetOne(x => x.Name?.ToLower() == form.Education))?.Id,
            StatusRequestId = (await statusRequestRepository.GetOne(x => x.Name?.ToLower() == "новая заявка"))?.Id,
            Agreement = Convert.ToBoolean(Convert.ToInt32(form.Agreement))
        };
    }

    /// <summary>
    /// Преобразование вебхука (данных от минцифры) в студента. Подумать над RequestWebhook, возможно сделать 2 его варианта (второй, состоящий из слова test / test  для установки связи между минцифрой и нашим сервисом).
    /// </summary>
    /// <param name="form">Вебхук (данне от минцифры).</param>
    /// <param name="studentRepository">Репозиторий студентов.</param>
    /// <param name="typeEducationRepository">Репозиторий типов образований.</param>
    /// <param name="scopeOfActivityRepository">Репозиторий сферы деятельности.</param>
    public static async Task<Person> WebhookToStudent(RequestWebhook form,
        IGenericRepository<TypeEducation> typeEducationRepository,
        IGenericRepository<ScopeOfActivity> scopeOfActivityRepository)
    {
        var fio = form.Name.Split(" ");
        return new Person
        {
            Address = form.Address,
            Family = fio.FirstOrDefault() ?? "",
            Name = fio.Length > 1
                ? fio[1]
                : "",
            Patron = fio.LastOrDefault() == fio.FirstOrDefault()
                ? ""
                : fio.LastOrDefault(),

            BirthDate = DateOnly.Parse(form.Birthday),
            IT_Experience = form.IT_Experience,
            Email = form.Email,
            Phone = form.Phone,
            TypeEducationId = (await typeEducationRepository.GetOne(x => x.Name?.ToLower() == form.EducationLevel))?.Id,
            ScopeOfActivityLevelOneId =
                (await scopeOfActivityRepository.GetOne(x => x.Id == Guid.Parse(form.ScopeOfActivityLevelOneId!)))!.Id,
            ScopeOfActivityLevelTwoId =
                (await scopeOfActivityRepository.GetOne(x => x.Id == Guid.Parse(form.ScopeOfActivityLevelTwoId!)))!.Id,
            Sex = default
            //Добавить в вебхук список, недостающих параметров, тут вставлять при наличии заполнения данных
            //Speciality = form.
            //Не хватает поля в вебхуке
            //.Projects = form.
            //CreatedAt = DateTime.Now,
        };
    }

    /// <summary>
    /// Преобразование Request в DTO.
    /// </summary>
    /// <param name="form">Заявка.</param>
    /// <returns>DTO заявки.</returns>
    public static async Task<RequestsDTO> RequestToRequestDTO(Request form)
    {
        return new RequestsDTO
        {
            Id = form.Id,
            StudentId = form.Person?.Id,
            StudentFullName = form.Person?.FullName ?? "",
            family = form.Person?.Family,
            name = form.Person?.Name,
            patron = form.Person?.Patron,
            StatusRequest = form.Status?.Name,
            StatusRequestId = form.StatusRequestId,
            EducationProgram = form.EducationProgram?.Name,
            EducationProgramId = form.EducationProgramId,
            TypeEducation = form.Person?.TypeEducation?.Name,
            TypeEducationId = form.Person?.TypeEducationId,
            speciality = form.Student?.Speciality,
            IT_Experience = form.Person?.IT_Experience,
            projects = form.Student?.Projects,
            statusEntrancExams = form.StatusEntrancExams ?? 0,
            BirthDate = form.Person?.BirthDate,
            Age = form.Person?.Age,
            Address = form.Person?.Address,
            phone = form.Person?.Phone,
            Email = form.Person?.Email,
            ScopeOfActivityLevelOneId = form.Person?.ScopeOfActivityLevelOneId,
            ScopeOfActivityLevelTwoId = form.Person?.ScopeOfActivityLevelTwoId,
            agreement = form.Agreement,
            trained = form.Orders != null && form.Orders!.Any(x => x.KindOrder?.Name?.ToLower() == "о зачислении")
        };
    }

    public static async Task<LearningHistoryDTO> RequestToLearningHistoryDTO(Request form)
    {
        var groupStudent = form.GroupStudent;
        return new LearningHistoryDTO()
        {
            Id = Guid.NewGuid(),
            EducationProgram = form.EducationProgram?.Name,
            EducationProgramId = form.EducationProgramId,
            StatusRequest = form.Status?.Name,
            StatusRequestId = form.StatusRequestId,
            GroupId = groupStudent?.Group?.Id,
            GroupName = groupStudent?.Group?.Name,
            GroupStartDate = groupStudent?.Group?.StartDate,
            GroupEndDate = groupStudent?.Group?.EndDate
        };
    }

    /// <summary>
    /// Преобразование Student в DTO.
    /// </summary>
    /// <param name="student">Студент.</param>
    /// <returns>DTO студента.</returns>
    public static async Task<StudentDTO> StudentToStudentDTO(Student student)
    {
        var groupStudent = student.GroupStudent?.FirstOrDefault();

        return new StudentDTO
        {
            Id = student.Id,
            Family = student.Person?.Family,
            Name = student.Person?.Name,
            Patron = student.Person?.Patron,
            FullName = student.Person?.FullName,
            BirthDate = student.Person?.BirthDate,
            Address = student.Person?.Address,
            Phone = student.Person?.Phone,
            Email = student.Person?.Email,
            SNILS = student.SNILS,
            Sex = student.Person?.Sex,
            Nationality = student.Person?.Nationality,
            TypeEducationId = student.Person?.TypeEducationId,
            Speciality = student.Speciality,
            IT_Experience = student.Person?.IT_Experience,
            RequestId = groupStudent?.Request?.Id,
            StatusRequestId = groupStudent?.Request?.StatusRequestId,
            StatusRequestName = groupStudent?.Request?.Status?.Name,
            EducationProgramId = groupStudent?.Group?.EducationProgramId,
            ProgramName = groupStudent?.Group?.EducationProgram?.Name,
            ScopeOfActivityLevelTwoId = student.Person?.ScopeOfActivityLevelTwoId,
            ScopeOfActivityLevelOneId = student.Person?.ScopeOfActivityLevelOneId,
            GroupId = groupStudent?.Group?.Id,
            GroupName = groupStudent?.Group?.Name,
            GroupStartDate = groupStudent?.Group?.StartDate,
            GroupEndDate = groupStudent?.Group?.EndDate
        };
    }

    public static async Task StudentDTOToStudent(StudentDTO form, Student student)
    {
        student.SNILS = form.SNILS;
        student.Speciality = form.Speciality;
    }

    public static async Task<PersonDTO> PersonToPersonDTO(Person person)
    {
        return new PersonDTO
        {
            Id = person.Id,
            PersonName = person.Name,
            PersonFamily = person.Family,
            PersonFullName = person.FullName,
            BirthDate = person.BirthDate,
            Email = person.Email,
            PersonPatron = person.Patron,
            PhoneNumber = person.Phone
        };
    }

    public static async Task<Person> NewPersonDTOToPerson(NewPersonDTO form)
    {
        return new Person()
        {
            Phone = form.Phone ?? String.Empty,
            Email = form.Email ?? String.Empty,
            BirthDate = form.BirthDate.Value,
            Family = form.Family ?? String.Empty,
            Name = form.Name ?? String.Empty,
            Patron = form.Patron,
            Sex = form.Sex,
            IT_Experience = form.It_Experience,
            Address = form.Address,
            ScopeOfActivityLevelOneId = form.ScopeOfActivityLevelOneId,
            ScopeOfActivityLevelTwoId = form.ScopeOfActivityLevelTwoId,
            TypeEducationId = form.TypeEducationId
        };
    }

    public static async Task<Person> PendingRequestToPerson(PendingRequest form)
    {
        return new Person()
        {
            Phone = form.Phone ?? String.Empty,
            Email = form.Email ?? String.Empty,
            BirthDate = DateOnly.Parse(form.Birthday),
            Family = form.Family ?? String.Empty,
            Name = form.Name ?? String.Empty,
            Patron = form.Patron,
            Sex = form.Sex,
            IT_Experience = form.IT_Experience,
            Address = form.Address,
            ScopeOfActivityLevelOneId = Guid.Parse(form.ScopeOfActivityLevelOneId),
            ScopeOfActivityLevelTwoId = Guid.Parse(form.ScopeOfActivityLevelTwoId),
        };
    }

    /// <summary>
    /// Преобразование NewRequestDTO в заявку.
    /// </summary>
    /// <param name="form">DTO новой заявки.</param>
    /// <param name="_statusRequestRepository">Репозиторий статусов заявок.</param>
    /// <returns>Заявка.</returns>
    public static async Task<Request> NewRequestDTOToRequest(NewRequestDTO form,
        IGenericRepository<StatusRequest> _statusRequestRepository)
    {
        return new Request
        {
            //Id = requestDTO.Id ?? default,
            //StudentId = requestDTO.StudentId,
            EducationProgramId = form.educationProgramId,
            //DocumentRiseQualificationId = requestDTO.
            StatusRequestId = (await _statusRequestRepository.GetOne(x => x.Name?.ToLower() == "новая заявка"))?.Id,
            StatusEntrancExams = (StatusEntrancExams)form.statusEntrancExams,
            Email = form.email,
            Phone = form.phone,
            Agreement = form.agreement
        };
    }

    /// <summary>
    /// Преобразование NewRequestDTO в заявку.
    /// </summary>
    /// <param name = "form" > DTO новой заявки.</param>
    /// <param name = "_statusRequestRepository" > Репозиторий статусов заявок.</param>
    /// <returns>Заявка.</returns>
    public static async Task<RequestsDTO> PendingRequestToRequestDTO(PendingRequest form,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IGenericRepository<TypeEducation> typeEducationRepository)
    {
        Guid scopeActivityFirstLevelId;
        Guid.TryParse(form.ScopeOfActivityLevelOneId, out scopeActivityFirstLevelId);


        Guid scopeActivitySecondLevelId;
        Guid.TryParse(form.ScopeOfActivityLevelTwoId, out scopeActivitySecondLevelId);

        return new RequestsDTO
        {
            Id = form.Id,
            StudentFullName = $"{form.Name} {form.Family} {form.Patron}",
            family = form.Family,
            name = form.Name,
            patron = form.Patron,
            EducationProgram = form.Education,
            EducationLevel = form.EducationLevel,
            EducationProgramId =
                (await educationProgramRepository.GetOne(x => x.Name?.ToLower() == form.Education))?.Id,
            IT_Experience = form.IT_Experience,
            BirthDate = DateOnly.FromDateTime(Convert.ToDateTime(form.Birthday)),
            Address = form.Address,
            phone = form.Phone,
            Email = form.Email,
            ScopeOfActivityLevelOneId = scopeActivityFirstLevelId,
            ScopeOfActivityLevelTwoId = scopeActivitySecondLevelId,
            TypeEducationId = (await typeEducationRepository.GetOne(x => x.Name?.ToLower() == form.EducationLevel))?.Id,
            agreement = form.Agreement,
            IsArchive = form.IsArchive
        };
    }

    public static async Task<PendingRequest> RequestDTOToPendingRequest(RequestsDTO form)

    {
        return new PendingRequest()
        {
            Phone = form.phone ?? String.Empty,
            Email = form.Email ?? String.Empty,
            Family = form.family ?? String.Empty,
            Name = form.name ?? String.Empty,
            Patron = form.patron ?? String.Empty,
            Education = form.EducationProgram ?? String.Empty,
            Address = form.Address ?? String.Empty,
            Agreement = form.agreement,
            Birthday = form.BirthDate.ToString() ?? String.Empty,
            IT_Experience = form.IT_Experience ?? String.Empty,
            EducationLevel = form.EducationLevel ?? String.Empty,
            ScopeOfActivityLevelOneId = form.ScopeOfActivityLevelOneId.ToString(),
            ScopeOfActivityLevelTwoId = form.ScopeOfActivityLevelTwoId.ToString(),
            IsArchive = form.IsArchive ?? false
        };
    }

    public static async Task<Request> PendingRequestToRequest(PendingRequest form,
        IGenericRepository<EducationProgram> educationProgramRepository,
        IGenericRepository<StatusRequest> statusRequestRepository)
    {
        return new Request()
        {
            Email = form.Email,
            Phone = form.Phone,
            Agreement = Convert.ToBoolean(form.Agreement),
            EducationProgramId =
                (await educationProgramRepository.GetOne(x => x.Name?.ToLower() == form.Education))?.Id,
            StatusRequestId = (await statusRequestRepository.GetOne(x => x.Name?.ToLower() == "новая заявка"))?.Id,
        };
    }

    /// <summary>
    /// Преобразование NewRequestDTO в студента.
    /// </summary>
    /// <param name="form">DTO новой заявки.</param>
    /// <returns>Студент.</returns>
    //public static async Task<Students.Models.Student> NewRequestDTOToStudent(NewRequestDTO form)
    //{
    //  return new Students.Models.Student
    //  {
    //    Address = form.address,
    //    Family = form.family ?? "",
    //    Name = form.name,
    //    Patron = form.patron,

    //    BirthDate = form.birthDate,
    //    IT_Experience = form.iT_Experience,
    //    Email = form.email,
    //    Phone = form.phone ?? "",
    //    Sex = SexHuman.Men,
    //    TypeEducationId = form.typeEducationId,
    //    ScopeOfActivityLevelOneId = form.scopeOfActivityLevelOneId,
    //    ScopeOfActivityLevelTwoId = form.scopeOfActivityLevelTwoId
    //  };
    //}
    public static async Task<Person> NewRequestDTOToPerson(NewRequestDTO form)
    {
        return new Person()
        {
            Address = form.address,
            Family = form.family ?? "",
            Name = form.name,
            Patron = form.patron,
            BirthDate = form.birthDate,
            IT_Experience = form.iT_Experience,
            Email = form.email,
            Phone = form.phone ?? "",
            Sex = SexHuman.Men,
            TypeEducationId = form.typeEducationId,
            ScopeOfActivityLevelOneId = form.scopeOfActivityLevelOneId,
            ScopeOfActivityLevelTwoId = form.scopeOfActivityLevelTwoId
        };
    }


    /// <summary>
    /// Преобразование RequestDTO в студента.
    /// </summary>
    /// <param name="form">DTO заявки.</param>
    /// <returns>Студент.</returns>
    public static async Task<Person> RequestDTOToPerson(RequestsDTO form)
    {
        return new Person
        {
            Family = form.family!,
            Name = form.name,
            Patron = form.patron,
            BirthDate = (DateOnly)form.BirthDate!,
            Sex = default,
            Address = form.Address!,
            Phone = form.phone!,
            Email = form.Email!,
            //Projects = form.projects,
            IT_Experience = form.IT_Experience!,
            TypeEducationId = form.TypeEducationId,
            //Ебать-кололить
            ScopeOfActivityLevelOneId = Guid.Parse("a5e1e718-4747-47f4-b7c3-08e56bb7ea34"),
            //Speciality = form.speciality
        };
    }

    public static Order OrderDTOToOrder(OrderDTO form)
    {
        return new Order
        {
            Date = DateTime.Now,
            KindOrderId = form.KindOrderId!.Value,
            RequestId = form.RequestId,
        };
    }
}
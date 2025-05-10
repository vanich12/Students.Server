using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Students.DBCore.Migrations
{
    /// <inheritdoc />
    public partial class MigrateData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EducationForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FEAPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FEAPrograms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancingTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KindDocumentRiseQualifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KindDocumentRiseQualifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KindOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KindOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScopesOfActivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NameOfScope = table.Column<string>(type: "text", nullable: true),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    ScopeOfActivityParentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScopesOfActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScopesOfActivity_ScopesOfActivity_ScopeOfActivityParentId",
                        column: x => x.ScopeOfActivityParentId,
                        principalTable: "ScopesOfActivity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StatusRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeEducation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeEducation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentRiseQualifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    KindDocumentRiseQualificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentRiseQualifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentRiseQualifications_KindDocumentRiseQualifications_K~",
                        column: x => x.KindDocumentRiseQualificationId,
                        principalTable: "KindDocumentRiseQualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EducationPrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<double>(type: "double precision", nullable: false),
                    HoursCount = table.Column<int>(type: "integer", nullable: false),
                    EducationFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    KindDocumentRiseQualificationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsModularProgram = table.Column<bool>(type: "boolean", nullable: false),
                    FEAProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    FinancingTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCollegeProgram = table.Column<bool>(type: "boolean", nullable: false),
                    IsArchive = table.Column<bool>(type: "boolean", nullable: false),
                    IsNetworkProgram = table.Column<bool>(type: "boolean", nullable: false),
                    IsDOTProgram = table.Column<bool>(type: "boolean", nullable: false),
                    IsFullDOTProgram = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EducationPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_EducationForms_EducationFormId",
                        column: x => x.EducationFormId,
                        principalTable: "EducationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_FEAPrograms_FEAProgramId",
                        column: x => x.FEAProgramId,
                        principalTable: "FEAPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EducationPrograms_FinancingTypes_FinancingTypeId",
                        column: x => x.FinancingTypeId,
                        principalTable: "FinancingTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EducationPrograms_KindDocumentRiseQualifications_KindDocume~",
                        column: x => x.KindDocumentRiseQualificationId,
                        principalTable: "KindDocumentRiseQualifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Family = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Patron = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Sex = table.Column<int>(type: "integer", nullable: false),
                    Nationality = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    IT_Experience = table.Column<string>(type: "text", nullable: false),
                    TypeEducationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScopeOfActivityLevelOneId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScopeOfActivityLevelTwoId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_ScopesOfActivity_ScopeOfActivityLevelOneId",
                        column: x => x.ScopeOfActivityLevelOneId,
                        principalTable: "ScopesOfActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_ScopesOfActivity_ScopeOfActivityLevelTwoId",
                        column: x => x.ScopeOfActivityLevelTwoId,
                        principalTable: "ScopesOfActivity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_TypeEducation_TypeEducationId",
                        column: x => x.TypeEducationId,
                        principalTable: "TypeEducation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    EducationProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Groups_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SNILS = table.Column<string>(type: "text", nullable: true),
                    Projects = table.Column<string>(type: "text", nullable: true),
                    Disability = table.Column<bool>(type: "boolean", nullable: true),
                    Speciality = table.Column<string>(type: "text", nullable: true),
                    FullNameDocument = table.Column<string>(type: "text", nullable: true),
                    DocumentSeries = table.Column<string>(type: "text", nullable: true),
                    DocumentNumber = table.Column<string>(type: "text", nullable: true),
                    DateTakeDiplom = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastChangedByUserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    PersonId = table.Column<Guid>(type: "uuid", nullable: true),
                    EducationProgramId = table.Column<Guid>(type: "uuid", nullable: true),
                    DocumentRiseQualificationId = table.Column<Guid>(type: "uuid", nullable: true),
                    DataNumberDogovor = table.Column<string>(type: "text", nullable: true),
                    StatusRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentStatusId = table.Column<Guid>(type: "uuid", nullable: true),
                    StatusEntrancExams = table.Column<int>(type: "integer", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: true),
                    IsAlreadyStudied = table.Column<bool>(type: "boolean", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Agreement = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_DocumentRiseQualifications_DocumentRiseQualificati~",
                        column: x => x.DocumentRiseQualificationId,
                        principalTable: "DocumentRiseQualifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_EducationPrograms_EducationProgramId",
                        column: x => x.EducationProgramId,
                        principalTable: "EducationPrograms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_StatusRequests_StatusRequestId",
                        column: x => x.StatusRequestId,
                        principalTable: "StatusRequests",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_StudentStatuses_StudentStatusId",
                        column: x => x.StudentStatusId,
                        principalTable: "StudentStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Requests_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StudentHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OldFamily = table.Column<string>(type: "text", nullable: false),
                    NewFamily = table.Column<string>(type: "text", nullable: false),
                    OldName = table.Column<string>(type: "text", nullable: false),
                    NewName = table.Column<string>(type: "text", nullable: false),
                    OldPatron = table.Column<string>(type: "text", nullable: false),
                    NewPatron = table.Column<string>(type: "text", nullable: false),
                    ChangeDate = table.Column<DateOnly>(type: "date", nullable: false),
                    ChangeType = table.Column<string>(type: "text", nullable: false),
                    LastChangedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentHistory_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GroupStudent",
                columns: table => new
                {
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupStudent", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_GroupStudent_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupStudent_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupStudent_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    KindOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_KindOrders_KindOrderId",
                        column: x => x.KindOrderId,
                        principalTable: "KindOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EducationForms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0241c1ac-bb5b-4ca1-bb46-89ba1e0c4287"), "Очная" },
                    { new Guid("77c07268-346c-443c-8a21-9ba091c828fd"), "Очно-заочная" },
                    { new Guid("9fd0638c-3976-42b0-8782-06ed3f9ca0db"), "Заочная" }
                });

            migrationBuilder.InsertData(
                table: "FEAPrograms",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("07b93879-1903-4c4f-9b63-19cd9a357c79"), "Транспортировка и хранение" },
                    { new Guid("0a2720fe-6381-4920-9fc1-765487de0c53"), "Деятельность в области здравоохранения и социальных услуг" },
                    { new Guid("2e0a8801-529d-406a-8b06-09dee812f0fd"), "Деятельность по операциям с недвижимым имуществом" },
                    { new Guid("2eaab406-6302-4707-be8b-f6de40bec96e"), "Строительство" },
                    { new Guid("313349b3-c3d2-42af-add6-c5fc6fc5f238"), "Торговля оптовая и розничная; ремонт автотранспортных средств и мотоциклов" },
                    { new Guid("33869d2e-968b-4928-a485-bcb01b9821d2"), "Деятельность экстерриториальных организаций и органов" },
                    { new Guid("3620d6b8-e3dd-407b-8f22-9f73162c0f09"), "Деятельность административная и сопутствующие дополнительные услуги" },
                    { new Guid("42c8476f-62f7-461e-b059-f3c2edd38f40"), "Обеспечение электрической энергией, газом и паром; кондиционирование воздуха" },
                    { new Guid("543ee5ea-587a-4ad3-9011-0e0040314112"), "Деятельность финансовая и страховая" },
                    { new Guid("55fca356-ecd3-40ed-b2b3-6262d685f321"), "Водоснабжение, водоотведение, организация сбора и утилизации отходов, деятельность по ликвидации загрязнений" },
                    { new Guid("57782aff-9f4c-4f83-a967-3229285bf140"), "Добыча полезных ископаемых" },
                    { new Guid("5b564193-b96a-4fd8-bfcd-f348e68694cb"), "Государственное управление и обеспечение военной безопасности; социальное обеспечение" },
                    { new Guid("6977c645-b2de-453e-b0e1-75fed0f06986"), "Деятельность домашних хозяйств как работодателей; недифференцированная деятельность частных домашних хозяйств" },
                    { new Guid("8a2953b7-d2f6-4bc8-813b-c0f4d1a928ae"), "Предоставление прочих видов услуг" },
                    { new Guid("8cc81b3a-3681-4bf8-bf1a-a62b1d1775fa"), "Сельское, лесное хозяйство, охота, рыболовство и рыбоводство" },
                    { new Guid("9a123e90-7122-49af-8e24-9aca964b39cb"), "Образование" },
                    { new Guid("a9ede175-ff97-49fa-a42a-d481a6e40008"), "Деятельность профессиональная, научная и техническая" },
                    { new Guid("bb4d092c-0a13-4e61-ad74-bab54565467a"), "Обрабатывающие производства" },
                    { new Guid("c841a174-36b3-40b9-b96a-78e7f40db406"), "Деятельность гостиниц и предприятий общественного питания" },
                    { new Guid("ceff5624-b542-4a93-aca9-0ded3cde2146"), "Деятельность в области культуры, спорта, организации досуг и развлечений" },
                    { new Guid("d6667aea-5077-491e-9fc6-75aba2b5c040"), "Деятельность в области информации и связи" }
                });

            migrationBuilder.InsertData(
                table: "FinancingTypes",
                columns: new[] { "Id", "SourceName" },
                values: new object[,]
                {
                    { new Guid("0457cc26-6b4f-472b-bdbf-a9be3599e931"), "За счет бюджетных ассигнований бюджетов субъектов РФ" },
                    { new Guid("06784e4c-dc85-4f51-9ee4-c87d3e478db4"), "За счет бюджетных ассигнований федерального бюджета" },
                    { new Guid("1f155a86-9bd1-4cbe-bc5e-829171cfb92b"), "За счет собственных средств организации" },
                    { new Guid("34de84cf-271d-4546-80f7-3a9a65b3830b"), "За счет бюджетных ассигнований местных бюджетов" },
                    { new Guid("533020ac-b4ae-4d7c-a946-fa5ff0b95633"), "По договорам за счет средств физических лиц" },
                    { new Guid("66d030a8-7441-4d40-840c-d5d44c3d634e"), "По договорам за счет средств юридических лиц " }
                });

            migrationBuilder.InsertData(
                table: "KindDocumentRiseQualifications",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("aa7a8325-4b0d-4dd2-bedc-2c4a065ab332"), "Удостоверение о повышении квалификации" },
                    { new Guid("f3963a72-8d77-47cc-85e5-0e46c1846f15"), "Диплом о профессиональной переподготовке" }
                });

            migrationBuilder.InsertData(
                table: "KindOrders",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("753df8b7-2d6f-4499-9f86-563771f016c1"), "О зачислении" },
                    { new Guid("c929e14d-e657-4b95-873c-0746f4edc68e"), "Об отчислении" }
                });

            migrationBuilder.InsertData(
                table: "ScopesOfActivity",
                columns: new[] { "Id", "Level", "NameOfScope", "ScopeOfActivityParentId" },
                values: new object[,]
                {
                    { new Guid("5f450e00-d584-4736-882b-1b6ada2484bc"), 1, "Незанятые лица по направлению службы занятости", null },
                    { new Guid("a5e1e718-4747-47f4-b7c3-08e56bb7ea34"), 1, "Другие", null },
                    { new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e"), 1, "Работники образовательных организаций", null },
                    { new Guid("e768a213-0421-4c6f-85b8-0069882870c6"), 1, "Работники предприятий и организаций", null },
                    { new Guid("ed3df49f-e714-4940-a151-458616ee7d84"), 1, "Гос. Служащие", null }
                });

            migrationBuilder.InsertData(
                table: "StatusRequests",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("0daf618a-7a2f-4099-bbbb-e5323f9921f7"), "Завершил" },
                    { new Guid("2d466d99-995c-473a-abcc-6260c6a2340a"), "Обучение" },
                    { new Guid("a8003ef9-b86d-4b63-9e41-d01720752b80"), "В архиве" },
                    { new Guid("d2b3c504-1890-43f4-a351-22eea9b8dc08"), "Новая заявка" },
                    { new Guid("d8ae2c61-3cd5-410f-a182-10f8f03f1500"), "Не соответствует" },
                    { new Guid("e51930d7-b466-4188-8b50-7b0013d95a55"), "Вступительное испытание" },
                    { new Guid("f42bbf66-14e0-44aa-b15b-605994370ffb"), "Отчислен" }
                });

            migrationBuilder.InsertData(
                table: "StudentStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("69b4aaff-b67f-483d-98e3-98d39da93d7a"), "Обучается" },
                    { new Guid("cc2de503-af1c-4c9e-b228-6c4178217169"), "Отчислен" },
                    { new Guid("fb1ec3f7-75a8-4a84-bdb6-db7a582e05ae"), "Окончил обучение" }
                });

            migrationBuilder.InsertData(
                table: "TypeEducation",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3b7bf44e-a9a1-46d6-aadc-7acff647d24f"), "Студент СПО" },
                    { new Guid("5cca2c3c-85b9-41ac-bf0d-73ba0cbaa833"), "Студент ВО" },
                    { new Guid("a4f6d736-28e7-4a63-845e-24e62b433fc1"), "Высшее образование" },
                    { new Guid("f87eaad5-5d84-45ce-b862-8da5c45ead5b"), "Среднее профессиональное образование" }
                });

            migrationBuilder.InsertData(
                table: "DocumentRiseQualifications",
                columns: new[] { "Id", "Date", "KindDocumentRiseQualificationId", "Number" },
                values: new object[,]
                {
                    { new Guid("5dde5f20-a247-45b5-b989-800a858d0b70"), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("aa7a8325-4b0d-4dd2-bedc-2c4a065ab332"), "2" },
                    { new Guid("b28d1f29-0aa9-4209-8bab-cd49a8ad548d"), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("f3963a72-8d77-47cc-85e5-0e46c1846f15"), "1" }
                });

            migrationBuilder.InsertData(
                table: "EducationPrograms",
                columns: new[] { "Id", "Cost", "EducationFormId", "FEAProgramId", "FinancingTypeId", "HoursCount", "IsArchive", "IsCollegeProgram", "IsDOTProgram", "IsFullDOTProgram", "IsModularProgram", "IsNetworkProgram", "KindDocumentRiseQualificationId", "Name" },
                values: new object[] { new Guid("b741f950-19b2-472c-bf66-e84bec7c0bb5"), 0.0, new Guid("0241c1ac-bb5b-4ca1-bb46-89ba1e0c4287"), null, new Guid("0457cc26-6b4f-472b-bdbf-a9be3599e931"), 250, false, false, false, false, false, false, new Guid("aa7a8325-4b0d-4dd2-bedc-2c4a065ab332"), "Академия цифра" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "BirthDate", "Email", "Family", "IT_Experience", "Name", "Nationality", "Patron", "Phone", "ScopeOfActivityLevelOneId", "ScopeOfActivityLevelTwoId", "Sex", "TypeEducationId" },
                values: new object[] { new Guid("c337e8c4-142a-4f01-a54f-fea1be3d874b"), "Проспект Сишарпа, 42", new DateOnly(2003, 3, 3), "iii@gmail.com", "Иванов", "Есть", "Иван", "Россия", "Иванович", "+7 (123) 453-78-90", new Guid("e768a213-0421-4c6f-85b8-0069882870c6"), null, 1, new Guid("a4f6d736-28e7-4a63-845e-24e62b433fc1") });

            migrationBuilder.InsertData(
                table: "ScopesOfActivity",
                columns: new[] { "Id", "Level", "NameOfScope", "ScopeOfActivityParentId" },
                values: new object[,]
                {
                    { new Guid("197a368e-b704-44ed-81fa-c494537a0813"), 2, "Педагогические работники профессиональных образовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("3af3138f-d487-4866-b8c3-b6e782bf1de8"), 2, "Руководители общеобразовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("5b12db36-3fe2-4831-be2f-075c0bc08d63"), 2, "Педагогические работники образовательных организаций ВО", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("71e13148-a111-4c45-94b2-a920353d0571"), 2, "Руководители дошкольных образовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("77fdb430-5b2f-4c64-8a1a-6728be169e1f"), 2, "Педагогические работники дошкольных образовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("9b70f630-83bf-4805-b9c9-e0a96c0a39b2"), 2, "Безработные", new Guid("5f450e00-d584-4736-882b-1b6ada2484bc") },
                    { new Guid("ab739f86-1efe-4a5a-883c-ea6963d49e5e"), 2, "Педагогические работники организаций дополнительного образования", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("ac7b229f-97a0-4325-9e32-c3334c2c8939"), 2, "Педагогические работники общеобразовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("b9115d04-3a42-42e3-8a74-61519f13a4e8"), 2, "Руководители образовательных организаций ВО", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("ca1c76a3-d268-4d06-8e4a-3721a4f2fb54"), 2, "Руководители предприятий и организаций", new Guid("e768a213-0421-4c6f-85b8-0069882870c6") },
                    { new Guid("d58399ce-b5f3-4d0d-8cb9-79419fc373dd"), 2, "Педагогические работники организаций ДПО", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("d8b097c4-a4c3-447f-9ead-112312c25530"), 2, "Руководители организаций ДПО", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("e060399a-c130-4ea9-901c-28f62cb7c532"), 2, "Руководители организаций дополнительного образования", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("e94fff46-4a09-4c26-ad86-75219d0bc489"), 2, "Руководители профессиональных образовательных организаций", new Guid("e1ab6e96-d9af-41c1-abca-61a1e5052a9e") },
                    { new Guid("fbd51db0-a7c0-4da0-9963-f5e668a13058"), 2, "Руководители гос.служащие", new Guid("ed3df49f-e714-4940-a151-458616ee7d84") }
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Id", "EducationProgramId", "EndDate", "Name", "StartDate" },
                values: new object[] { new Guid("9a8cd57f-4afe-488b-ab0c-1a25519a2fd7"), new Guid("b741f950-19b2-472c-bf66-e84bec7c0bb5"), new DateOnly(2025, 6, 1), "С42-019-10", new DateOnly(2024, 9, 1) });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Address", "BirthDate", "Email", "Family", "IT_Experience", "Name", "Nationality", "Patron", "Phone", "ScopeOfActivityLevelOneId", "ScopeOfActivityLevelTwoId", "Sex", "TypeEducationId" },
                values: new object[] { new Guid("ce523bbd-dbd2-4bc6-8986-0f0c83926c57"), "Проспект PHP, 47", new DateOnly(2004, 4, 4), "iai@gmail.com", "Иванова", "Есть", "Анна", "Россия", "Ивановна", "+7 (123) 451-71-90", new Guid("9b70f630-83bf-4805-b9c9-e0a96c0a39b2"), null, 0, new Guid("f87eaad5-5d84-45ce-b862-8da5c45ead5b") });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateTakeDiplom", "Disability", "DocumentNumber", "DocumentSeries", "FullNameDocument", "LastChangedByUserId", "PersonId", "Projects", "SNILS", "Speciality" },
                values: new object[] { new Guid("c337e8c4-142a-4f01-a54f-fea1be3d874b"), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, null, "Иванов", null, new Guid("c337e8c4-142a-4f01-a54f-fea1be3d874b"), "Немало", null, "Сварщик" });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "Agreement", "DataNumberDogovor", "DocumentRiseQualificationId", "EducationProgramId", "Email", "IsAlreadyStudied", "PersonId", "Phone", "RegistrationNumber", "StatusEntrancExams", "StatusRequestId", "StudentId", "StudentStatusId" },
                values: new object[] { new Guid("4178e3fa-dca8-4e28-a815-46cfacb61fe5"), true, "2024-09-01, 9876", new Guid("b28d1f29-0aa9-4209-8bab-cd49a8ad548d"), new Guid("b741f950-19b2-472c-bf66-e84bec7c0bb5"), "iii@gmail.com", null, null, "+7 (123) 456-78-90", "432", 3, new Guid("d2b3c504-1890-43f4-a351-22eea9b8dc08"), new Guid("c337e8c4-142a-4f01-a54f-fea1be3d874b"), new Guid("69b4aaff-b67f-483d-98e3-98d39da93d7a") });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateTakeDiplom", "Disability", "DocumentNumber", "DocumentSeries", "FullNameDocument", "LastChangedByUserId", "PersonId", "Projects", "SNILS", "Speciality" },
                values: new object[] { new Guid("ce523bbd-dbd2-4bc6-8986-0f0c83926c57"), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, null, null, "Иванова", null, new Guid("ce523bbd-dbd2-4bc6-8986-0f0c83926c57"), "Мало", null, "HR" });

            migrationBuilder.InsertData(
                table: "GroupStudent",
                columns: new[] { "RequestId", "GroupId", "StudentId" },
                values: new object[] { new Guid("4178e3fa-dca8-4e28-a815-46cfacb61fe5"), new Guid("9a8cd57f-4afe-488b-ab0c-1a25519a2fd7"), new Guid("c337e8c4-142a-4f01-a54f-fea1be3d874b") });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Date", "KindOrderId", "Number", "RequestId" },
                values: new object[] { new Guid("0f6779b4-4e09-4f91-b7df-881205ea39d0"), new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new Guid("753df8b7-2d6f-4499-9f86-563771f016c1"), "42", new Guid("4178e3fa-dca8-4e28-a815-46cfacb61fe5") });

            migrationBuilder.InsertData(
                table: "Requests",
                columns: new[] { "Id", "Agreement", "DataNumberDogovor", "DocumentRiseQualificationId", "EducationProgramId", "Email", "IsAlreadyStudied", "PersonId", "Phone", "RegistrationNumber", "StatusEntrancExams", "StatusRequestId", "StudentId", "StudentStatusId" },
                values: new object[] { new Guid("7ecc61ae-2472-484e-8078-3b34f3448b8e"), true, "2024-09-02, 9877", new Guid("5dde5f20-a247-45b5-b989-800a858d0b70"), new Guid("b741f950-19b2-472c-bf66-e84bec7c0bb5"), "iai@gmail.com", null, null, "+7 (123) 451-71-90", "432", 3, new Guid("d2b3c504-1890-43f4-a351-22eea9b8dc08"), new Guid("ce523bbd-dbd2-4bc6-8986-0f0c83926c57"), new Guid("69b4aaff-b67f-483d-98e3-98d39da93d7a") });

            migrationBuilder.InsertData(
                table: "GroupStudent",
                columns: new[] { "RequestId", "GroupId", "StudentId" },
                values: new object[] { new Guid("7ecc61ae-2472-484e-8078-3b34f3448b8e"), new Guid("9a8cd57f-4afe-488b-ab0c-1a25519a2fd7"), new Guid("ce523bbd-dbd2-4bc6-8986-0f0c83926c57") });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentRiseQualifications_KindDocumentRiseQualificationId",
                table: "DocumentRiseQualifications",
                column: "KindDocumentRiseQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_EducationFormId",
                table: "EducationPrograms",
                column: "EducationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_FEAProgramId",
                table: "EducationPrograms",
                column: "FEAProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_FinancingTypeId",
                table: "EducationPrograms",
                column: "FinancingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EducationPrograms_KindDocumentRiseQualificationId",
                table: "EducationPrograms",
                column: "KindDocumentRiseQualificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_EducationProgramId",
                table: "Groups",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudent_GroupId_StudentId",
                table: "GroupStudent",
                columns: new[] { "GroupId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupStudent_StudentId",
                table: "GroupStudent",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_KindOrderId",
                table: "Orders",
                column: "KindOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequestId",
                table: "Orders",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Email",
                table: "Persons",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_Phone",
                table: "Persons",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ScopeOfActivityLevelOneId",
                table: "Persons",
                column: "ScopeOfActivityLevelOneId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ScopeOfActivityLevelTwoId",
                table: "Persons",
                column: "ScopeOfActivityLevelTwoId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_TypeEducationId",
                table: "Persons",
                column: "TypeEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_DocumentRiseQualificationId",
                table: "Requests",
                column: "DocumentRiseQualificationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requests_EducationProgramId",
                table: "Requests",
                column: "EducationProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_PersonId",
                table: "Requests",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StatusRequestId",
                table: "Requests",
                column: "StatusRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StudentId",
                table: "Requests",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_StudentStatusId",
                table: "Requests",
                column: "StudentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScopesOfActivity_ScopeOfActivityParentId",
                table: "ScopesOfActivity",
                column: "ScopeOfActivityParentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentHistory_StudentId",
                table: "StudentHistory",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_PersonId",
                table: "Students",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_SNILS",
                table: "Students",
                column: "SNILS",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupStudent");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "StudentHistory");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "KindOrders");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "DocumentRiseQualifications");

            migrationBuilder.DropTable(
                name: "EducationPrograms");

            migrationBuilder.DropTable(
                name: "StatusRequests");

            migrationBuilder.DropTable(
                name: "StudentStatuses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "EducationForms");

            migrationBuilder.DropTable(
                name: "FEAPrograms");

            migrationBuilder.DropTable(
                name: "FinancingTypes");

            migrationBuilder.DropTable(
                name: "KindDocumentRiseQualifications");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "ScopesOfActivity");

            migrationBuilder.DropTable(
                name: "TypeEducation");
        }
    }
}

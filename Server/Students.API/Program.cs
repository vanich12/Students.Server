using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Students.DBCore.Contexts;
using Students.Models;
using ClosedXML.Excel;
using Studens.Application.Report;
using Studens.Application.Report.Interfaces;
using Students.API.EndpointsFilters;
using Students.API.Middlewares;
using Students.Application.Services;
using Students.Application.Services.Interfaces;
using Students.Infrastructure.Extension;
using Students.Infrastructure.Interfaces;
using Students.Infrastructure.Storages;
using Students.Infrastructure.Storages.Reports;
using Students.Models.ReferenceModels;
using Students.Models.ReportsModel;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// builder.Services.AddLogging(loggingBuilder =>
// {
//     loggingBuilder.AddSeq(configuration.GetSection("Seq"));
// });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<StudentContext, PgContext>();
// builder.Services.AddDbContext<StudentContext, InMemoryContext>();
//builder.Services.AddScoped<InMemoryContext>();
//builder.Services.AddScoped<StudentContext>();
builder.Services.AddScoped<PgContext>();
builder.Services.AddSingleton<ExceptionHandlingMiddleware>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupStudentRepository, GroupStudentRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IPendingRequestService, PendingRequestService>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IPendingRequestRepository, PendingRequestRepository>();
builder.Services.AddScoped<IReportRepository<FRDOModel>, FRDOReportRepository>();
builder.Services.AddScoped<IReportRepository<RosstatModel>, RosstatReportRepository>();
builder.Services.AddScoped<IReport<XLWorkbook>, GenerateReports>();
builder.Services.AddScoped<IEducationProgramRepository, EducationProgramRepository>();
//builder.Services.AddScoped<IReportRepository, CSVReportRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IFEAProgramRepository, FEAProgramRepository>();
builder.Services.AddScoped<IFinancingTypeRepository, FinancingTypeRepository>();
builder.Services.AddSwaggerGen(options =>
{
    var basePath = AppContext.BaseDirectory;

    //var apiDoc = Path.Combine(basePath, "Students.APIServer.xml");
    //var modelsDoc = Path.Combine(basePath, "Students.Models.xml");
    //options.IncludeXmlComments(apiDoc);
    //options.IncludeXmlComments(modelsDoc);
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<EducationForm>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<EducationProgram>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<FEAProgram>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<StatusRequest>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<FinancingType>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<Group>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<Request>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<ScopeOfActivity>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<Students.Models.Student>>();
    //options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<StudentDocument>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<TypeEducation>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<StudentStatus>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<KindDocumentRiseQualification>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<DocumentRiseQualification>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<KindOrder>>();
    options.SchemaFilter<Swagger.ExcludeIdPropertyFilter<Order>>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .SetIsOriginAllowed(e => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});


builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});

builder.Services.AddScoped<LogModelStateActionFilter>();
builder.Services.AddControllers(options =>
        options.Filters.Add<LogModelStateActionFilter>())
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

await using var scope = app.Services.CreateAsyncScope();
await using var db = scope.ServiceProvider.GetService<PgContext>();
//if (db != null)
//{
//    await db.Database.MigrateAsync();
//}

app.Run();
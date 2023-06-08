using CLTTechnicalAssessmentApi;
using CLTTechnicalAssessmentApi.Data;
using CLTTechnicalAssessmentApi.Repository;
using CLTTechnicalAssessmentApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseInMemoryDatabase("UsersDB");
});
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers(opt =>
{
    opt.ReturnHttpNotAcceptable = true;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.0",
        Title = "CLT Technical Assessment v1",
        Contact = new OpenApiContact
        {
            Name = "Hugo Rodriguez",
            Url = new Uri("https://github.com/hugo098")
        },
    });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddMvc().ConfigureApiBehaviorOptions(opt =>
{
    opt.InvalidModelStateResponseFactory = context =>
    {
        var problems = new CustomBadRequest(context);
        return new BadRequestObjectResult(problems);
    };
    opt.SuppressMapClientErrors = true;
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
    options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CLTTechnicalAssessmentV1");
    }
);


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

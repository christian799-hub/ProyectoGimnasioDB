using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Gimnasio.Core.Interfaces;
using Gimnasio.Infrastructure.Data;
using Gimnasio.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Gimnasio.Core.Profiles;
using Gimnasio.Infrastructure.Filters;
using Gimnasio.Infrastructure.Validators;
using FluentValidation;
using Gimnasio.Core.Services;
using Gimnasio.Core.Repositories;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using Gimnasio.Infrastructure.Data.TypeHandlers;

var builder = WebApplication.CreateBuilder(args);
 
 //Configurar secretos de usuario
 if(builder.Environment.IsDevelopment())
 {
     builder.Configuration.AddUserSecrets<Program>();
 }
 //En produccion los secretos vendran de Entornos Globales 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GimnasioContext>(options =>
    options.UseMySql(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
));

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IUsuariosRepository,UsuariosRepository>();
// builder.Services.AddTransient<IMembresiasRepository,MembresiaRepository>();
builder.Services.AddTransient<IUsuarioMembresiasRepository,UsuarioMembresiasRepository>();
builder.Services.AddTransient<IClasesRepository,ClasesRepository>();
builder.Services.AddTransient<IAsistenciaRepository,AsistenciaRepository>();
builder.Services.AddTransient<IHorariosRepository,HorariosRepository>();

//Validators
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<AsistenciumDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<ClaseDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<MembresiaDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<HorarioDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioMembresiaDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<GetByIdRequestValidator>();

builder.Services.AddScoped<IValidationService,ValidationService>();

builder.Services.AddScoped<ValidationFilter>();

//Service
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<IUsuarioMembresiaService, UsuarioMembresiaService>();
builder.Services.AddScoped<IHorarioService, HorarioService>();
builder.Services.AddScoped<IClasesService, ClasesService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMembresiaService, MembresiaService>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>)); // Repositorio generico

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // Inyectar UnitOfWork

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext,DapperContext>();

builder.Services.AddControllers(options => 
    options.Filters.Add<ValidationFilter>()
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});


 builder.Services.AddControllers(
    options => 
    {
        options.Filters.Add<GlobalExceptionFilter>();
    }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }).ConfigureApiBehaviorOptions(options => 
           {
            options.SuppressModelStateInvalidFilter = true;
            });
builder.Services.AddSwaggerGen( options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Gimnasio API",
        Version = "v1",
        Description = "API para la gestión de un gimnasio",
        Contact = new(){
            Name = "Equipo de desarrollo yo solo",
            Email = "christian.ferrufino.m@ucb.edu.bo"
        }
    });
    var xmlFile =  $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
});

builder.Services.AddApiVersioning(options =>
{
    // Reporta las versiones soportadas y obsoletas en encabezados de respuesta
    options.ReportApiVersions = true;

    // Versión por defecto si no se especifica
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Soporta versionado mediante URL, Header o QueryString
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),       // Ejemplo: /api/v1/...
        new HeaderApiVersionReader("x-api-version"), // Ejemplo: Header → x-api-version: 1.0
        new QueryStringApiVersionReader("api-version") // Ejemplo: ?api-version=1.0
    );
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer =true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"])
        )
    };
});

//Typehandlers para casteo en la base de datos para el dapper

SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
SqlMapper.AddTypeHandler(new TimeOnlyTypeHandler());


var app = builder.Build();

//Usar swagger

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Gimnasio API v1");
        options.RoutePrefix = string.Empty;
    });
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
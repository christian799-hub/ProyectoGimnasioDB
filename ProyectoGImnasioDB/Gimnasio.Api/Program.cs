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

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddTransient<IMembresiasRepository,MembresiaRepository>();
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


 builder.Services.AddControllers().AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }).ConfigureApiBehaviorOptions(options => 
           {
            options.SuppressModelStateInvalidFilter = true;
            });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
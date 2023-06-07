using FactoryMaintenanceAPI.Data;
using FactoryMaintenanceAPI.ModelsDtoMapper;
using FactoryMaintenanceAPI.Repository;
using FactoryMaintenanceAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddXmlDataContractSerializerFormatters(); //To return XML

//SQL Server conexion setup

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexionSql"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


//Add Cache Services
builder.Services.AddResponseCaching();

//Add Repositories
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IFactoryRepository, FactoryRepository>();
builder.Services.AddScoped<IMachineRepository, MachineRepository>();
builder.Services.AddScoped<IMaintenanceChoreRepository, MaintenanceChoreRepository>();


builder.Services.AddHttpClient();

//Add autoMapper
builder.Services.AddAutoMapper(typeof(ModelsDtoMapper));


// Add services to the container.

builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("DefaultCacheProfile", new CacheProfile() { Duration = 60 });//Add Global Cache Profile one day 86400
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Add CORS support
builder.Services.AddCors(p => p.AddPolicy("PolicyCors", build => {
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();//Giving access to all domains for now
}));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

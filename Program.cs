using Commander.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<ICommanderRepo, MockCommanderRepo>();
builder.Services.AddScoped<ICommanderRepo, SQLCommanderRepo>();

builder.Services.AddDbContext<CommanderContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("CommanderConnection")));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using BenchmarkDotNet.Running;
using ConectaAtende.Application.Services;
using ConectaAtende.Application.Undo;
using ConectaAtende.Benchmarks;
using ConectaAtende.Domain.Policies;
using ConectaAtende.Domain.Repositories;
using ConectaAtende.Domain.Services;
using ConectaAtende.Infrastructure.Repositories;
using ConectaAtende.Infrastructure.Triage;
using Projeto.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ITicketRepository, InMemoryTicketRepository>();
builder.Services.AddSingleton<ITicketTriagePolicy, DefaultTicketTriagePolicy>();
builder.Services.AddSingleton<IContactRepository, InMemoryContactRepository>();
builder.Services.AddSingleton<UndoService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddSingleton<ITriagePolicy, FifoPolicy>();
builder.Services.AddSingleton<ITriagePolicy, PriorityPolicy>();
builder.Services.AddSingleton<ITriagePolicy, MixedPolicy>();
builder.Services.AddSingleton<TriageService>();

var app = builder.Build();
BenchmarkRunner.Run<ContactBenchmarks>();

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
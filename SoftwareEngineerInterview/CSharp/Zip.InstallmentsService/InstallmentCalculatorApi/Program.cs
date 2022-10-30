using Microsoft.AspNetCore.Mvc;
using Zip.InstallmentsService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApiVersioning(api =>
{
    api.DefaultApiVersion = new ApiVersion(1, 0);
    api.AssumeDefaultVersionWhenUnspecified = true;
    api.ReportApiVersions = true;
});

builder.Services.AddScoped<PaymentPlanFactory>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

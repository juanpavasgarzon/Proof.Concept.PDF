using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Channels;
using File.Api.Extensions;
using File.Api.Models;
using File.Api.Services;
using QuestPDF.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddOpenApi();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ConcurrentDictionary<string, Pdf>>();
builder.Services.AddSingleton<PublishPdfGenerationService>();
builder.Services.AddHostedService<PdfGenerationService>();
builder.Services.AddSingleton(_ =>
{
    var channel = Channel.CreateBounded<PdfGenerationJob>(new BoundedChannelOptions(100)
    {
        FullMode = BoundedChannelFullMode.Wait
    });

    return channel;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI V1"); });

    app.UseReDoc(options => { options.SpecUrl("/openapi/v1.json"); });

    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();
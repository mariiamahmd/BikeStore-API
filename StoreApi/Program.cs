using Microsoft.AspNetCore.Components;
using StoreApi.Data;
using Scalar.AspNetCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<BikestoreContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<BikestoreContext>();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Bike Store Management API";
        document.Info.Version = "v1";
        document.Info.Description = "RESTful API for managing a bike store.";
        return Task.CompletedTask;
    });
});
builder.Services.AddAutoMapper(typeof(Program));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();
// app.MapIdentityApi<IdentityUser>();
app.MapControllers();

app.Run();
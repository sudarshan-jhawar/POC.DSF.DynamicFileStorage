using POC.DSF.FileStorage.Service;
using POC.DSF.FileStorage.Service.Abstractions;
using POC.DSF.FileStorage.Service.Services;
using POC.DSF.FileStorage.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// Add services to the container.
builder.Services.AddScoped<AzureBlobStorageService>();
builder.Services.AddScoped<AWSS3StorageService>();
builder.Services.AddTransient<IFileStorageService>(sp =>
{
    var config = builder.Configuration.GetValue<EnvironmentProviders>("AppSettings:Provider");
    return config switch
    {
        EnvironmentProviders.Azure => sp.GetService<AzureBlobStorageService>(),
        EnvironmentProviders.Aws => sp.GetService<AWSS3StorageService>(),
        _ => sp.GetService<AzureBlobStorageService>(),
    };
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

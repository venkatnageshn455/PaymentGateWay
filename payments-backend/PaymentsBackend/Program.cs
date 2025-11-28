using Microsoft.EntityFrameworkCore;
using PaymentsBackend.DataModels;
using PaymentsBackend.DataStore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with retry on failure
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null);
        }));

builder.Services.AddScoped<IPaymentStore, PaymentStore>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IDailySequenceRepository, DailySequenceRepository>();
builder.Services.AddScoped<ILoggingService, LoggingService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS for Angular dev
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Configure Kestrel to always use HTTPS with dev certificate
builder.WebHost.ConfigureKestrel(options =>
{
    options.ConfigureHttpsDefaults(httpsOptions =>
    {
        // null will use the dev certificate (trusted after dotnet dev-certs https --trust)
        httpsOptions.ServerCertificate = null;
    });
});

var app = builder.Build();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enforce HTTPS
app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AngularPolicy");

app.MapControllers();

app.Run();

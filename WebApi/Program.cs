var builder = WebApplication.CreateBuilder(args);

// Enable CORS for development environment
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowAllPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Kestrel to listen on a specific port with HTTPS
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ListenAnyIP(7126, listenOptions =>
    {
        listenOptions.UseHttps(); // Use default development certificate
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Apply CORS middleware
app.UseCors("_myAllowAllPolicy");

app.MapControllers();

app.Run();

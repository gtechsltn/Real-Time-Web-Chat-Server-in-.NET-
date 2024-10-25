using RealTimeWebChat.Api.Contacts;
using RealTimeWebChat.Api.Data;
using RealTimeWebChat.Api.Hubs;
using RealTimeWebChat.Api.Repositorys;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddSignalR();
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("https://localhost:5000") // Add the client URL here
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials for SignalR WebSocket connections
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS before routing
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();
app.UseRouting();

app.MapControllers();

// Configure SignalR Hub route
app.MapHub<ChatHub>("/chatHub");

app.Run();

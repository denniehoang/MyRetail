var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddTransient<IConfiguration>(x => builder.Configuration);
builder.Services.AddTransient<IProductManager, ProductManager>();
builder.Services.AddTransient<ITargetDataAccess, TargetDataAccess>();
builder.Services.AddTransient<IProductDB, ProductDbAccess>();
builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpClient("Target", x => x.BaseAddress = new Uri($"{builder.Configuration["target:uri"]}?key={builder.Configuration["target:key"]}"));

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

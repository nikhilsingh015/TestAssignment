using Api.Libraries.AddressLibrary.Db;
using Api.Libraries.AddressLibrary.Repositories;

namespace Api;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient("DbWorkerClient", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5000");
        });


        builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
        {
            var db =
                builder.Configuration.GetValue<string>("DB_FILE")
                ?? throw new ArgumentException("DB_FILE not set");

            if (!File.Exists(db))
            {
                throw new FileNotFoundException($"Database file not found: {db}");
            }

            return new SqliteConnectionFactory($"Data Source={db}");
        });

        builder.Services.AddScoped<IAddressRepository, AddressRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();

        app.Run();
    }
}

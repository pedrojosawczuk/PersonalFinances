using PersonalFinances.DataContext;
using PersonalFinances.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EFDataContext>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(builder =>
		{
			builder.WithOrigins("http://127.0.0.1:5500")
				.AllowAnyHeader()
				.AllowAnyMethod()
				.AllowCredentials();
		});
});

var app = builder.Build();

app.UseCors();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
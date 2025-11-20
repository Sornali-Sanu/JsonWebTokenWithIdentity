using JsonWebTokenwithIdentity.DBIbitializer;
using JsonWebTokenwithIdentity.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.ApplicationService(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Create scope for InitializerDB
using (var scope = app.Services.CreateScope())
{
    var services= scope.ServiceProvider;
	try
	{
		var dbInitializer = services.GetRequiredService<IDbInitializer>();
		dbInitializer.Initialize();

	}
	catch (Exception ex) 
	{

		var logger=services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "an Error Occured in DB");
	}
}


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

app.UseHttpsRedirection();

app.UseCors(
	op => op.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("https://localhost:7070"));

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();

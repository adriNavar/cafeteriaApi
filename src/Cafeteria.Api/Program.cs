var builder = WebApplication.CreateBuilder(args); // Crea la instancia del proyecto



builder.Services.AddControllers();// Indica que va a trabajar con controladores, que estan en la carpeta Controllers, y que van a manejar las peticiones HTTP

// builder.Services.AddOpenApi(); //Agrega al proyecto documentacion en Json

var app = builder.Build(); // Inyecya servicios(por ejemplo autenticacion o mapeo de controles)


// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// } // esto habla de la documentacion, que solo se va a mostrar en desarrollo, no en produccion

// app.UseHttpsRedirection(); // esto es para https 

// app.UseAuthorization();

// app.MapControllers();

app.Run();

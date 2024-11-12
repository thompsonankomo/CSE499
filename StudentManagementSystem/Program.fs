open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.IdentityModel.Tokens
open System.Text

let (builder: obj) = WebApplication.CreateBuilder(args)

builder.Services.AddControllers()

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(fun options ->
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    |> ignore)

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(fun options ->
        options.TokenValidationParameters <- TokenValidationParameters(
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"))
        ))

let app = builder.Build()

if app.Environment.IsDevelopment() then
    app.UseDeveloperExceptionPage()

app.UseHttpsRedirection()
app.UseRouting()

// Add Authentication and Authorization middleware
app.UseAuthentication()
app.UseAuthorization()

app.MapControllers()
app.Run()

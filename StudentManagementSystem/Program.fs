namespace StudentManagementSystem

open System
open System.Text
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Authentication.JwtBearer
open Microsoft.IdentityModel.Tokens
open Microsoft.EntityFrameworkCore
open StudentManagementSystem.Data
open StudentManagementSystem.Models

module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        // Configure services
        builder.Services.AddDbContext<ApplicationDbContext>(fun (options: DbContextOptionsBuilder) ->
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")) |> ignore
        )

        builder.Services.AddControllers()
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(fun options ->
                options.TokenValidationParameters <- TokenValidationParameters(
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKeyHere"))
                )
            ) |> ignore

        let app = builder.Build()
        app.UseHttpsRedirection()
        app.UseAuthentication()
        app.UseAuthorization()
        app.MapControllers()
        app.Run()
        0

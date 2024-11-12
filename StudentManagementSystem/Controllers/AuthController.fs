namespace StudentManagementSystem.Controllers

open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open System.Text
open Microsoft.AspNetCore.Mvc
open Microsoft.IdentityModel.Tokens
open StudentManagementSystem.Data
open StudentManagementSystem.Models
open Microsoft.AspNetCore.Cryptography.KeyDerivation
open Microsoft.Extensions.Configuration
open Microsoft.AspNetCore.Authorization

[<Authorize(Roles = "Admin")>]
[<Route("api/[controller]")>]
[<ApiController>]
type StudentController() =
    // Secure student management endpoints for Admin role only


[<Route("api/[controller]")>]
[<ApiController>]
type AuthController (context: ApplicationDbContext, configuration: IConfiguration) =
    inherit ControllerBase()

    let hashPassword (password: string) =
        Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password, Encoding.UTF8.GetBytes("salt"), KeyDerivationPrf.HMACSHA256, 10000, 32))

    let generateJwtToken (user: User) =
        let claims = [|
            Claim(JwtRegisteredClaimNames.Sub, user.Username)
            Claim("role", user.Role)
        |]
        let key = SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        let creds = SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        let token = JwtSecurityToken(claims = claims, signingCredentials = creds, expires = DateTime.UtcNow.AddDays(1.0))
        JwtSecurityTokenHandler().WriteToken(token)

    [<HttpPost("register")>]
    member this.Register([<FromBody>] user: User) =
        let userExists = context.Users.Any(fun u -> u.Username = user.Username)
        if userExists then
            this.BadRequest("User already exists") :> IActionResult
        else
            let hashedPassword = hashPassword user.PasswordHash
            let newUser = { user with PasswordHash = hashedPassword }
            context.Users.Add(newUser) |> ignore
            context.SaveChanges()
            this.Ok("Registration successful") :> IActionResult

    [<HttpPost("login")>]
    member this.Login([<FromBody>] login: User) =
        let user = context.Users.SingleOrDefault(fun u -> u.Username = login.Username)
        if user = null || hashPassword login.PasswordHash <> user.PasswordHash then
            this.Unauthorized("Invalid username or password") :> IActionResult
        else
            let token = generateJwtToken user
            this.Ok(new { Token = token }) :> IActionResult

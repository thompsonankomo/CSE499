open System.IdentityModel.Tokens.Jwt
open System.Security.Claims
open Microsoft.IdentityModel.Tokens
open System.Text

[<HttpPost("login")>]
member this.Login(username: string, password: string) =
    if username = "admin" && password = "password" then
        let tokenHandler = JwtSecurityTokenHandler()
        let key = Encoding.UTF8.GetBytes("YourSecretKeyHere")
        let tokenDescriptor = SecurityTokenDescriptor(
            Subject = ClaimsIdentity([Claim(ClaimTypes.Name, username)]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = SigningCredentials(SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        )
        let token = tokenHandler.CreateToken(tokenDescriptor)
        Ok(tokenHandler.WriteToken(token))
    else
        Unauthorized()

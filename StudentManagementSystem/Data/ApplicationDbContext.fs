namespace StudentManagementSystem.Models

open System.ComponentModel.DataAnnotations
namespace StudentManagementSystem.Data

open Microsoft.EntityFrameworkCore
open StudentManagementSystem.Models

type User =
    {
        [<Key>]
        Id: int
        Username: string
        PasswordHash: string
        Role: string  // e.g., "Admin" or "Student"
    }


type ApplicationDbContext(options: DbContextOptions<ApplicationDbContext>) =
    inherit DbContext(options)

    [<DefaultValue>] val mutable Users: DbSet<User>
    member this.Users with get() = this.Users and set v = this.Users <- v


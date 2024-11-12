namespace StudentManagementSystem.Data

open Microsoft.EntityFrameworkCore
open StudentManagementSystem.Models

type ApplicationDbContext(options: DbContextOptions<ApplicationDbContext>) =
    inherit DbContext(options)
    [<DefaultValue>] val mutable students : DbSet<Student>
    member this.Students with get() = this.students and set v = this.students <- v

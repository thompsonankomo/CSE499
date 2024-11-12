namespace StudentManagementSystem.Models

open System.ComponentModel.DataAnnotations

type Student() =
    [<Key>]
    member val Id: int = 0 with get, set
    member val Name: string = "" with get, set
    member val Email: string = "" with get, set
    member val EnrollmentDate: DateTime = DateTime.Now with get, set
    member val Course: string = "" with get, set

﻿using System;
using System.ComponentModel.DataAnnotations;

namespace APBD_Tutorial_11.Models
{
    public class InsertStudentRequest
    {
        [Required] public string IndexNumber { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Password { get; set; }
        [Required] public string BirthDate { get; set; }
        [Required] public string IdEnrollment { get; set; }
    }
}
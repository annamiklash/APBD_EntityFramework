#nullable enable
using System;
using System.ComponentModel.DataAnnotations;

namespace APBD_Tutorial_11.Models
{
    public class UpdateStudentRequest
    {
        [Required]
        public string IndexNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? BirthDate { get; set; }
        public string? IdEnrollment { get; set; }

        public override string ToString()
        {
            return $"{FirstName}, {LastName}, {Password}, {BirthDate}, {IdEnrollment}";
        }
    }
}
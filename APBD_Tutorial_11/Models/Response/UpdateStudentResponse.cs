#nullable enable
namespace APBD_Tutorial_11.Models.Response
{
    public class UpdateStudentResponse
    {
        public string IndexNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Password { get; set; }
        public string? BirthDate { get; set; }
        public string? IdEnrollment { get; set; }

    }
}
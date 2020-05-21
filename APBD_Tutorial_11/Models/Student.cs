using System;
using Newtonsoft.Json;

namespace APBD_Tutorial_11.Models
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }

        [JsonIgnore]
        public virtual Enrollment IdEnrollmentNavigation { get; set; }

        public Student()
        {
        }
        
        
        public override string ToString()
        {
            return $"Student : IndexNumber {IndexNumber}, FirstName {FirstName}, LastName {LastName} ";
        }
        
    }


}

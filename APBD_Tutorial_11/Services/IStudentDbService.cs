using System.Collections.Generic;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;

namespace APBD_Tutorial_11.Services
{
    public interface IStudentDbService
    {
        bool StudentExists(string index);
        IEnumerable<Student> GetAllStudents();
        Student GetStudentByIndex(string index);
        InsertStudentResponse InsertStudent(InsertStudentRequest request);
        UpdateStudentResponse UpdateStudent(UpdateStudentRequest request);
        void DeleteStudent(string index);
        bool EnrollmentExists(string enrollmentId);
        
    }
}
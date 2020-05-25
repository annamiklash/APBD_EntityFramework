using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;

namespace APBD_Tutorial_11.Services
{
    public class StudentDbService : IStudentDbService
    {
        private readonly s18458Context _context;

        public StudentDbService(s18458Context context)
        {
            _context = context;
        }


        public bool StudentExists(string index)
        {
            return _context.Student
                .Any(student => student.IndexNumber.Equals(index));
        }

        public IEnumerable<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();
            try
            {
                students = _context.Student.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return students;
        }

        public Student GetStudentByIndex(string index)
        {
            var doctorExists = StudentExists(index);

            if (doctorExists)
            {
                return _context.Student.FirstOrDefault(st => st.IndexNumber.Equals(index));
            }
            return null;
        }

        public InsertStudentResponse InsertStudent(InsertStudentRequest request)
        {
            var student = new Student()
            {
                IndexNumber = request.IndexNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                BirthDate = DateTime.ParseExact(request.BirthDate, "dd.MM.yyyy", null),
                Password = request.Password,
                IdEnrollment = Convert.ToInt32(request.IdEnrollment)
            };

            _context.Student.Add(student);
            _context.SaveChanges();

            return new InsertStudentResponse()
            {
                IndexNumber = request.IndexNumber,
                LastName = request.LastName
            };
        }

        public UpdateStudentResponse UpdateStudent(UpdateStudentRequest request)
        {
            var student = _context.Student
                .FirstOrDefault(s => s.IndexNumber == request.IndexNumber);

            var response = new UpdateStudentResponse();
            response.IndexNumber = request.IndexNumber;

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                student.FirstName = request.FirstName;
                _context.SaveChanges();
               
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                student.LastName = request.LastName;
                _context.SaveChanges();
                
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                student.Password = request.Password;
                _context.SaveChanges();
            }
            
            if (!string.IsNullOrEmpty(request.BirthDate))
            {
                student.BirthDate = Convert.ToDateTime(request.BirthDate);
                _context.SaveChanges();
            }
            
            if (!string.IsNullOrEmpty(request.IdEnrollment))
            {
                var enrollmentExists = EnrollmentExists(request.IdEnrollment);
                if (!enrollmentExists)
                {
                    student.IdEnrollment = Convert.ToInt32(request.IdEnrollment);
                    _context.SaveChanges();
                    
                }
            }
            response.FirstName = student.FirstName;
            response.LastName = student.LastName;
            response.BirthDate = student.BirthDate.ToString(CultureInfo.InvariantCulture);
            response.IdEnrollment = student.IdEnrollment.ToString();
            response.Password = student.Password;

            return response;
        }

        public void DeleteStudent(string index)
        {
            var student = _context.Student
                .FirstOrDefault(s => s.IndexNumber == index);

            _context.Student.Remove(student);
            _context.SaveChanges();
        }

        public bool EnrollmentExists(string enrollmentId)
        {
            return _context.Enrollment
                .Any(enr => enr.IdEnrollment.ToString().Equals(enrollmentId));
            
        }
    }
}
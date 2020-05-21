using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD_Tutorial_11.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Tutorial_11.Controllers
{
    [Route("/api/students_EF")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly s18458Context _context;

        public StudentController(s18458Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            List<Models.Student> students = _context.Student.ToList();
            return Ok(students);
        }

        [HttpPut("add")]
        public IActionResult InsertNewStudent(Models.InsertStudentRequest studentRequest)
        {

            List<Error> errorList = ValidationHelper.ValidateStudent(studentRequest, _context);

            if (!errorList.Count.Equals(0))
            {
                return StatusCode(400, errorList);
            }

            try
            {
                var insertedStudent = new Student()
                {
                    IndexNumber = studentRequest.IndexNumber,
                    FirstName = studentRequest.FirstName,
                    LastName = studentRequest.LastName,
                    BirthDate = DateTime.ParseExact(studentRequest.BirthDate, "dd.MM.yyyy", null),
                    Password = studentRequest.Password,
                    IdEnrollment = Convert.ToInt32(studentRequest.IdEnrollment)
                };
                
                _context.Student.Add(insertedStudent);
                _context.SaveChanges();
                return StatusCode(201, "Successfully Inserted");
            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("{index}/delete")]
        public IActionResult DeleteStudent(string index )
        {
            var student = _context.Student.FirstOrDefault(student => student.IndexNumber == index);
            if (student == null)
            {
                return StatusCode(404,"Student with index " + index + " doesnt exist");
            }

            try
            {
                _context.Student.Remove(student);
                _context.SaveChanges();
                
                return StatusCode(204);
            }
            catch (Exception exception)
            {
                return StatusCode(400, exception.Message);
            }
        }

        [HttpPut("update")]
        public IActionResult UpdateStudent(UpdateStudentRequest updateRequest)
        {
            
            List<Error> errorList = ValidationHelper.ValidateUpdateStudentRequest(updateRequest, _context);
            
            if (!errorList.Count.Equals(0))
            {
                return StatusCode(400, errorList);
            }

            var student = _context.Student.FirstOrDefault(student => student.IndexNumber == updateRequest.IndexNumber);
            if (student == null)
            {
                return StatusCode(404,"Student with index " + updateRequest.IndexNumber + " doesnt exist");
            }

            try
            {
                if (!string.IsNullOrEmpty(updateRequest.FirstName))
                {
                    student.FirstName = updateRequest.FirstName;
                }

                if (!string.IsNullOrEmpty(updateRequest.LastName))
                {
                    student.LastName = updateRequest.LastName;
                }

                if (!string.IsNullOrEmpty(updateRequest.Password))
                {
                    student.Password = updateRequest.Password;
                }

                if (!string.IsNullOrEmpty(updateRequest.BirthDate))
                {
                    student.BirthDate = DateTime.ParseExact(updateRequest.BirthDate, "dd.MM.yyyy", null);
                }

                if (!string.IsNullOrEmpty(updateRequest.IdEnrollment))
                {
                    student.IdEnrollment = Convert.ToInt32(updateRequest.IdEnrollment);
                }

                _context.SaveChanges();
                return StatusCode(204);
            }
            catch (Exception exception)
            {
                return StatusCode(400, exception.Message);
            }
        }
    }
}
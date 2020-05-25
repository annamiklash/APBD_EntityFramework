using System;
using System.Collections.Generic;
using System.Linq;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_Tutorial_11.Controllers
{
    [Route("/api/students_EF")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentDbService _studentDbService;

        public StudentController(IStudentDbService service)
        {
            _studentDbService = service;
        }

        [HttpGet]
        public IActionResult GetAllStudents()
        {
            var students = _studentDbService.GetAllStudents();
            return !students.Any() ? StatusCode(404,"No Students Found") : StatusCode(200, students);
        }

        [HttpPut("add")]
        public IActionResult InsertNewStudent(Models.InsertStudentRequest studentRequest)
        {

            List<Error> errorList = ValidationHelper.ValidateStudent(studentRequest);

            if (!errorList.Count.Equals(0))
            {
                return StatusCode(400, errorList);
            }

            bool exists = _studentDbService.StudentExists(studentRequest.IndexNumber);
            if (exists)
            {
                return StatusCode(400, "Student with index " + studentRequest.IndexNumber + " already exists");
            }

            var response = _studentDbService.InsertStudent(studentRequest);
            return (StatusCode(201, response));
        }

        [HttpDelete]
        [Route("{index}/delete")]
        public IActionResult DeleteStudent(string index)
        {
            bool exists = _studentDbService.StudentExists(index);
            if (exists)
            {
                _studentDbService.DeleteStudent(index);
                return StatusCode(204);
            }
            return StatusCode(404, "No student with index " + index);
        }

        [HttpPut("update")]
        public IActionResult UpdateStudent(UpdateStudentRequest updateRequest)
        {
            
            List<Error> errorList = ValidationHelper.ValidateUpdateStudentRequest(updateRequest);
            
            if (!errorList.Count.Equals(0))
            {
                return StatusCode(400, errorList);
            }

            var studentExists = _studentDbService.StudentExists(updateRequest.IndexNumber);
            if (!studentExists) return StatusCode(404, "No student with index " + updateRequest.IndexNumber);


            var response = _studentDbService.UpdateStudent(updateRequest);
            return (StatusCode(201, response));

        }
    }
}
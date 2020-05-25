using System;
using System.Collections.Generic;
using System.Linq;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;
using APBD_Tutorial_11.Services;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable All

namespace APBD_Tutorial_11.Controllers
{
    [Route("/api/enrollments_EF")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IStudentDbService _studentDbService;
        private readonly IEnrollmentDbService _enrollmentDbService;

        public EnrollmentController(IStudentDbService studentDbService, IEnrollmentDbService enrollmentDbService)
        {
            _studentDbService = studentDbService;
            _enrollmentDbService = enrollmentDbService;
        }


        [HttpPost]
        public IActionResult EnrollNewStudent(EnrollmentRequest enrollmentRequest)
        {
            List<Error> errorsList = ValidationHelper.ValidateEnrollmentRequest(enrollmentRequest);
            if (!errorsList.Count.Equals(0))
            {
                return StatusCode(400, errorsList);
            }

            var studentExists = _studentDbService.StudentExists(enrollmentRequest.IndexNumber);
            if (studentExists)
            {
                return BadRequest("Student with index " + enrollmentRequest.IndexNumber + " already exists");
            }

            var studyExists = _enrollmentDbService.StudyExists(enrollmentRequest.Studies);
            if (!studyExists)
            {
                return NotFound("Studies " + enrollmentRequest.Studies + " does not exist");
            }

            var enrollmentExists = _enrollmentDbService.EnrollmentExists(1, enrollmentRequest.Studies);
            if (!enrollmentExists)
            {
                return NotFound("Enrollment for semester 1 for " + enrollmentRequest.Studies + " does not exist");
            }
            
            var response = _enrollmentDbService.EnrollStudent(enrollmentRequest);
            return StatusCode(201, response);
        }

        [HttpPost("promotion")]
        public IActionResult PromoteStudent(PromotionRequest promotionRequest)
        {
            var errors = ValidationHelper.ValidatePromotionRequest(promotionRequest);
            if (!errors.Count.Equals(0))
            {
                return StatusCode(400, errors);
            }
            
            bool studyExists = _enrollmentDbService.StudyExists(promotionRequest.Studies);
            if (!studyExists)
            {
                return NotFound("Studies " + promotionRequest.Studies + " does not exist");
            }

            var enrollmentExists = _enrollmentDbService.EnrollmentExists(promotionRequest.Semester, promotionRequest.Studies);
            if (!enrollmentExists)
            {
                return NotFound("Enrollment for semester " + promotionRequest.Semester + " for " + promotionRequest.Studies + " does not exist");
            }

            var studentForPromotion =_enrollmentDbService.GetStudentOnEnrollment(promotionRequest.Semester, promotionRequest.Studies);
            if (!studentForPromotion.Any())
            {
                return StatusCode(404, "No students on semester " + promotionRequest.Semester + " and with studies " + promotionRequest.Studies);
            }
            
            var responses = _enrollmentDbService.PromoteStudents(promotionRequest, studentForPromotion);
            return StatusCode(201, responses);
            }
           
        }
    }


        
    
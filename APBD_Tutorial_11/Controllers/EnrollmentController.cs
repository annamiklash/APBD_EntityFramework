using System;
using System.Collections.Generic;
using System.Linq;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable All

namespace APBD_Tutorial_11.Controllers
{
    [Route("/api/enrollments_EF")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly Models.s18458Context _context;

        public EnrollmentController(Models.s18458Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult EnrollNewStudent(EnrollmentRequest enrollmentRequest)
        {

            List<Error> errorsList = ValidationHelper.ValidateEnrollmentRequest(enrollmentRequest, _context);
            if (!errorsList.Count.Equals(0))
            {
                return StatusCode(400, errorsList);
            }

            try
            {
                var studyId = _context.Studies
                    .FirstOrDefault(studies => studies.Name == enrollmentRequest.Studies)
                    .IdStudy;

                var idEnrollment = _context.Enrollment
                    .FirstOrDefault(enrollment => enrollment.Semester == 1 && enrollment.IdStudy == studyId)
                    .IdEnrollment;

                Student student = new Student()
                {
                    IndexNumber = enrollmentRequest.IndexNumber,
                    FirstName = enrollmentRequest.FirstName,
                    LastName = enrollmentRequest.LastName,
                    BirthDate = DateTime.ParseExact(enrollmentRequest.BirthDate, "dd.MM.yyyy", null),
                    Password = enrollmentRequest.Password,
                    IdEnrollment = idEnrollment
                };

                _context.Student.Add(student);
                _context.SaveChanges();

                EnrollmentResponse response = new EnrollmentResponse()
                {
                    LastName = enrollmentRequest.LastName,
                    Semester = 1
                };

                return StatusCode(201, response);

            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("promotion")]
        public IActionResult PromoteStudent(PromotionRequest promotionRequest)
        {
            try
            {
                var studyId = _context
                    .Studies
                    .FirstOrDefault(study => study.Name.Equals(promotionRequest.Studies))
                    .IdStudy;

                var currentEnrollmentId = _context.Enrollment
                    .FirstOrDefault(enrollment => enrollment.Semester == promotionRequest.Semester && enrollment.IdStudy == studyId)
                    .IdEnrollment;

                if (currentEnrollmentId == null)
                {
                    return NotFound("Enrollment for semester " + promotionRequest.Semester + " and studies " +
                                    promotionRequest.Studies + " does not exist");
                }

                var futureEnrollmentIdExists = _context.Enrollment
                    .Any(enrollment =>
                        enrollment.Semester == (promotionRequest.Semester + 1) && enrollment.IdStudy == studyId);

                var futureEnrollmentId = 0;
                if (!futureEnrollmentIdExists)
                {
                    var futureEnrollment = new Enrollment()
                    {
                        IdEnrollment = (_context.Enrollment.Max(enrollment => enrollment.IdEnrollment)) + 1,
                        IdStudy = studyId,
                        Semester = promotionRequest.Semester + 1,

                    };
                    _context.Enrollment.Add(futureEnrollment);
                    _context.SaveChanges();
                }
                 futureEnrollmentId = _context.Enrollment
                    .FirstOrDefault(enrollment => enrollment.Semester == (promotionRequest.Semester + 1) && enrollment.IdStudy == studyId).IdEnrollment;
                

                List<Student> studentsForPromotion = _context.Student
                    .Where(student => student.IdEnrollment == currentEnrollmentId)
                    .ToList();
                
             
                foreach (var student in studentsForPromotion)
                {
                    student.IdEnrollment = futureEnrollmentId;
                    _context.SaveChanges();
                  
                }
                
                List<string> promotedStudentsIndexList = new List<string>();
                foreach (var student in studentsForPromotion)
                {
                    promotedStudentsIndexList.Add(student.IndexNumber);
                }

                return StatusCode(201, promotedStudentsIndexList);
            }
            catch (Exception exception)
            {
                return StatusCode(400, "Request values are incorrect. " + exception.Message);
            }
        }
    }
}

        
    
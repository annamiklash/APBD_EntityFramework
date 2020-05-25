using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;

namespace APBD_Tutorial_11.Services
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
    public class EnrollmentDbService : IEnrollmentDbService
    {
        private readonly s18458Context _context;
        private readonly IStudentDbService _studentDb;

        public EnrollmentDbService(s18458Context context, IStudentDbService studentDb)
        {
            _context = context;
            _studentDb = studentDb;
        }

        public int GetStudyId(string name)
        {
            return _context.Studies
                .FirstOrDefault(studies => studies.Name == name)
                .IdStudy;
        }

        public int GetEnrollmentId(int semester, int studyId)
        {
            return _context.Enrollment
                .FirstOrDefault(enrollment =>
                    enrollment.Semester == semester && enrollment.IdStudy == studyId)
                .IdEnrollment;
        }

        public bool EnrollmentExists(int semester, string studyName)
        {
            var studyId = GetStudyId(studyName);

            return _context.Enrollment
                .Any(enrollment =>
                    enrollment.Semester == semester && enrollment.IdStudy == studyId);
        }

        public bool StudyExists(string name)
        {
            return _context.Studies
                .Any(studies => studies.Name == name);
        }

        public EnrollmentResponse EnrollStudent(EnrollmentRequest enrollmentRequest)
        {
            var studyId = GetStudyId(enrollmentRequest.Studies);
            var enrollmentId = GetEnrollmentId(1, studyId);
            var insertRequest = new InsertStudentRequest()
            {
                IndexNumber = enrollmentRequest.IndexNumber,
                FirstName = enrollmentRequest.FirstName,
                LastName = enrollmentRequest.LastName,
                BirthDate = enrollmentRequest.BirthDate,
                Password = enrollmentRequest.Password,
                IdEnrollment = enrollmentId.ToString()

            };
            _studentDb.InsertStudent(insertRequest);

            EnrollmentResponse response = new EnrollmentResponse()
            {
                LastName = enrollmentRequest.LastName,
                Semester = 1
            };

            return response;
        }

        public List<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest, IEnumerable<Student> students)
        {
            var studyId = GetStudyId(promotionRequest.Studies);
            var futureEnrollmentIdExists = _context.Enrollment
                .Any(enrollment =>
                    enrollment.Semester == (promotionRequest.Semester + 1) && enrollment.IdStudy == studyId);

            var futureEnrollmentId = 0;
            if (!futureEnrollmentIdExists)
            {
                futureEnrollmentId = CreateNewEnrollmentForNextSemester(studyId, promotionRequest.Semester);
            }
            else
            {
                futureEnrollmentId = _context.Enrollment
                    .FirstOrDefault(enrollment =>
                        enrollment.Semester == (promotionRequest.Semester + 1) && enrollment.IdStudy == studyId)
                    .IdEnrollment;
            }

            var studentsList = students.ToList();
            studentsList.ForEach(student =>
            {
                student.IdEnrollment = futureEnrollmentId;

            });
            _context.SaveChanges();

            return studentsList.Select(student => new PromotionResponse()
                {
                    IdEnrollment = futureEnrollmentId,
                    Semester = promotionRequest.Semester + 1,
                    Student = new Student()
                    {
                        IndexNumber = student.IndexNumber,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        BirthDate = student.BirthDate,
                        Password = student.Password,
                        IdEnrollment = student.IdEnrollment
                    }
                })
                .ToList();
        }

        public IEnumerable<Student> GetStudentOnEnrollment(int semester, string studies)
        {

            var studyId = GetStudyId(studies);
            var currentEnrollmentId = GetEnrollmentId(semester, studyId);

            return _context.Student
                .Where(student => student.IdEnrollment == currentEnrollmentId)
                .ToList();
        }

        private int CreateNewEnrollmentForNextSemester(int studyId, int semester)
        {
            var futureEnrollment = new Enrollment()
            {
                IdEnrollment = (_context.Enrollment.Max(enrollment => enrollment.IdEnrollment)) + 1,
                IdStudy = studyId,
                Semester = semester + 1,

            };
            _context.Enrollment.Add(futureEnrollment);
            _context.SaveChanges();

            return futureEnrollment.IdEnrollment;
        }
    }
}
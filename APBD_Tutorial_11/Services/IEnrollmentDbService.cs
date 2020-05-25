using System.Collections.Generic;
using APBD_Tutorial_11.Models;
using APBD_Tutorial_11.Models.Response;

namespace APBD_Tutorial_11.Services
{
    public interface IEnrollmentDbService
    {
        int GetStudyId(string name);
        int GetEnrollmentId(int semester, int studyId);
        bool EnrollmentExists(int semester, string studyName);
        bool StudyExists(string name);
        EnrollmentResponse EnrollStudent(EnrollmentRequest request);
        List<PromotionResponse> PromoteStudents(PromotionRequest promotionRequest, IEnumerable<Student> students);
        IEnumerable<Student> GetStudentOnEnrollment(int semester, string studies);
    }
}
using System;

namespace APBD_Tutorial_11.Models.Response
{
    public class PromotionResponse
    {
        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public  Student Student { get; set; }
        
        public override string ToString()
        {
            return $"IdEnrollment : {IdEnrollment}, Semester {Semester}, Student {Student}, ";
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APBD_Tutorial_11.Models
{
    public class ValidationHelper
    {
        private const string INDEX_NUMBER_REGEX = "^s[0-9]+$";
        private const string NAME_REGEX = "^[A-Z][-a-zA-Z]+$";
        private const string DATE_REGEX = @"^\s*(3[01]|[12][0-9]|0?[1-9])\.(1[012]|0?[1-9])\.((?:19|20)\d{2})\s*$";

        public static List<Error> ValidateStudent(InsertStudentRequest student)
        {
            List<Error> errorList = new List<Error>();


            if (!IsIndexNumberValid(student.IndexNumber))
            {
                errorList.Add(new Error("IndexNumber", student.IndexNumber, "Invalid Index Number format. Should match " + INDEX_NUMBER_REGEX));
            }

            if (!IsNameValid(student.FirstName))
            {
                errorList.Add(new Error("FirstName", student.FirstName, "Invalid First Name format. Should match " + NAME_REGEX));
            }

            if (!IsNameValid(student.LastName))
            {
                errorList.Add(new Error("LastName", student.LastName, "Invalid Last Name format. Should match " + NAME_REGEX));
            }

            if (!IsDateValid(student.BirthDate))
            {
                errorList.Add(new Error("BirthDate", student.BirthDate.ToString(), "Invalid Date format. Should match " + DATE_REGEX));
            }

            if (string.IsNullOrEmpty(student.Password))
            {
                errorList.Add(new Error("Password", student.Password, "No Password Provided"));
            }

            return errorList;
        }

        public static List<Error> ValidateEnrollmentRequest(EnrollmentRequest enrollmentRequest)
        {
            List<Error> errorList = new List<Error>();

            if (!IsIndexNumberValid(enrollmentRequest.IndexNumber))
            {
                errorList.Add(new Error("IndexNumber", enrollmentRequest.IndexNumber, "Invalid Index Number format. Should match " + INDEX_NUMBER_REGEX));
            }

            if (!IsNameValid(enrollmentRequest.FirstName))
            {
                errorList.Add(new Error("FirstName", enrollmentRequest.FirstName, "Invalid First Name format. Should match " + NAME_REGEX));
            }

            if (!IsNameValid(enrollmentRequest.LastName))
            {
                errorList.Add(new Error("LastName", enrollmentRequest.LastName, "Invalid Last Name format. Should match " + NAME_REGEX));
            }

            if (!IsDateValid(enrollmentRequest.BirthDate))
            {
                errorList.Add(new Error("BirthDate", enrollmentRequest.BirthDate, "Invalid Date format. Should match " + DATE_REGEX));
            }

            if (string.IsNullOrEmpty(enrollmentRequest.Password))
            {
                errorList.Add(new Error("Password", enrollmentRequest.Password, "No Password Provided"));
            }

            return errorList;
        }

        public static List<Error> ValidatePromotionRequest(PromotionRequest request)
        {
            List<Error> errorList = new List<Error>();

            if (string.IsNullOrEmpty(request.Studies))
            {
                errorList.Add(new Error("Studies", request.Studies, "Studies field is empty"));
            }

            if (string.IsNullOrEmpty(request.Semester.ToString()))
            {
                errorList.Add(new Error("Semester", request.Semester.ToString(), "Semester field is empty"));
            }

            if (!IsDigit(request.Semester))
            {
                errorList.Add(new Error("Semester", request.Semester.ToString(), "Semester should be digit"));
            }

            return errorList;
        }

        public static List<Error> ValidateUpdateStudentRequest(UpdateStudentRequest updateStudentRequest)
        {
            List<Error> errorList = new List<Error>();

            if (!string.IsNullOrEmpty(updateStudentRequest.FirstName) && !IsNameValid(updateStudentRequest.FirstName))
            {
                errorList.Add(new Error("FirstName", updateStudentRequest.FirstName, "Invalid First Name format. Should match " + NAME_REGEX));
            }

            if (!string.IsNullOrEmpty(updateStudentRequest.LastName) && !IsNameValid(updateStudentRequest.LastName))
            {
                errorList.Add(new Error("LastName", updateStudentRequest.LastName, "Invalid Last Name format. Should match " + NAME_REGEX));
            }

            if (!string.IsNullOrEmpty(updateStudentRequest.BirthDate) && !IsDateValid(updateStudentRequest.BirthDate))
            {
                errorList.Add(new Error("BirthDate", updateStudentRequest.BirthDate, "Invalid Date format. Should match " + DATE_REGEX));
            }
            
            if (string.IsNullOrEmpty(updateStudentRequest.FirstName) && string.IsNullOrEmpty(updateStudentRequest
                                                                         .LastName)
                                                                     && string.IsNullOrEmpty(updateStudentRequest
                                                                         .BirthDate) &&
                                                                     string.IsNullOrEmpty(updateStudentRequest
                                                                         .IdEnrollment)
                                                                     && string.IsNullOrEmpty(updateStudentRequest
                                                                         .Password) &&
                                                                     string.IsNullOrEmpty(updateStudentRequest
                                                                         .BirthDate))
            {
                errorList.Add(new Error("multiple fields", updateStudentRequest.ToString(), "Nothing to update"));
            }

            return errorList;
        }
        private static bool EnrollmentExists(s18458Context context, int idEnrollment)
        {
            return context.Enrollment.Any(enrollment => enrollment.IdEnrollment == idEnrollment);
        }

        private static bool IsIndexNumberValid(string indexNumber)
        {
            return Regex.IsMatch(indexNumber, INDEX_NUMBER_REGEX, RegexOptions.IgnoreCase);
        }

        private static bool IsNameValid(string name)
        {
            return Regex.IsMatch(name, NAME_REGEX);
        }

        private static bool IsDateValid(string birthDate)
        {
            return Regex.IsMatch(birthDate, DATE_REGEX);
        }


        private static bool IsDigit(int input)
        {
            return Regex.IsMatch(input.ToString(), "^[0-9]+$");
        }
   
    }
    
}

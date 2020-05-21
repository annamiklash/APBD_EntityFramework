using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBD_Tutorial_11.Models
{
    public class Error { 
    public string Field { get; set; }
    public string InvalidValue { get; set; }
    public string Message { get; set; }


    public Error(string field, string invalidValue, string message)
    {
        Field = field;
        Message = message;
        InvalidValue = invalidValue;
    }
}
}

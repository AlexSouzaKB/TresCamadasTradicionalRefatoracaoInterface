using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class ValidationAttribute : Attribute
    {
        public bool Presence { set; get; }
    }
}

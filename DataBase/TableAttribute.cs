using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class TableAttribute : Attribute
    {
        public string Name { set; get; }
    }
}

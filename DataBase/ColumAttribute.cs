using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase
{
    public class ColumAttribute : Attribute
    {
        public string Name { set; get; }
        public bool PrimaryKey { set; get; }
        public string Collection { set; get; }
        public string ForeignKey { set; get; }
        public bool IsNotOnDataBase { set; get; }
    }
}

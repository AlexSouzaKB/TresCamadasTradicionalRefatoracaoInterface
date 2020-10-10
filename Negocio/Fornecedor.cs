using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DataBase;

namespace Negocio
{
    [Table(Name = "Fonecedores")]
    public class Fornecedor : DataBase.ADb
    {
        [Colum(PrimaryKey = true)]
        public int Id { get; set; }

        public string Nome { get; set; }
        
        public string CNPJ { get; set; }

        [Colum(IsNotOnDataBase = true)]
        public string CEP { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DataBase;

namespace Negocio
{
    [Table(Name = "Clientes")]
    public class Cliente : DataBase.ADb
    {
        [Colum(PrimaryKey = true)]
        public int Id { get; set; }
        [Validation(Presence = true)]
        public string Nome { get; set; }

        [Colum(Name = "cpf")]
        public string CPF { get; set; }

        [Colum(IsNotOnDataBase = true)]
        public string CEP { get; set; }

        public bool Valido()
        {
            return true;
        }

    }
}

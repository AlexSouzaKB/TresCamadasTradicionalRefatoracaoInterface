using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DataBase;

namespace Negocio
{
    //[Table(Name = "tbCliente")]
    public class Cliente : DataBase.ICliente
    {
        [Colum(PrimaryKey = true)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }

        [Colum(IsNotOnDataBase = true)]
        public string CEP { get; set; }

        public bool Valido()
        {
            return true;
        }

        public void Salvar()
        {
            if (string.IsNullOrEmpty(this.Nome))
            {
                throw new ClienteError("Nome é obrigatório");
            }

            if (string.IsNullOrEmpty(this.CPF))
            {
                throw new ClienteError("CPF é obrigatório");
            }

            new DataBase.Cliente().Salvar(this);
        }
           
        public List<Cliente> Todos()
        {
            var dados = new DataBase.Cliente().Todos();

            var clientes = new List<Cliente>();

            foreach(DataRow row in dados.Rows)
            {
                clientes.Add(new Cliente()
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nome = row["Nome"].ToString(),
                    CPF = row["CPF"].ToString()
                });
            }

            return clientes;
        }
    }

    public class ClienteError : Exception
    {
        public ClienteError(string message)
        {
            this._message = message;
        }
        private string _message;
        public override string Message { get => _message; }
    }
}

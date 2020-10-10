using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Negocio
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }

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

            this.Id = new DataBase.Cliente().Salvar(this.Id, this.Nome, this.CPF);
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

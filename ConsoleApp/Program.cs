using Negocio;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new Cliente() { Nome="Lana", CPF = "222.333.444-33"}.Salvar();
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }

            try
            {
                new Fornecedor() { Nome = "Avanade", CNPJ = "222.333.444/0001-11" }.Salvar();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}

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
            catch(ClienteError erro)
            {
                Console.WriteLine(erro.Message);
            }
            catch(Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}

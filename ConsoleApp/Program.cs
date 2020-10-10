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
                new Cliente().Salvar();
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

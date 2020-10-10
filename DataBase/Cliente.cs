using System;
using System.Data;
using System.Data.SqlClient;

namespace DataBase
{
    public class Cliente
    {
        private const string STRING_CNN = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TresCamadas;Data Source=DESKTOP-UQ8D1SH\SQLEXPRESS";

        public DataTable Todos()
        {
            string queryString ="SELECT * FROM Clientes;";

            using (SqlConnection connection =
                       new SqlConnection(STRING_CNN))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                DataTable dataTable = new DataTable();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dataTable);
                connection.Close();
                da.Dispose();

                return dataTable;
            }
        }

        public int Salvar(int id, string nome, string CPF)
        {
            string queryString ="IncluirCliente";

            using (SqlConnection connection =
                       new SqlConnection(STRING_CNN))
            {
                try
                {
                    // transation 
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@Nome", SqlDbType.VarChar);
                    command.Parameters["@Nome"].Value = nome;

                    command.Parameters.Add("@CPF", SqlDbType.VarChar);
                    command.Parameters["@CPF"].Value = CPF;

                    connection.Open();

                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch { }
                finally
                {
                    connection.Close();
                }

                return 0;
            }
        }
    }
}

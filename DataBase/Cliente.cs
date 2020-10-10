using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

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

        public void Salvar(ICliente iCliente)
        {
            using (SqlConnection connection =
                       new SqlConnection(STRING_CNN))
            {
                try
                {
                    string queryString = "insert into clientes (nome, cpf) values(@Nome, @CPF)";

                    // transation 
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.Text;

                    foreach (var p in iCliente.GetType().GetProperties())
                    {
                        ColumAttribute columAttribute = p.GetCustomAttribute<ColumAttribute>();
                        if (p.GetValue(iCliente) == null) continue;
                        if (columAttribute != null && (columAttribute.PrimaryKey || columAttribute.IsNotOnDataBase)) continue;

                        var value = p.GetValue(iCliente);
                        command.Parameters.Add($"@{p.Name}", GetDbType(value));
                        command.Parameters[$"@{p.Name}"].Value = value;
                    }

                    connection.Open();

                    command.ExecuteNonQuery();
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private SqlDbType GetDbType(object value)
        {
            var result = SqlDbType.VarChar;

            try
            {
                Type type = value.GetType();

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Object:
                        result = SqlDbType.Variant;
                        break;
                    case TypeCode.Boolean:
                        result = SqlDbType.Bit;
                        break;
                    case TypeCode.Char:
                        result = SqlDbType.NChar;
                        break;
                    case TypeCode.SByte:
                        result = SqlDbType.SmallInt;
                        break;
                    case TypeCode.Byte:
                        result = SqlDbType.TinyInt;
                        break;
                    case TypeCode.Int16:
                        result = SqlDbType.SmallInt;
                        break;
                    case TypeCode.UInt16:
                        result = SqlDbType.Int;
                        break;
                    case TypeCode.Int32:
                        result = SqlDbType.Int;
                        break;
                    case TypeCode.UInt32:
                        result = SqlDbType.BigInt;
                        break;
                    case TypeCode.Int64:
                        result = SqlDbType.BigInt;
                        break;
                    case TypeCode.UInt64:
                        result = SqlDbType.Decimal;
                        break;
                    case TypeCode.Single:
                        result = SqlDbType.Real;
                        break;
                    case TypeCode.Double:
                        result = SqlDbType.Float;
                        break;
                    case TypeCode.Decimal:
                        result = SqlDbType.Money;
                        break;
                    case TypeCode.DateTime:
                        result = SqlDbType.DateTime;
                        break;
                    case TypeCode.String:
                        result = SqlDbType.VarChar;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            return result;
        }
    }
}

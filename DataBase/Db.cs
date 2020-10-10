using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataBase
{
    public class Db
    {
        private const string STRING_CNN = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TresCamadas;Data Source=DESKTOP-UQ8D1SH\SQLEXPRESS";

        public List<ADb> Todos(ADb aDb)
        {
            TableAttribute tableAttribute = aDb.GetType().GetCustomAttribute<TableAttribute>();
            string nomeTabela = tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name) ? tableAttribute.Name : iCliente.GetType().Name;
            string queryString = $"SELECT * FROM {nomeTabela};";

            using (SqlConnection connection =
                       new SqlConnection(STRING_CNN))
            {
                SqlCommand command =
                    new SqlCommand(queryString, connection);
                connection.Open();

                var adbs = new List<ADb>();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    var newAbb = (ADb)Activator.CreateInstance(aDb.GetType());
                    foreach (var p in aDb.GetType().GetProperties())
                    {
                        ColumAttribute columAttribute = p.GetCustomAttribute<ColumAttribute>();
                        if (columAttribute != null && columAttribute.IsNotOnDataBase) continue;

                        string nomeColunaTabela = columAttribute != null && !string.IsNullOrEmpty(columAttribute.Name) ? columAttribute.Name : p.Name;
                        p.SetValue(aDb, reader[nomeColunaTabela]);
                    }

                    adbs.Add(newAbb);
                }

                reader.Close();
                connection.Close();

                return adbs;
            }
        }

        public void Salvar(ADb aDb)
        {
            using (SqlConnection connection = new SqlConnection(STRING_CNN))
            {
                try
                {
                    var colunas = new List<string>();
                    var valores = new List<object>();
                    foreach (var p in aDb.GetType().GetProperties())
                    {
                        ColumAttribute columAttribute = p.GetCustomAttribute<ColumAttribute>();
                        if (p.GetValue(aDb) == null) continue;
                        if (columAttribute != null && (columAttribute.PrimaryKey || columAttribute.IsNotOnDataBase)) continue;

                        string nomeColunaTabela = columAttribute != null && !string.IsNullOrEmpty(columAttribute.Name) ? columAttribute.Name : p.Name;
                        colunas.Add(nomeColunaTabela);
                        valores.Add(p.GetValue(aDb));
                    }

                    var parans = new List<string>();
                    foreach(var coluna in colunas)
                    {
                        parans.Add($"@{coluna}");
                    }
                    string parametros = string.Join(",", parans.ToArray());
                    string colunasJoin = string.Join(",", colunas.ToArray());

                    TableAttribute tableAttribute = aDb.GetType().GetCustomAttribute<TableAttribute>();
                    string nomeTabela = tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Name) ? tableAttribute.Name : iCliente.GetType().Name;

                    string queryString = $"insert into {nomeTabela} ({colunasJoin} values(${parametros})";

                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.CommandType = CommandType.Text;

                    for (var i=0; i<colunas.Count; i++)
                    {
                        var nomeColuna = colunas[i];
                        var valorColuna = valores[i];
                        command.Parameters.Add($"@{nomeColuna}", GetDbType(valorColuna));
                        command.Parameters[$"@{nomeColuna}"].Value = valorColuna;
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

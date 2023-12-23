using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Ficohsa.RecurringPayments.Core
{
    public class SecurityRepository
    {
        public static List<string> GetRolesList(string login)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
            var connectionString = configuration.GetConnectionString("SecurityDataBase");
            /*connectionString = new string(connectionString.Reverse().ToArray());
            var base64EncodedBytes = System.Convert.FromBase64String(connectionString);
            connectionString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);*/
            var securityAppCode = configuration.GetSection("SecurityAppCode").Value;
            var sqlConnection = new SqlConnection(connectionString);
            var command = new SqlCommand(configuration.GetSection("StoredProcedureRoles").Value)
            {
                Connection = sqlConnection,
                CommandType = CommandType.StoredProcedure,
                Parameters =
                {
                    new SqlParameter { ParameterName = "@CodigoAplicacion", SqlDbType = SqlDbType.VarChar, Value = securityAppCode, Direction = ParameterDirection.Input},
                    new SqlParameter { ParameterName = "@CodigoUsuario", SqlDbType = SqlDbType.VarChar, Value = login, Direction = ParameterDirection.Input},
                    new SqlParameter { ParameterName = "@ErrorMessage", SqlDbType = SqlDbType.NVarChar, Value = string.Empty, Direction = ParameterDirection.Output},
                    new SqlParameter { ParameterName = "@ErrorSeverity", SqlDbType = SqlDbType.Int, Value = 0, Direction = ParameterDirection.Output},
                    new SqlParameter { ParameterName = "@ErrorState", SqlDbType = SqlDbType.Int, Value = 0, Direction = ParameterDirection.Output},
                }
            };
            var sqlDataAdapter = new SqlDataAdapter
            {
                SelectCommand = command,
            };
            var data = new DataSet();
            var rowCount = sqlDataAdapter.Fill(data);
            var result = new List<string>();
            if (rowCount > 0 && (data?.Tables?.Count ?? 0) > 0 && data.Tables[0].Rows.Count > 0)
            {
                result.AddRange(from DataRow row in data.Tables[0].Rows let name = row[1].ToString() select name);
            }
            data.Dispose();
            sqlDataAdapter.Dispose();
            command.Dispose();
            sqlConnection.Close();
            sqlConnection.Dispose();
            return result;
        }
    }
}
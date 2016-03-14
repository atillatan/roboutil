using RoboUtil.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RoboUtil.Utils
{
    public class DynamicDbUtil
    {
        public static SqlConnection CreateConnection(string connectionString)
        {             
            return new SqlConnection(connectionString);
        }
        #region base methods
        public static dynamic Get(SqlConnection connection, string commandText, params object[] args)
        {
            dynamic result = null;

            using (SqlCommand sqlCommand = CreateSqlCommand(connection, null, CommandType.Text, commandText, args))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                result = MapToExpandoObject(reader);
                reader.Close();
            }
            return result;
        }
        public static List<dynamic> List(SqlConnection connection, string commandText, params object[] args)
        {
            List<dynamic> result = new List<dynamic>();

            using (SqlCommand sqlCommand = CreateSqlCommand(connection, null, CommandType.Text, commandText, args))
            {
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.HasRows)
                {
                    result.Add(MapToExpandoObject(reader));
                }
                reader.Close();
            }
            return result;
        }
        public static List<dynamic> List(SqlConnection connection, string commandText, int pageNumber, int rowsPage, params object[] args)
        {
            if (!commandText.ToUpper(System.Globalization.CultureInfo.CurrentCulture).Contains("ORDER BY")) throw new Exception("commandText must contains ORDER BY expression!");

            commandText = string.Format(@"             
                                        {0}
                                        OFFSET (({1} - 1) * {2} ROWS
                                        FETCH NEXT {2} ROWS ONLY                                         
                                        ", commandText, pageNumber, rowsPage);

            return List(connection, commandText, null);
        }
        public static int Execute(SqlConnection connection, string commandText, params object[] args)
        {
            using (SqlCommand sqlCommand = CreateSqlCommand(connection, null, CommandType.Text, commandText, args))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }
        public static int Execute(SqlConnection connection, SqlTransaction transaction, string commandText, params object[] args)
        {
            using (SqlCommand sqlCommand = CreateSqlCommand(connection, transaction, CommandType.Text, commandText, args))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }
        #endregion

        #region extended methods
        public static T Get<T>(SqlConnection connection, string commandText, params object[] args)
        {
            return ExpandoObjectMapper.Map<T>(Get(connection, commandText, args));
        }
        public static List<T> List<T>(SqlConnection connection, string commandText, params object[] args)
        {
            return ExpandoObjectMapper.ToMap<T>(List(connection, commandText, args));
        }
        public static List<T> List<T>(SqlConnection connection, string commandText, int pageIndex, int PageCount, string sortExpression, params object[] args)
        {
            return ExpandoObjectMapper.ToMap<T>(List(connection, commandText, pageIndex, PageCount, args));
        }
        public static int Execute(SqlConnection connection, SqlTransaction transaction, string commandText,CommandType commandType, params object[] args)
        {
            using (SqlCommand sqlCommand = CreateSqlCommand(connection, transaction, commandType, commandText, args))
            {
                return sqlCommand.ExecuteNonQuery();
            }
        }

        #endregion

        #region private methods
        private static dynamic MapToExpandoObject(SqlDataReader reader)
        {
            dynamic result = null;

            if (reader.HasRows) result = new ExpandoObject();

            if (reader.Read())
            {
                if (reader.FieldCount == 1)
                {
                    result = (!(reader[0] is DBNull) ? reader[0] : null);
                }
                else
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string[] fields = reader.GetName(i).Split('.');

                        if (fields.Length == 1)
                        {
                            (result as IDictionary<string, object>)[fields[0]] = (!(reader[i] is DBNull) ? reader[i] : null);
                        }
                        else
                        {
                            if ((!(reader[i] is DBNull) ? reader[i] : null) != null)
                            {
                                (result as IDictionary<string, object>)[fields[0]] = MapToExpandoObject(fields.Where(f => f != fields[0]).ToArray(), (!(reader[i] is DBNull) ? reader[i] : null),
                                    (result as IDictionary<string, object>).Keys.Contains(fields[0]) ? (result as IDictionary<string, object>)[fields[0]] : new ExpandoObject());
                            }
                        }
                    }
                }
            }

            return result;
        }
        private static dynamic MapToExpandoObject(string[] fields, object value, dynamic o)
        {
            dynamic result = o;

            if (fields.Length == 0)
                return value;

            IDictionary<string, object> fieldDict = result as IDictionary<string, object>;

            string currentField = fields[0];

            string[] nextFields = fields.Where(f => f != currentField).ToArray();

            dynamic obj = fieldDict.Keys.Contains(currentField) ? fieldDict[currentField] : new ExpandoObject();

            fieldDict[currentField] = MapToExpandoObject(nextFields, value, obj);

            return fieldDict;
        }
        private static SqlCommand CreateSqlCommand(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, object[] args)
        {
            FormatParameters(ref commandText, args);

            SqlCommand sqlCommand = connection.CreateCommand();
            sqlCommand.CommandText = commandText;
            sqlCommand.Connection = connection;
            sqlCommand.Transaction = transaction;

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
                    {
                        int index = 0;
                        foreach (var arg in (args[i] as IEnumerable))
                        {
                            sqlCommand.Parameters.AddWithValue(string.Format("@p{0}inp{1}", i, index), arg ?? DBNull.Value);
                            index++;
                        }
                    }
                    else
                    {
                        SqlParameter parameter = new SqlParameter(string.Format("@p{0}", i), args[i] ?? DBNull.Value);
                        parameter.IsNullable = args[i] == null;
                        sqlCommand.Parameters.Add(parameter);
                    }
                }
            }

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return sqlCommand;
        }
        private static void FormatParameters(ref string commandText, object[] args)
        {
            if (args != null)
            {
                string[] parameterNames = new string[args.Length];

                for (int i = 0; i < args.Length; i++)
                {
                    if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
                    {
                        int index = 0;
                        string parameterNameValues = string.Empty;
                        StringBuilder sbParameterNameValues = new StringBuilder();

                        foreach (var arg in (args[i] as IEnumerable))
                        {
                            sbParameterNameValues.Append(string.Format("@p{0}inp{1},", i, index));
                            index++;
                        }
                        parameterNameValues = sbParameterNameValues.ToString();
                        parameterNames[i] = parameterNameValues.Substring(0, parameterNameValues.Length - 1);
                    }
                    else
                    {
                        parameterNames[i] = string.Format("@p{0}", i);
                    }
                }

                commandText = string.Format(commandText, parameterNames);
            }
        }
        #endregion        

    }
}

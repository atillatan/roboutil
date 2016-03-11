using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.managers
{
    public class DbManager
    {

        //public static bool Execute(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];
        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        //Usis.Common.Logging.Logger.Debug("## Execute Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        returnValue = command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //        logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return returnValue > 0;
        //}

        //public static bool ExecuteSil(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];
        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        //Usis.Common.Logging.Logger.Debug("## Execute Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //    try
        //    {
        //        watch.Start();
        //        command.CommandTimeout = 120;
        //        //connection.Open();
        //        returnValue = command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //        logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return returnValue > 0;
        //}


        //public static bool ExecuteSQL(string commandText)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];
        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        Usis.Common.Logging.Logger.Debug("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = new SqlCommand(commandText, connection);

        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        returnValue = command.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //        logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return returnValue > 0;
        //}

        //public static int ExecuteReturnValue(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];
        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        Usis.Common.Logging.Logger.Debug("## ExecuteReturnValue Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        var myReader = command.ExecuteReader();

        //        while (myReader.Read()) // read from resultset
        //        {
        //            if (myReader.GetInt32(0) > -1)
        //            {
        //                returnValue = myReader.GetInt32(0); // (2) GET LAST INSERTED ID
        //            }
        //        }

        //        //  returnValue = (int)command.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //        logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return returnValue;
        //}
        //public static bool ExecuteWithNewConnection(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    {
        //        SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //        try
        //        {
        //            connection.Open();
        //            returnValue = command.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            sqlException = ex;
        //            throw ex;
        //        }
        //        finally
        //        {
        //            List<string> values = new List<string>();
        //            foreach (SqlParameter parameter in command.Parameters)
        //            {
        //                values.Add(parameter.Value.ToString());
        //            }
        //            difference = DateTime.UtcNow.Subtract(startTime);
        //            AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //            logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //        }
        //    }
        //    return returnValue > 1;
        //}

        //public static int ExecuteWithNewConnectionReturnValue(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    int returnValue = 0;
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    {
        //        SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //        try
        //        {
        //            connection.Open();
        //            returnValue = command.ExecuteNonQuery();
        //        }
        //        catch (Exception ex)
        //        {
        //            sqlException = ex;
        //            throw ex;
        //        }
        //        finally
        //        {
        //            List<string> values = new List<string>();
        //            foreach (SqlParameter parameter in command.Parameters)
        //            {
        //                values.Add(parameter.Value.ToString());
        //            }
        //            difference = DateTime.UtcNow.Subtract(startTime);
        //            AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.MODIFY;
        //            logAudit("Db.Execute", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //        }
        //    }
        //    return returnValue;
        //}

        //public static List<dynamic> List(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    List<dynamic> result = new List<dynamic>();

        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];

        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        //Usis.Common.Logging.Logger.Debug("## List Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = CreateSqlCommand(connection, commandText, args);
        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        //command.CommandTimeout = 60 * 60 * 60;
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            if (reader.FieldCount == 1)
        //            {
        //                result.Add(!(reader[0] is DBNull) ? reader[0] : null);
        //            }
        //            else
        //            {
        //                dynamic item = new ExpandoObject();

        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    //(item as IDictionary<string, dynamic>)[reader.GetName(i)] = (!(reader[i] is DBNull) ? reader[i] : null);
        //                    //(result as IDictionary<string, object>)[reader.GetName(i)] = (!(reader[i] is DBNull) ? reader[i] : null);
        //                    string[] fields = reader.GetName(i).Split('.');

        //                    if (fields.Length == 1)
        //                    {
        //                        (item as IDictionary<string, object>)[fields[0]] = (!(reader[i] is DBNull) ? reader[i] : null);
        //                    }
        //                    else
        //                    {
        //                        if ((!(reader[i] is DBNull) ? reader[i] : null) != null)
        //                        {
        //                            (item as IDictionary<string, object>)[fields[0]] = DynamicMap(fields.Where(f => f != fields[0]).ToArray(), (!(reader[i] is DBNull) ? reader[i] : null),
        //                                (item as IDictionary<string, object>).Keys.Contains(fields[0]) ? (item as IDictionary<string, object>)[fields[0]] : new ExpandoObject());
        //                        }
        //                    }
        //                }

        //                result.Add(item);
        //                //  Usis.Common.Logging.Logger.Debug("result item count:" + result.Count);
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {

        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.SELECT;
        //        logAudit("Db.List", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);

        //    }
        //    //}
        //    return result;
        //}

        //public static List<dynamic> Fetch(FetchParameters parameters, string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    List<dynamic> result = new List<dynamic>();
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();

        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];

        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        Usis.Common.Logging.Logger.Debug("## Fetch Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlCommand command = CreateSqlFetchCommand(parameters, connection, commandText, args);
        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            if (reader.FieldCount == 1)
        //            {
        //                result.Add(!(reader[0] is DBNull) ? reader[0] : null);
        //            }
        //            else
        //            {
        //                dynamic item = new ExpandoObject();

        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    string[] fields = reader.GetName(i).Split('.');

        //                    if (fields.Length == 1)
        //                    {
        //                        (item as IDictionary<string, object>)[fields[0]] = (!(reader[i] is DBNull) ? reader[i] : null);
        //                    }
        //                    else
        //                    {
        //                        if ((!(reader[i] is DBNull) ? reader[i] : null) != null)
        //                        {
        //                            (item as IDictionary<string, object>)[fields[0]] = DynamicMap(fields.Where(f => f != fields[0]).ToArray(), (!(reader[i] is DBNull) ? reader[i] : null),
        //                                (item as IDictionary<string, object>).Keys.Contains(fields[0]) ? (item as IDictionary<string, object>)[fields[0]] : new ExpandoObject());
        //                        }
        //                    }
        //                }

        //                result.Add(item);
        //            }
        //            parameters.TotalCount = Convert.ToInt32(reader["RowCount"]);
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.SELECT;
        //        logAudit("Db.Fetch", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return result;
        //}

        //public static dynamic Get(string commandText, params object[] args)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    dynamic result = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];

        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        //Usis.Common.Logging.Logger.Debug("## Get Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }

        //    SqlCommand command = CreateSqlCommand(connection, commandText, args);

        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            result = new ExpandoObject();
        //        }

        //        if (reader.Read())
        //        {

        //            if (reader.FieldCount == 1)
        //            {
        //                result = (!(reader[0] is DBNull) ? reader[0] : null);
        //            }
        //            else
        //            {
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    string[] fields = reader.GetName(i).Split('.');

        //                    if (fields.Length == 1)
        //                    {
        //                        (result as IDictionary<string, object>)[fields[0]] = (!(reader[i] is DBNull) ? reader[i] : null);
        //                    }
        //                    else
        //                    {
        //                        if ((!(reader[i] is DBNull) ? reader[i] : null) != null)
        //                        {

        //                            (result as IDictionary<string, object>)[fields[0]] = DynamicMap(fields.Where(f => f != fields[0]).ToArray(), (!(reader[i] is DBNull) ? reader[i] : null),
        //                                (result as IDictionary<string, object>).Keys.Contains(fields[0]) ? (result as IDictionary<string, object>)[fields[0]] : new ExpandoObject());
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        reader.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        List<string> values = new List<string>();
        //        foreach (SqlParameter parameter in command.Parameters)
        //        {
        //            values.Add(parameter.Value.ToString());
        //        }
        //        //connection.Close();
        //        if (IsNewConnection) { connection.Close(); }
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", commandText, string.Join(", ", values)), sqlException, "Database Query Executed.", DateTime.Now, watch.ElapsedMilliseconds);
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.SELECT;
        //        logAudit("Db.Get", string.Format("[Val:{0}][Ex:{1}][Sql:{2}]", string.Join(", ", values), Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), commandText), difference.Milliseconds, operationType);
        //    }
        //    //}
        //    return result;
        //}

        //private static dynamic DynamicMap(string[] fields, object value, dynamic o)
        //{


        //    dynamic result = o;

        //    if (fields.Length == 0)
        //    {
        //        return value;
        //    }
        //    else
        //    {

        //        (result as IDictionary<string, object>)[fields[0]] = DynamicMap(fields.Where(f => f != fields[0]).ToArray(), value,
        //            (result as IDictionary<string, object>).Keys.Contains(fields[0]) ? (result as IDictionary<string, object>)[fields[0]] : new ExpandoObject());

        //        return result;
        //    }
        //}

        //private static SqlCommand CreateSqlCommand(SqlConnection connection, string commandText, object[] args)
        //{
        //    if (args != null)
        //    {
        //        string[] parameterNames = new string[args.Length];

        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
        //            {
        //                int index = 0;
        //                string parameterNameValues = string.Empty;
        //                foreach (var arg in (args[i] as IEnumerable))
        //                {
        //                    parameterNameValues += string.Format("@p{0}inp{1},", i, index);
        //                    index++;
        //                }
        //                parameterNames[i] = parameterNameValues.Substring(0, parameterNameValues.Length - 1);
        //            }
        //            else
        //            {
        //                parameterNames[i] = string.Format("@p{0}", i);
        //            }
        //        }

        //        commandText = string.Format(commandText, parameterNames);
        //    }

        //    SqlCommand sqlCommand = null;
        //    if (OperationContext.Current != null && OperationContext.Current.RequestContext.RequestMessage.Properties.Keys.Contains("DATABASE_TRANSACTION"))
        //    {
        //        SqlTransaction tran = (SqlTransaction)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_TRANSACTION"];
        //        sqlCommand = new SqlCommand(string.Format("/* {0} */ {1}", GetMethod(), commandText), connection, tran);
        //    }
        //    else
        //    {
        //        sqlCommand = new SqlCommand(string.Format("/* {0} */ {1}", GetMethod(), commandText), connection);
        //    }

        //    if (args != null)
        //    {
        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
        //            {
        //                int index = 0;
        //                foreach (var arg in (args[i] as IEnumerable))
        //                {
        //                    sqlCommand.Parameters.AddWithValue(string.Format("@p{0}inp{1}", i, index), arg ?? DBNull.Value);
        //                    index++;
        //                }
        //            }
        //            else
        //            {
        //                SqlParameter parameter = new SqlParameter(string.Format("@p{0}", i), args[i] ?? DBNull.Value);

        //                parameter.IsNullable = args[i] == null;

        //                sqlCommand.Parameters.Add(parameter);
        //            }
        //        }
        //    }

        //    return sqlCommand;
        //}

        //private static SqlCommand CreateSqlFetchCommand(FetchParameters parameters, SqlConnection connection, string commandText, object[] args)
        //{
        //    parameters.SortExpression = Regex.Replace(parameters.SortExpression, @"[^a-zA-Z,\s\.\[\]]", string.Empty);

        //    if (args != null)
        //    {
        //        string[] parameterNames = new string[args.Length];

        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
        //            {
        //                int index = 0;
        //                string parameterNameValues = string.Empty;
        //                foreach (var arg in (args[i] as IEnumerable))
        //                {
        //                    parameterNameValues += string.Format("@p{0}inp{1},", i, index);
        //                    index++;
        //                }
        //                parameterNames[i] = parameterNameValues.Substring(0, parameterNameValues.Length - 1);
        //            }
        //            else
        //            {
        //                parameterNames[i] = string.Format("@p{0}", i);
        //            }
        //        }

        //        commandText = string.Format(commandText, parameterNames);
        //    }

        //    commandText = string.Format(@"
        //    /* {0} */ 
        //    WITH Data AS
        //    (
        //        SELECT *, ROW_NUMBER() OVER (ORDER BY {1}) AS 'RowNumber', Count(*) OVER() 'RowCount'
        //     FROM
        //     (
        //      {4}
        //     ) X
        //    )
        //    SELECT *
        //    FROM Data
        //    WHERE RowNumber BETWEEN {2} AND {3};
        //    ", GetMethod(), parameters.SortExpression, (parameters.PageIndex * parameters.PageCount) + 1, (parameters.PageIndex + 1) * parameters.PageCount, commandText);

        //    SqlCommand sqlCommand = new SqlCommand(commandText, connection);

        //    if (args != null)
        //    {
        //        for (int i = 0; i < args.Length; i++)
        //        {
        //            if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
        //            {
        //                int index = 0;
        //                foreach (var arg in (args[i] as IEnumerable))
        //                {
        //                    sqlCommand.Parameters.AddWithValue(string.Format("@p{0}inp{1}", i, index), arg ?? DBNull.Value);
        //                    index++;
        //                }
        //            }
        //            else
        //            {
        //                sqlCommand.Parameters.AddWithValue(string.Format("@p{0}", i), args[i] ?? DBNull.Value);
        //            }
        //        }
        //    }

        //    return sqlCommand;
        //}

        //private static string GetMethod()
        //{
        //    string result = "";
        //    StackTrace stackTrace = new StackTrace();

        //    for (int i = 0; i < stackTrace.FrameCount; i++)
        //    {
        //        StackFrame stackFrame = stackTrace.GetFrame(i);
        //        MethodBase method = stackFrame.GetMethod();
        //        if (method.Module.Name == "STRATEGIA.CIMS.Service.Core.dll")
        //        {
        //            result = string.Format("{0}.{1}", method.DeclaringType.FullName, method.Name);
        //        }
        //    }

        //    return result;
        //}

        ////Mustafa Yoldaş
        //public static void insertBulk(DataTable dataTable, string TableName)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    //using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    //{
        //    SqlConnection connection = null;
        //    if (OperationContext.Current != null)
        //        connection = (SqlConnection)OperationContext.Current.RequestContext.RequestMessage.Properties["DATABASE_CONNECTION"];

        //    bool IsNewConnection = false;
        //    if (connection == null)
        //    {
        //        ///Usis.Common.Logging.Logger.Debug("## insertBulk Uyari! Connection context haricinde oluşturuldu.");
        //        connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString);
        //        connection.Open();
        //        IsNewConnection = true;
        //    }
        //    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.UseInternalTransaction, null);
        //    bulkCopy.DestinationTableName = TableName;

        //    try
        //    {
        //        watch.Start();
        //        //connection.Open();                
        //        bulkCopy.WriteToServer(dataTable);
        //    }
        //    catch (Exception ex)
        //    {
        //        sqlException = ex;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        watch.Stop();
        //        //AtillaReplace:LogManager.Current.Write(null, LogType.DatabaseQuery, LogSeverity.Information, STRATEGIA.CIMS.Security.Layer.Data, string.Format("{0} [{1}]", "bulk insert", ""), sqlException, "Database Bulk insert", DateTime.Now, watch.ElapsedMilliseconds);

        //        StringBuilder output = new StringBuilder();
        //        foreach (DataRow rows in dataTable.Rows)
        //        {
        //            foreach (DataColumn col in dataTable.Columns)
        //            {
        //                output.AppendFormat("{0} ", rows[col]);
        //            }
        //            output.AppendLine();
        //        }
        //        difference = DateTime.UtcNow.Subtract(startTime);
        //        if (IsNewConnection) { connection.Close(); }
        //        AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.SELECT;
        //        logAudit("Db.InsertBulk", string.Format("[TableName:{0}][Ex:{1}][Sql:{2}]", TableName, Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), output.ToString()), difference.Milliseconds, operationType);
        //    }
        //    //}           
        //}

        //public static void insertBulkWithNewConnection(DataTable dataTable, string TableName)
        //{
        //    Stopwatch watch = new Stopwatch();
        //    Exception sqlException = null;
        //    DateTime startTime = DateTime.UtcNow;
        //    TimeSpan difference = new TimeSpan();
        //    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[CIMSConfigurationSection.Current.Service.DataConnectionString].ConnectionString))
        //    {
        //        connection.Open();
        //        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.UseInternalTransaction, null);
        //        bulkCopy.DestinationTableName = TableName;

        //        try
        //        {
        //            bulkCopy.WriteToServer(dataTable);
        //        }
        //        catch (Exception ex)
        //        {
        //            sqlException = ex;
        //            throw ex;
        //        }
        //        finally
        //        {
        //            StringBuilder output = new StringBuilder();
        //            foreach (DataRow rows in dataTable.Rows)
        //            {
        //                foreach (DataColumn col in dataTable.Columns)
        //                {
        //                    output.AppendFormat("{0} ", rows[col]);
        //                }
        //                output.AppendLine();
        //            }
        //            difference = DateTime.UtcNow.Subtract(startTime);
        //            AuditLogEnum operationType = sqlException != null ? AuditLogEnum.EXCEPTION : AuditLogEnum.SELECT;
        //            logAudit("Db.InsertBulk", string.Format("[TableName:{0}][Ex:{1}][Sql:{2}]", TableName, Usis.Common.Util.GeneralUtil.ToDetailString(sqlException), output.ToString()), difference.Milliseconds, operationType);
        //        }
        //    }
        //}

        //private static void logAudit(string methodName, string methodRequest, int TotalTime, AuditLogEnum auditLogEnum)
        //{
        //    string kullaniciAdi = "";
        //    int kullaniciPk = -1;
        //    string sessionId = "";
        //    string ipAddress = "";
        //    string activePage = "";

        //    try
        //    {
        //        //excluding methods
        //        //string[] excludingMethods = new string[] { "getCacheInfoDto", "FaalCount", "KamuYarariCountIller", "GetExportFiles" };
        //        //if (excludingMethods.Contains(methodName)) return;
        //        Identity identity = Identity.Current;
        //        if (identity != null)
        //        {
        //            if (identity.User != null)
        //            {
        //                kullaniciAdi = identity.User.Username != null ? identity.User.Username : null;
        //                kullaniciPk = identity.User.No != 0 ? identity.User.No : -1;

        //                //atilla>kullanici sistem ise toplu islem yapiliyordur log tutulmayacak
        //                if (kullaniciAdi.Equals("sistem"))
        //                {
        //                    return;
        //                }
        //            }

        //            if (identity.Action != null)
        //            {
        //                sessionId = identity.Action.TrackingCode != null ? identity.Action.TrackingCode : "";
        //                ipAddress = identity.Action.IpAddress != null ? identity.Action.IpAddress : "";
        //                activePage = identity.Action.Url != null ? identity.Action.Url : "";
        //            }
        //        }

        //        string controlStr = methodRequest.ToUpper();

        //        if (controlStr.Contains("INSERT") || controlStr.Contains("UPDATE") || controlStr.Contains("DELETE"))
        //        {
        //            auditLogEnum = AuditLogEnum.MODIFY;
        //        }
        //        else
        //        {
        //            auditLogEnum = AuditLogEnum.SELECT;
        //        }

        //        AuditLogManager.Instance.Log(methodName, methodRequest.Trim(), auditLogEnum, kullaniciAdi, kullaniciPk.ToString(), sessionId, ipAddress, activePage, TotalTime);
        //    }
        //    catch (Exception auditEx)
        //    {
        //        Usis.Common.Logging.Logger.Error(auditEx);
        //    }
        //}

    }
}

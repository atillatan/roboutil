using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using EmitMapper.MappingConfiguration.MappingOperations;
using EmitMapper.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.utils
{
    public static class DatabaseUtil
    {
        #region base methods
        public static DbConnection CreateConnection(string connectionString, string providerName)
        {
            var factory = DbProviderFactories.GetFactory(providerName);
            if (factory == null)
                return null;
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        }
        public static DbConnection CreateConnection(string configurationName)
        {
            var connectionSetting = System.Configuration.ConfigurationManager.ConnectionStrings[configurationName];
            if (connectionSetting == null)
                return null;

            var factory = DbProviderFactories.GetFactory(connectionSetting.ProviderName);
            if (factory == null)
                return null;
            var connection = factory.CreateConnection();
            connection.ConnectionString = connectionSetting.ConnectionString;
            return connection;
        }
        public static int ExecuteNonQuery(DbConnection conn, CommandType commandType, string commandText, params object[] args)
        {
            using (var cmd = CreateCommand(conn, null, commandType, commandText, args))
            {
                return cmd.ExecuteNonQuery();
            }
        }
        public static T Get<T>(DbConnection connection, string commandText, params object[] args)
        {
            T result;

            using (DbCommand cmd = CreateCommand(connection, null, CommandType.Text, commandText, args))
            {
                EmitMapper<T> dd = new EmitMapper<T>();
                DbDataReader reader = cmd.ExecuteReader();
                result = dd.ReadSingle(reader);
                reader.Close();
            }

            return result;
        }
        public static dynamic Get(DbConnection connection, string commandText, params object[] args)
        {
            dynamic result = null;

            using (DbCommand cmd = CreateCommand(connection, null, CommandType.Text, commandText, args))
            {
                EmitMapper<dynamic> dd = new EmitMapper<dynamic>();
                DbDataReader reader = cmd.ExecuteReader();
                result = dd.ReadSingle(reader);
                reader.Close();
            }

            return result;
        }
        public static List<T> List<T>(DbConnection connection, string commandText, params object[] args)
        {
            List<T> result = new List<T>();

            using (DbCommand cmd = CreateCommand(connection, null, CommandType.Text, commandText, args))
            {
                EmitMapper<T> dd = new EmitMapper<T>();
                DbDataReader reader = cmd.ExecuteReader();
                result = dd.ReadCollection(reader).ToList();
                reader.Close();
            }
            return result;
        }
        public static List<T> List<T>(DbConnection connection, string commandText, int pageNumber, int rowsPage, params object[] args)
        {
            if (!commandText.ToUpper(System.Globalization.CultureInfo.CurrentCulture).Contains("ORDER BY")) throw new Exception("commandText must contains ORDER BY expression!");

            commandText = string.Format(@"             
                                        {0}
                                        OFFSET (({1} - 1) * {2} ROWS
                                        FETCH NEXT {2} ROWS ONLY                                         
                                        ", commandText, pageNumber, rowsPage);
            return List<T>(connection, commandText, null);
        }
        #endregion

        #region extended methods
        public static object ExecuteScalar(DbConnection conn, CommandType commandType, string commandText, params object[] args)
        {
            using (var cmd = CreateCommand(conn, null, commandType, commandText, args))
            {
                return cmd.ExecuteScalar();
            }
        }
        public static T ExecuteScalar<T>(DbConnection conn, CommandType commandType, string commandText, params object[] args)
        {
            object result = ExecuteScalar(conn, commandType, commandText, args);
            if (typeof(T) == typeof(Guid))
            {
                if (result == null)
                {
                    return (T)((object)Guid.Empty);
                }
                return (T)((object)new Guid(result.ToString()));
            }
            if (result is DBNull || result == null)
            {
                return default(T);
            }
            return (T)Convert.ChangeType(result, typeof(T));
        }
        #endregion

        #region private methods
        public static DbCommand CreateCommand(DbConnection connection, DbTransaction transaction, CommandType commandType, string commandText, object[] args)
        {
            FormatParameters(ref commandText, args);

            DbCommand dbCommand = connection.CreateCommand();
            dbCommand.CommandText = commandText;
            dbCommand.Connection = connection;
            dbCommand.Transaction = transaction;

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (!(args[i] is String) && !(args[i] is IEnumerable<byte>) && args[i] is IEnumerable)
                    {
                        int k = 0;
                        foreach (var arg in (args[i] as IEnumerable))
                        {
                            DbParameter p = dbCommand.CreateParameter();
                            p.ParameterName = string.Format("@p{0}inp{1}", i, k);
                            p.Value = arg ?? DBNull.Value;
                            p.IsNullable = arg == null;
                            dbCommand.Parameters.Add(p);
                            k++;
                        }
                    }
                    else
                    {
                        DbParameter p = dbCommand.CreateParameter();
                        p.ParameterName = string.Format("@p{0}", i);
                        p.Value = args[i] ?? DBNull.Value;
                        p.IsNullable = args[i] == null;
                        dbCommand.Parameters.Add(p);
                    }
                }
            }

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            return dbCommand;
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

    public class EmitMapper<T> : ObjectsMapper<DbDataReader, T>
    {
        class DbReaderMappingConfig : IMappingConfigurator
        {
            class ReaderValuesExtrator<T>
            {
                public Func<int, DbDataReader, T> valueExtractor;
                public int fieldNum;
                public string fieldName;

                public ReaderValuesExtrator(string fieldName, Func<int, DbDataReader, T> valueExtractor)
                {
                    fieldNum = -1;
                    this.fieldName = fieldName;
                    this.valueExtractor = valueExtractor;
                }

                public Delegate ExtrationDelegate
                {
                    get
                    {
                        return (ValueGetter<T>)
                            (
                                (value, state) =>
                                {
                                    return ValueToWrite<T>.ReturnValue(GetValue((DbDataReader)state));
                                }
                            );
                    }
                }

                private T GetValue(DbDataReader reader)
                {
                    if (fieldNum == -1)
                    {
                        fieldNum = reader.GetOrdinal(fieldName);
                    }
                    return reader.IsDBNull(fieldNum) ? default(T) : valueExtractor(fieldNum, reader);
                }
            }

            IEnumerable<string> _skipFields;
            string _mappingKey;

            public DbReaderMappingConfig(IEnumerable<string> skipFields, string mappingKey)
            {
                _skipFields = skipFields == null ? new List<string>() : skipFields;
                _mappingKey = mappingKey;
            }

            public IRootMappingOperation GetRootMappingOperation(Type from, Type to)
            {
                return null;
            }

            private Delegate GetValuesGetter(int ind, MemberInfo m)
            {
                var memberType = ReflectionUtils.GetMemberType(m);

                if (_mappingKey != null)
                {
                    if (memberType == typeof(string))
                    {
                        return new ReaderValuesExtrator<string>(m.Name, (idx, reader) => reader.IsDBNull(idx) ? null : reader.GetString(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(bool))
                    {
                        return new ReaderValuesExtrator<bool>(m.Name, (idx, reader) => reader.GetBoolean(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(bool?))
                    {
                        return new ReaderValuesExtrator<bool?>(m.Name, (idx, reader) => reader.GetBoolean(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int16))
                    {
                        return new ReaderValuesExtrator<Int16>(m.Name, (idx, reader) => reader.GetInt16(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int16?))
                    {
                        return new ReaderValuesExtrator<Int16?>(m.Name, (idx, reader) => reader.GetInt16(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int32))
                    {
                        return new ReaderValuesExtrator<Int32>(m.Name, (idx, reader) => reader.GetInt32(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int32?))
                    {
                        return new ReaderValuesExtrator<Int32?>(m.Name, (idx, reader) => reader.GetInt32(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int64))
                    {
                        return new ReaderValuesExtrator<Int64>(m.Name, (idx, reader) => reader.GetInt64(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Int64?))
                    {
                        return new ReaderValuesExtrator<Int64?>(m.Name, (idx, reader) => reader.GetInt64(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(byte))
                    {
                        return new ReaderValuesExtrator<byte>(m.Name, (idx, reader) => reader.GetByte(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(byte?))
                    {
                        return new ReaderValuesExtrator<byte?>(m.Name, (idx, reader) => reader.GetByte(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(char))
                    {
                        return new ReaderValuesExtrator<char>(m.Name, (idx, reader) => reader.GetChar(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(char?))
                    {
                        return new ReaderValuesExtrator<char?>(m.Name, (idx, reader) => reader.GetChar(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(DateTime))
                    {
                        return new ReaderValuesExtrator<DateTime>(m.Name, (idx, reader) => reader.GetDateTime(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(DateTime?))
                    {
                        return new ReaderValuesExtrator<DateTime?>(m.Name, (idx, reader) => reader.GetDateTime(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(decimal))
                    {
                        return new ReaderValuesExtrator<decimal>(m.Name, (idx, reader) => reader.GetDecimal(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(decimal?))
                    {
                        return new ReaderValuesExtrator<decimal?>(m.Name, (idx, reader) => reader.GetDecimal(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(double))
                    {
                        return new ReaderValuesExtrator<double>(m.Name, (idx, reader) => reader.GetDouble(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(double?))
                    {
                        return new ReaderValuesExtrator<double?>(m.Name, (idx, reader) => reader.GetDouble(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(float))
                    {
                        return new ReaderValuesExtrator<float>(m.Name, (idx, reader) => reader.GetFloat(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(float?))
                    {
                        return new ReaderValuesExtrator<float?>(m.Name, (idx, reader) => reader.GetFloat(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Guid))
                    {
                        return new ReaderValuesExtrator<Guid>(m.Name, (idx, reader) => reader.GetGuid(idx)).ExtrationDelegate;
                    }
                    else if (memberType == typeof(Guid?))
                    {
                        return new ReaderValuesExtrator<Guid?>(m.Name, (idx, reader) => reader.GetGuid(idx)).ExtrationDelegate;
                    }
                }

                Func<object, object> converter = StaticConvertersManager.DefaultInstance.GetStaticConverterFunc(typeof(object), memberType);
                if (converter == null)
                {
                    throw new EmitMapperException("Could not convert an object to " + memberType.ToString());
                }
                int fieldNum = -1;
                string fieldName = m.Name;
                return
                    (ValueGetter<object>)
                    (
                        (value, state) =>
                        {
                            var reader = ((DbDataReader)state);
                            object result = null;
                            if (_mappingKey != null)
                            {
                                if (fieldNum == -1)
                                {
                                    fieldNum = reader.GetOrdinal(fieldName);
                                }
                                result = reader[fieldNum];
                            }
                            else
                            {
                                result = reader[fieldName];
                            }

                            if (result is DBNull)
                            {
                                return ValueToWrite<object>.ReturnValue(null);
                            }
                            return ValueToWrite<object>.ReturnValue(converter(result));
                        }
                    )
                    ;
            }

            public IMappingOperation[] GetMappingOperations(Type from, Type to)
            {
                return ReflectionUtils
                    .GetPublicFieldsAndProperties(to)
                    .Where(
                        m => m.MemberType == MemberTypes.Field ||
                            m.MemberType == MemberTypes.Property && ((PropertyInfo)m).GetSetMethod() != null
                    )
                    .Where(m => !_skipFields.Select(sf => sf.ToUpper()).Contains(m.Name.ToUpper()))
                    .Select(
                        (m, ind) =>
                            new DestWriteOperation()
                            {
                                Destination = new MemberDescriptor(new[] { m }),
                                Getter = GetValuesGetter(ind, m)
                            }
                    )
                    .ToArray();
            }

            public string GetConfigurationName()
            {
                if (_mappingKey != null)
                {
                    return "dbreader_" + _mappingKey;
                }
                else
                {
                    return "dbreader_";
                }
            }

            public StaticConvertersManager GetStaticConvertersManager()
            {
                return null;
            }
        }

        public EmitMapper(
            string mappingKey,
            ObjectMapperManager mapperManager,
            IEnumerable<string> skipFields)
            : base(GetMapperImpl(mappingKey, mapperManager, skipFields))
        {
        }

        public EmitMapper(ObjectMapperManager mapperManager)
            : this(null, mapperManager, null)
        {
        }

        public EmitMapper()
            : this(null, null, null)
        {
        }

        public EmitMapper(IEnumerable<string> skipFields)
            : this(null, null, skipFields)
        {
        }

        public T ReadSingle(DbDataReader reader)
        {
            T result = reader.Read() ? MapUsingState(reader, reader) : default(T);
            return result;
        }

        public IEnumerable<T> ReadCollection(DbDataReader reader)
        {
            while (reader.Read())
            {
                T result = MapUsingState(reader, reader);
                yield return result;
            }
            reader.Close();
        }

        private static ObjectsMapperBaseImpl GetMapperImpl(
            string mappingKey,
            ObjectMapperManager mapperManager,
            IEnumerable<string> skipFields)
        {
            IMappingConfigurator config = new DbReaderMappingConfig(skipFields, mappingKey);

            if (mapperManager != null)
            {
                return mapperManager.GetMapperImpl(
                    typeof(DbDataReader),
                    typeof(T),
                    config);
            }
            else
            {
                return ObjectMapperManager.DefaultInstance.GetMapperImpl(
                    typeof(DbDataReader),
                    typeof(T),
                    config);
            }
        }
    }
}

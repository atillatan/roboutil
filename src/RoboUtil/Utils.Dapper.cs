using Dapper.Contrib.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;

using System.Reflection;
using System.Reflection.Emit;

//using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using static Dapper.Contrib.Extensions.SqlMapperExtensions;

namespace RoboUtil
{
    static partial class Utils
    {
        public static class Dapper
        {
            internal static Dictionary<Type, DbType> oracleDbTypeMap;
            internal static Dictionary<Type, DbType> dbTypeMap;

            static Dapper()
            {
                oracleDbTypeMap = new Dictionary<Type, DbType>
                {
                    [typeof(byte)] = DbType.Byte,
                    [typeof(sbyte)] = DbType.SByte,
                    [typeof(short)] = DbType.Int16,
                    [typeof(ushort)] = DbType.UInt16,
                    [typeof(int)] = DbType.Int32,
                    [typeof(Int64)] = DbType.Decimal,
                    [typeof(uint)] = DbType.UInt32,
                    [typeof(long)] = DbType.Int64,
                    [typeof(ulong)] = DbType.UInt64,
                    [typeof(float)] = DbType.Single,
                    [typeof(double)] = DbType.Double,
                    [typeof(decimal)] = DbType.Decimal,
                    [typeof(bool)] = DbType.Decimal,
                    [typeof(string)] = DbType.String,
                    [typeof(char)] = DbType.StringFixedLength,
                    [typeof(Guid)] = DbType.Guid,
                    [typeof(DateTime)] = DbType.DateTime,
                    [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                    [typeof(TimeSpan)] = DbType.Time,
                    [typeof(byte[])] = DbType.Binary,
                    [typeof(byte?)] = DbType.Byte,
                    [typeof(sbyte?)] = DbType.SByte,
                    [typeof(short?)] = DbType.Int16,
                    [typeof(ushort?)] = DbType.UInt16,
                    [typeof(int?)] = DbType.Int32,
                    [typeof(uint?)] = DbType.UInt32,
                    [typeof(long?)] = DbType.Int64,
                    [typeof(ulong?)] = DbType.UInt64,
                    [typeof(float?)] = DbType.Single,
                    [typeof(double?)] = DbType.Double,
                    [typeof(decimal?)] = DbType.Decimal,
                    [typeof(bool?)] = DbType.Decimal,
                    [typeof(char?)] = DbType.StringFixedLength,
                    [typeof(Guid?)] = DbType.Guid,
                    [typeof(DateTime?)] = DbType.DateTime,
                    [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                    [typeof(TimeSpan?)] = DbType.Time,
                    [typeof(object)] = DbType.Object
                };

                dbTypeMap = new Dictionary<Type, DbType>
                {
                    [typeof(byte)] = DbType.Byte,
                    [typeof(sbyte)] = DbType.SByte,
                    [typeof(short)] = DbType.Int16,
                    [typeof(ushort)] = DbType.UInt16,
                    [typeof(int)] = DbType.Int32,
                    [typeof(uint)] = DbType.UInt32,
                    [typeof(long)] = DbType.Int64,
                    [typeof(ulong)] = DbType.UInt64,
                    [typeof(float)] = DbType.Single,
                    [typeof(double)] = DbType.Double,
                    [typeof(decimal)] = DbType.Decimal,
                    [typeof(bool)] = DbType.Boolean,
                    [typeof(string)] = DbType.String,
                    [typeof(char)] = DbType.StringFixedLength,
                    [typeof(Guid)] = DbType.Guid,
                    [typeof(DateTime)] = DbType.DateTime,
                    [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
                    [typeof(TimeSpan)] = DbType.Time,
                    [typeof(byte[])] = DbType.Binary,
                    [typeof(byte?)] = DbType.Byte,
                    [typeof(sbyte?)] = DbType.SByte,
                    [typeof(short?)] = DbType.Int16,
                    [typeof(ushort?)] = DbType.UInt16,
                    [typeof(int?)] = DbType.Int32,
                    [typeof(uint?)] = DbType.UInt32,
                    [typeof(long?)] = DbType.Int64,
                    [typeof(ulong?)] = DbType.UInt64,
                    [typeof(float?)] = DbType.Single,
                    [typeof(double?)] = DbType.Double,
                    [typeof(decimal?)] = DbType.Decimal,
                    [typeof(bool?)] = DbType.Boolean,
                    [typeof(char?)] = DbType.StringFixedLength,
                    [typeof(Guid?)] = DbType.Guid,
                    [typeof(DateTime?)] = DbType.DateTime,
                    [typeof(DateTimeOffset?)] = DbType.DateTimeOffset,
                    [typeof(TimeSpan?)] = DbType.Time,
                    [typeof(object)] = DbType.Object
                };
            }

            public static void AddTypeMap(Type type, DbType dbType)
            {
                var snapshot = Dapper.dbTypeMap;

                DbType oldValue;
                if (snapshot.TryGetValue(type, out oldValue) && oldValue == dbType) return; // nothing to do

                var newCopy = new Dictionary<Type, DbType>(snapshot) { [type] = dbType };
                Dapper.dbTypeMap = newCopy;
            }

            public static class ProxyGenerator
            {
                private static readonly Dictionary<Type, Type> TypeCache = new Dictionary<Type, Type>();

                private static AssemblyBuilder GetAsmBuilder(string name)
                {
#if COREFX
                return AssemblyBuilder.DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
#else
                    return Thread.GetDomain().DefineDynamicAssembly(new AssemblyName { Name = name }, AssemblyBuilderAccess.Run);
#endif
                }

                public static T GetInterfaceProxy<T>()
                {
                    Type typeOfT = typeof(T);

                    Type k;
                    if (TypeCache.TryGetValue(typeOfT, out k)) return (T)Activator.CreateInstance(k);

                    var assemblyBuilder = GetAsmBuilder(typeOfT.Name);

                    var moduleBuilder = assemblyBuilder.DefineDynamicModule("Utils.Dapper." + typeOfT.Name); //NOTE: to save, add "asdasd.dll" parameter

                    var interfaceType = typeof(IProxy);
                    var typeBuilder = moduleBuilder.DefineType(typeOfT.Name + "_" + Guid.NewGuid(),
                        TypeAttributes.Public | TypeAttributes.Class);
                    typeBuilder.AddInterfaceImplementation(typeOfT);
                    typeBuilder.AddInterfaceImplementation(interfaceType);

                    //create our _isDirty field, which implements IProxy
                    var setIsDirtyMethod = CreateIsDirtyProperty(typeBuilder);

                    // Generate a field for each property, which implements the T
                    foreach (var property in typeof(T).GetProperties())
                    {
                        var isId = property.GetCustomAttributes(true).Any(a => a is KeyAttribute);
                        CreateProperty<T>(typeBuilder, property.Name, property.PropertyType, setIsDirtyMethod, isId);
                    }

#if COREFX
                var generatedType = typeBuilder.CreateTypeInfo().AsType();
#else
                    var generatedType = typeBuilder.CreateType();
#endif

                    TypeCache.Add(typeOfT, generatedType);
                    return (T)Activator.CreateInstance(generatedType);
                }

                private static MethodInfo CreateIsDirtyProperty(TypeBuilder typeBuilder)
                {
                    var propType = typeof(bool);
                    var field = typeBuilder.DefineField("_" + "IsDirty", propType, FieldAttributes.Private);
                    var property = typeBuilder.DefineProperty("IsDirty",
                                                   System.Reflection.PropertyAttributes.None,
                                                   propType,
                                                   new[] { propType });

                    const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.NewSlot | MethodAttributes.SpecialName |
                                                        MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.HideBySig;

                    // Define the "get" and "set" accessor methods
                    var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + "IsDirty",
                                                 getSetAttr,
                                                 propType,
                                                 Type.EmptyTypes);
                    var currGetIl = currGetPropMthdBldr.GetILGenerator();
                    currGetIl.Emit(OpCodes.Ldarg_0);
                    currGetIl.Emit(OpCodes.Ldfld, field);
                    currGetIl.Emit(OpCodes.Ret);
                    var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + "IsDirty",
                                                 getSetAttr,
                                                 null,
                                                 new[] { propType });
                    var currSetIl = currSetPropMthdBldr.GetILGenerator();
                    currSetIl.Emit(OpCodes.Ldarg_0);
                    currSetIl.Emit(OpCodes.Ldarg_1);
                    currSetIl.Emit(OpCodes.Stfld, field);
                    currSetIl.Emit(OpCodes.Ret);

                    property.SetGetMethod(currGetPropMthdBldr);
                    property.SetSetMethod(currSetPropMthdBldr);
                    var getMethod = typeof(IProxy).GetMethod("get_" + "IsDirty");
                    var setMethod = typeof(IProxy).GetMethod("set_" + "IsDirty");
                    typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                    typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);

                    return currSetPropMthdBldr;
                }

                private static void CreateProperty<T>(TypeBuilder typeBuilder, string propertyName, Type propType, MethodInfo setIsDirtyMethod, bool isIdentity)
                {
                    //Define the field and the property
                    var field = typeBuilder.DefineField("_" + propertyName, propType, FieldAttributes.Private);
                    var property = typeBuilder.DefineProperty(propertyName,
                                                   System.Reflection.PropertyAttributes.None,
                                                   propType,
                                                   new[] { propType });

                    const MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.Virtual |
                                                        MethodAttributes.HideBySig;

                    // Define the "get" and "set" accessor methods
                    var currGetPropMthdBldr = typeBuilder.DefineMethod("get_" + propertyName,
                                                 getSetAttr,
                                                 propType,
                                                 Type.EmptyTypes);

                    var currGetIl = currGetPropMthdBldr.GetILGenerator();
                    currGetIl.Emit(OpCodes.Ldarg_0);
                    currGetIl.Emit(OpCodes.Ldfld, field);
                    currGetIl.Emit(OpCodes.Ret);

                    var currSetPropMthdBldr = typeBuilder.DefineMethod("set_" + propertyName,
                                                 getSetAttr,
                                                 null,
                                                 new[] { propType });

                    //store value in private field and set the isdirty flag
                    var currSetIl = currSetPropMthdBldr.GetILGenerator();
                    currSetIl.Emit(OpCodes.Ldarg_0);
                    currSetIl.Emit(OpCodes.Ldarg_1);
                    currSetIl.Emit(OpCodes.Stfld, field);
                    currSetIl.Emit(OpCodes.Ldarg_0);
                    currSetIl.Emit(OpCodes.Ldc_I4_1);
                    currSetIl.Emit(OpCodes.Call, setIsDirtyMethod);
                    currSetIl.Emit(OpCodes.Ret);

                    //TODO: Should copy all attributes defined by the interface?
                    if (isIdentity)
                    {
                        var keyAttribute = typeof(KeyAttribute);
                        var myConstructorInfo = keyAttribute.GetConstructor(new Type[] { });
                        var attributeBuilder = new CustomAttributeBuilder(myConstructorInfo, new object[] { });
                        property.SetCustomAttribute(attributeBuilder);
                    }

                    property.SetGetMethod(currGetPropMthdBldr);
                    property.SetSetMethod(currSetPropMthdBldr);
                    var getMethod = typeof(T).GetMethod("get_" + propertyName);
                    var setMethod = typeof(T).GetMethod("set_" + propertyName);
                    typeBuilder.DefineMethodOverride(currGetPropMthdBldr, getMethod);
                    typeBuilder.DefineMethodOverride(currSetPropMthdBldr, setMethod);
                }
            }
        }

        #region Extensions

        public static DbType ToOracleDbType(this Type type)
        {
            return Dapper.oracleDbTypeMap[type];
        }

        public static DbType ToDbType(this Type type)
        {
            return Dapper.dbTypeMap[type];
        }

        public static string GetDbTableName(this Type type)
        {
            IDictionary<string, object> tableNameCache = Utils.Cache("TypeNameTableName");

            object name;
            if (tableNameCache.TryGetValue(type.Name, out name)) return name.ToString();

            var tableAttr = type
#if COREFX
                            .GetTypeInfo()
#endif
                            .GetCustomAttributes(false).SingleOrDefault(attr => attr.GetType().Name == "TableAttribute") as dynamic;

            if (tableAttr != null)
                name = tableAttr.Name;
            else
            {
                name = type.Name;
                if (type.IsInterface() && name.ToString().StartsWith("I"))
                    name = name.ToString().Substring(1);
            }

            tableNameCache[type.Name] = name;
            return name.ToString();
        }

        #endregion Extensions
    }
}
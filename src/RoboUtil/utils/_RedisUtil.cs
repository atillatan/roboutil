//using System;
//using System.IO;

//using BookSleeve;
//using RoboUtil.managers;
//using RoboUtil.managers.cache;
//using ProtoBuf;

//namespace RoboUtil.utils
//{
//    /// <summary>
//    /// Takes RedisConf class from constructor, or
//    /// takes xml  configuration filepath
//    /// RedisUtil alwasy connectin to redis server
//    /// RedisUtil handle all operation with redis
//    /// </summary>
//    public static class RedisUtil
//    {
//        private static object _redisLock = new object();

//        private static RedisConnection _redisConnection;

//        public static RedisConnection Connection
//        {
//            get
//            {
//                if (_redisConnection != null) return _redisConnection;

//                lock (_redisLock)
//                {
//                    if (_redisConnection != null) return _redisConnection;

//                    var server = ConfigManager.Current.GetConfig<string>("redis.server.address1", "localhost:6379");

//                    if (server == null) return null;

//                    int i = server.IndexOf(':');
//                    if (i == -1) throw new Exception("Misconfigured RedisServerAddress, expected [host]:[port]");

//                    var host = server.Substring(0, i);

//                    var portStr = server.Substring(i + 1);

//                    //var connectionOptions = "";

//                    //if (portStr.Contains(","))
//                    //{
//                    //int j = portStr.IndexOf(',');
//                    //connectionOptions = portStr.Substring(j);
//                    //portStr = portStr.Substring(0, j);
//                    //}

//                    var port = Int32.Parse(portStr);

//                    _redisConnection = new RedisConnection(host, port);
//                    _redisConnection.Open();
//                }

//                _redisConnection.Closed += delegate { _redisConnection = null; };
//                _redisConnection.Error +=
//                    delegate
//                    {
//                        try
//                        {
//                            // Suspect connection, bail and re-establish
//                            _redisConnection.Close(false);
//                        }
//                        catch { }
//                    };

//                return _redisConnection;
//            }
//        }

//        public static void Add<T>(int redisDBNo, string name, T o) where T : class
//        {
//            byte[] bytes;
//            using (var stream = new MemoryStream())
//            {
//                Serializer.Serialize<T>(stream, o);
//                bytes = stream.ToArray();
//            }

//            var task = Connection.Strings.Set(redisDBNo, name, bytes, true);
//            Connection.Wait(task);
//        }

//        public static void AddWithExpiry<T>(int redisDBNo, string name, T o, TimeSpan expiresIn) where T : class
//        {
//            byte[] bytes;
//            using (var stream = new MemoryStream())
//            {
//                Serializer.Serialize<T>(stream, o);
//                bytes = stream.ToArray();
//            }

//            var task = Connection.Strings.Set(redisDBNo, name, (int)expiresIn.TotalSeconds, bytes, true);
//            Connection.Wait(task);
//        }

//        public static T GetValue<T>(string name, int redisDBNo) where T : class
//        {
//            var reps = Connection.Strings.Get(redisDBNo, name, false);
//            var bytes = reps.Result;

//            if (bytes == null) return null;

//            using (var stream = new MemoryStream(bytes))
//            {
//                return Serializer.Deserialize<T>(stream);
//            }

//        }
//    }
//}

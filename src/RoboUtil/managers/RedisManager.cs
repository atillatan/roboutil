using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.managers
{
    /// <summary>
    /// Takes RedisConf class from constructor, or
    /// takes xml  configuration filepath
    /// RedisManager alwasy connectin to redis server
    /// RedisManager handle all operation with redis
    /// </summary>
    public static  class RedisManager
    {
    //    private static RedisConnection Redis
    //    {
    //        get
    //        {
    //            if (_redisConnection != null) return _redisConnection;

    //            lock (_redisLock)
    //            {
    //                if (_redisConnection != null) return _redisConnection;

    //                var server = WebConfigurationManager.AppSettings["RedisServerAddress"];

    //                if (server == null) return null;

    //                int i = server.IndexOf(':');
    //                if (i == -1) throw new Exception("Misconfigured RedisServerAddress, expected [host]:[port]");

    //                var host = server.Substring(0, i);
    //                var port = Int32.Parse(server.Substring(i + 1));

    //                _redisConnection = new RedisConnection(host, port);
    //                _redisConnection.Open();
    //            }

    //            _redisConnection.Closed += delegate { _redisConnection = null; };
    //            _redisConnection.Error +=
    //                delegate
    //                {
    //                    try
    //                    {
    //                        // Suspect connection, bail and re-establish
    //                        _redisConnection.Close(false);
    //                    }
    //                    catch { }
    //                };

    //            return _redisConnection;
    //        }
    //    }
    }
}

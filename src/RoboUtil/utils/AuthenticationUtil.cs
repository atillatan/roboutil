//using System;
//using System.IO;
//using System.Linq;
//using System.Security.Cryptography;
//using System.Text;
////using BookSleeve;
//using Microsoft.AspNetCore.Http;
//using ProtoBuf;
//using RoboUtil.managers;
//using RoboUtil.managers.cache;

//namespace RoboUtil.utils
//{
//    public static class AuthenticationUtil
//    {
//        public static AesCryptoServiceProvider AesProvider;
//        public static readonly string UserCookieName = "usr";
//        public static readonly string AnonymousCookieName = "anon";
//        public static RandomNumberGenerator RandomSource = RandomNumberGenerator.Create();

//        static AuthenticationUtil()
//        {
//            AesProvider = new AesCryptoServiceProvider();
//        }

//        public static void GenerateAnonymousXSRFCookie(HttpContext context)
//        {
//            var random = UniqueId().ToString();

//            AddCookie(context, AnonymousCookieName, random, TimeSpan.FromMinutes(15));

//            var token = UniqueId();
//            var cookieHash = WeakHash(random);

//            AddToCache("xsrf-" + cookieHash, token.ToString(), TimeSpan.FromMinutes(15));

//            // Can't allow for anon and usr to coexist, things get funky.
//            if (context.Request.Cookies.ContainsKey(UserCookieName))
//            {
//                KillCookie(context, UserCookieName);
//            }
//        }

//        public static Guid? XSRFToken(HttpContext context)
//        {

//            var user = context.Request.Cookies.FirstOrDefault(a => a.Key.Equals(UserCookieName)).Value.ToString();

//            string toHash;

//            if (user.HasValue())
//            {
//                toHash = user;
//            }
//            else
//            {
//                var anon = context.Request.Cookies.FirstOrDefault(a => a.Key.Equals(AnonymousCookieName)).Value.ToString();

//                if (anon == null) return null;

//                toHash = anon;
//            }

//            if (toHash == null) return null;

//            var hash = WeakHash(toHash);

//            var retStr = GetFromCache<string>("xsrf-" + hash);

//            // If the user is logged in and *needs* an XSRF, we should just create one if there isn't already one
//            if (retStr == null && user != null)
//                return GenerateXSRFToken(user);

//            // Things got a little wonky, need to clean up
//            if (retStr == null)
//            {
//                KillCookie(context, AnonymousCookieName);
//                throw new Exception("Bad cookie keyed XSRF token");
//            }

//            return Guid.Parse(retStr);

//        }

//        private static Guid GenerateXSRFToken(string userCookieValue)
//        {
//            if (userCookieValue == null) throw new Exception("Cannot generated an XSRF token this way for an anonymous user.");

//            var ret = UniqueId();

//            var newToken =
//                new
//                {
//                    CookieHash = WeakHash(userCookieValue),
//                    CreationDate = DateTime.UtcNow,
//                    Token = ret
//                };

//            AddToCache("xsrf-" + newToken.CookieHash, ret.ToString(), TimeSpan.FromDays(1));

//            return ret;
//        }

//        public static void AddCookie(HttpContext context, string cookieName, string value, TimeSpan expiresIn)
//        {
//            var cookieOpt = new CookieOptions();
//            cookieOpt.Expires = DateTime.UtcNow + expiresIn;
//            cookieOpt.HttpOnly = true;

//#if !DEBUG_HTTP
//            cookieOpt.Secure = true;
//#endif

//            context.Response.Cookies.Append(cookieName, value, cookieOpt);

//            //   Basically, this tells IE that we're not doing anything nefarious (just tracking for tailoring and dev purposes)
//            //   ... no other browser even pretends to care.
//            context.Response.Headers["p3p"] = @"CP=""NOI CURa ADMa DEVa TAIa OUR BUS IND UNI COM NAV INT""";
//        }

//        public static void KillCookie(HttpContext context, string cookieName)
//        {
//            if (!context.Request.Cookies.ContainsKey(cookieName)) return;

//            var killCookie = new CookieOptions();
//            killCookie.Expires = DateTime.UtcNow.AddMinutes(-15);
//            killCookie.HttpOnly = true;
//            killCookie.Secure = true;

//            context.Response.Cookies.Delete(cookieName);
//            context.Response.Cookies.Append(cookieName, "null", killCookie);
//        }

//        public static Guid UniqueId()
//        {
//            var bytes = Random(16);
//            bytes[7] = (byte)((bytes[7] & 0x0F) | 0x40);  // Set the GUID version to 4
//            bytes[8] = (byte)((bytes[8] & 0x0F) | (0x80 + (Random(1)[0] % 4))); // tweaking 8th byte as required

//            return new Guid(bytes);
//        }

//        public static byte[] Random(int bytes)
//        {
//            //using (MiniProfiler.Current.Step("Random"))
//            //{
//            var ret = new byte[bytes];

//            lock (RandomSource)
//                RandomSource.GetBytes(ret);

//            return ret;
//            //}
//        }

//        public static string WeakHash(string value)
//        {
//            //using (MiniProfiler.Current.Step("WeakHash"))
//            //{
//            var hasher = SHA1.Create();

//            byte[] bytes = value.HasValue() ? Encoding.UTF8.GetBytes(value) : new byte[0];

//            return Convert.ToBase64String(hasher.ComputeHash(bytes));
//            //}
//        }

//        public static void AddToCache<T>(string name, T o, TimeSpan expiresIn) where T : class
//        {

//            if (o != null)
//            {
//                RedisConnection redis = RedisUtil.Connection;

//                if (redis == null)
//                {

//                    ICache cache = CacheManager.Instance.CreateOrGetCache("AuthenticationCache",
//                         new CacheProperties()
//                         {
//                             CacheCollectionType = CacheCollectionType.DictionaryCache,
//                             CacheItemExpireDuration = expiresIn.Minutes,//minutes
//                             IsSlidingExpiration = false
//                         });

//                    cache.Add(name, o);

//                    //HttpRuntime.Cache.Insert(name, o, null, Current.Now + expiresIn, Cache.NoSlidingExpiration);
//                }
//                else
//                {
//                    byte[] bytes;
//                    using (var stream = new MemoryStream())
//                    {
//                        Serializer.Serialize<T>(stream, o);
//                        bytes = stream.ToArray();
//                    }

//                    int db = ConfigManager.Current.GetConfig<int>("redis.authenticationdb.number", 1);

//                    var task = redis.Strings.Set(db, "oid-" + name, (int)expiresIn.TotalSeconds, bytes, true);
//                    redis.Wait(task);
//                }
//            }
//        }

//        public static T GetFromCache<T>(string name) where T : class
//        {
//            RedisConnection redis = RedisUtil.Connection;

//            if (redis == null)
//            {
//                ICache cache = CacheManager.Instance.GetCache("AuthenticationCache");
//                return cache.GetValue<T>(name);
//                //return HttpRuntime.Cache[name] as T;
//            }
//            else
//            {
//                int db = ConfigManager.Current.GetConfig<int>("redis.authenticationdb.number", 1);

//                var reps = redis.Strings.Get(db, "oid-" + name, false);
//                var bytes = reps.Result;

//                if (bytes == null) return null;

//                using (var stream = new MemoryStream(bytes))
//                {
//                    return Serializer.Deserialize<T>(stream);
//                }
//            }
//        }

//        public static void Login(HttpContext context, DateTime now)
//        {
//            //Validate login model

//            //Getuser from database

//            //check for IPBlock

//            // Kill the anonymous cookie
//            KillCookie(context, AnonymousCookieName);

//            // Write a login event

//            // Generate and write a session
//            var session = Convert.ToBase64String(Random(32));
//            var sessionHash = WeakHash(session);

//            //set sessioncreation date, and lastactivitydate
          

//            // Add the user session cookie
//            AddCookie(context,UserCookieName, session, TimeSpan.FromDays(7));

//            // writeuser to database
//        }

//        public static void Logout(HttpContext context, string returnUrl)
//        {
//            // Delete this users session cookie
//            KillCookie(context, UserCookieName);

//            //logout from external providers

//            //write user history event
             
//            //set user object to null

          
//            //redirect
//        }
//    }
//}

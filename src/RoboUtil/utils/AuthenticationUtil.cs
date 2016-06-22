using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Http;
using RoboUtil.managers;
using RoboUtil.managers.cache;

namespace RoboUtil.utils
{
    public static class AuthenticationUtil
    {


        public static AesCryptoServiceProvider AesProvider;
        public static readonly string UserCookieName = "usr";
        public static readonly string AnonymousCookieName = "anon";
        public static RandomNumberGenerator RandomSource = RandomNumberGenerator.Create();

        static AuthenticationUtil()
        {
            AesProvider = new AesCryptoServiceProvider();
        }

        public static void GenerateAnonymousXSRFCookie(HttpContext context)
        {
            var random = UniqueId().ToString();

            AddCookie(context, AnonymousCookieName, random, TimeSpan.FromMinutes(15));

            var token = UniqueId();
            var cookieHash = WeakHash(random);

            AddToCache("xsrf-" + cookieHash, token.ToString(), TimeSpan.FromMinutes(15));

            // Can't allow for anon and usr to coexist, things get funky.
            if (context.Request.Cookies.ContainsKey(UserCookieName))
            {
                KillCookie(context, UserCookieName);
            }
        }

        public static void AddCookie(HttpContext context, string cookieName, string value, TimeSpan expiresIn)
        {
            var cookieOpt = new CookieOptions();
            cookieOpt.Expires = DateTime.UtcNow + expiresIn;
            cookieOpt.HttpOnly = true;

#if !DEBUG_HTTP
            cookieOpt.Secure = true;
#endif

            context.Response.Cookies.Append(cookieName, value, cookieOpt);

            //   Basically, this tells IE that we're not doing anything nefarious (just tracking for tailoring and dev purposes)
            //   ... no other browser even pretends to care.
            context.Response.Headers["p3p"] = @"CP=""NOI CURa ADMa DEVa TAIa OUR BUS IND UNI COM NAV INT""";
        }

        public static void KillCookie(HttpContext context, string cookieName)
        {
            if (!context.Request.Cookies.ContainsKey(cookieName)) return;

            var killCookie = new CookieOptions();
            killCookie.Expires = DateTime.UtcNow.AddMinutes(-15);
            killCookie.HttpOnly = true;
            killCookie.Secure = true;

            context.Response.Cookies.Delete(cookieName);
            context.Response.Cookies.Append(cookieName, "null", killCookie);
        }

        public static Guid UniqueId()
        {
            var bytes = Random(16);
            bytes[7] = (byte)((bytes[7] & 0x0F) | 0x40);  // Set the GUID version to 4
            bytes[8] = (byte)((bytes[8] & 0x0F) | (0x80 + (Random(1)[0] % 4))); // tweaking 8th byte as required

            return new Guid(bytes);
        }

        public static byte[] Random(int bytes)
        {
            //using (MiniProfiler.Current.Step("Random"))
            //{
            var ret = new byte[bytes];

            lock (RandomSource)
                RandomSource.GetBytes(ret);

            return ret;
            //}
        }

        public static string WeakHash(string value)
        {
            //using (MiniProfiler.Current.Step("WeakHash"))
            //{
            var hasher = SHA1.Create();

            byte[] bytes = value.HasValue() ? Encoding.UTF8.GetBytes(value) : new byte[0];

            return Convert.ToBase64String(hasher.ComputeHash(bytes));
            //}
        }

        public static void AddToCache<T>(string name, T o, TimeSpan expiresIn) where T : class
        {

            if (o != null)
            {
                //var redis = Redis;

                //if (redis == null)
                //{

                ICache cache = CacheManager.Instance.CreateOrGetCache("CookieCache",
                     new CacheProperties()
                     {
                         CacheCollectionType = CacheCollectionType.DictionaryCache,
                         CacheItemExpireDuration = 15,//minutes
                         IsSlidingExpiration = false
                     });

                cache.Add(name, o);

                //HttpRuntime.Cache.Insert(name, o, null, Current.Now + expiresIn, Cache.NoSlidingExpiration);
                //}
                //else
                //{
                //    byte[] bytes;
                //    using (var stream = new MemoryStream())
                //    {
                //        Serializer.Serialize<T>(stream, o);
                //        bytes = stream.ToArray();
                //    }

                //    var task = redis.SetWithExpiry(RedisDB.Value.Value, "oid-" + name, (int)expiresIn.TotalSeconds, bytes, true);
                //    redis.Wait(task);
                //}
            }
        }

    }
}

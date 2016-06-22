using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
 
using System.Linq;

using System.Collections;
 
using RoboUtil.dto;
using RoboUtil.managers;
using RoboUtil.managers.cache;
using RoboUtil.managers.thread;
using RoboUtil.utils;

namespace com.robonom.example
{
    public class CacheManagerExample
    {
        public static void ExampleCreateCache1()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("key" + i, "value" + i);


            // Creating Caches
            CacheManager.Instance.CreateCache("TestCache1");
            CacheManager.Instance.GetCache("TestCache1").Add("test1Key", "test1Value");
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache1").GetValue<string>("test1Key"));
            //
            CacheManager.Instance.CreateCache("TestCache2", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache });
            CacheManager.Instance.GetCache("TestCache2").Add("test2Key", "test2Value");
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache2").GetValue<string>("test2Key"));
            //
            CacheManager.Instance.CreateCache("TestCache3", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache3").GetValue<string>("key1"));
            //
            CacheManager.Instance.CreateCache("TestCache4", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100 }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache4").GetValue<string>("key2"));
            //
            CacheManager.Instance.CreateCache("TestCache5", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100 }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache5").GetValue<string>("key3"));
            //
            CacheManager.Instance.CreateCache("TestCache6", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100, ControlPeriod = 1000 }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache6").GetValue<string>("key4"));
            //            
            CacheManager.Instance.CreateCache("TestCache7", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100, ControlPeriod = 1000, CacheItemExpireDuration = 1 }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache7").GetValue<string>("key5"));
            //
            CacheManager.Instance.CreateCache("TestCache71", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, CacheItemExpireDuration = 1 }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache71").GetValue<string>("key51"));
            //            
            CacheManager.Instance.CreateCache("TestCache8", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100, ControlPeriod = 1000, CacheItemExpireDuration = 1, IsSlidingExpiration = true }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache8").GetValue<string>("key6"));
            //            
            CacheManager.Instance.CreateCache("TestCache9", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100, ControlPeriod = 1000, CacheItemExpireDuration = 1, IsSlidingExpiration = true, CacheItemExpireFunction = ExpireFuncForCacheItem }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache9").GetValue<string>("key7"));

            //
            CacheManager.Instance.CreateOrGetCache("Ulkeler");
            CacheManager.Instance.CreateOrGetCache("Ulkeler", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, Capacity = 100, ControlPeriod = 1000, CacheItemExpireDuration = 1, IsSlidingExpiration = true, CacheItemExpireFunction = ExpireFuncForCacheItem }, myDictionary);
            //
            CacheManager.Instance.CreateCache("TestCache10", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache, CacheExpireDuration = 1, CacheExpireFunction = ReloadCache });
            CacheManager.Instance.GetCache("TestCache10").Add("test10Key", "test10Value");
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache10").GetValue<string>("test10Key"));

            //// Cache te veri islemleri CRUD

            ////Create
            CacheManager.Instance.GetCache("Ulkeler").Add("tr", "Turkiye");
            CacheManager.Instance.GetCache("Ulkeler").AddAll(myDictionary);

            //Read
            object myObj = CacheManager.Instance.GetCache("Ulkeler").GetValue<string>("tr");
            IList<string> ls1 = CacheManager.Instance.GetCache("Ulkeler").GetValues<string>(new List<string>() { "tr", "en" });

            ////Delete
            CacheManager.Instance.GetCache("Ulkeler").Remove("tr");
            CacheManager.Instance.GetCache("Ulkeler").RemoveKeys(new List<string>() { "tr", "en" });
            CacheManager.Instance.GetCache("Ulkeler").Clear();
        }
        public static void ExampleCreateCache2()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("key" + i, "value" + i);


            //Cache olusturma yontemleri           
            CacheManager.Instance.CreateCache("MTestCache1");
            CacheManager.Instance.GetCache("MTestCache1").Add("test1Key", "test1Value");
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache1").GetValue<string>("test1Key"));
            //
            CacheManager.Instance.CreateCache("MTestCache2", new CacheProperties(), myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache2").GetValue<string>("key1"));
            //
            CacheManager.Instance.CreateCache("MTestCache3", new CacheProperties() { Capacity = 100 }, myDictionary);//100mb
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache3").GetValue<string>("key3"));
            //            
            CacheManager.Instance.CreateCache("MTestCache4", new CacheProperties() { Capacity = 100, CacheItemExpireDuration = 1 }, myDictionary);//100mb, 1 minute
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache4").GetValue<string>("key5"));
            //            
            CacheManager.Instance.CreateCache("MTestCache5", new CacheProperties() { Capacity = 100, CacheItemExpireDuration = 1, IsSlidingExpiration = true }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache5").GetValue<string>("key6"));
            //            
            CacheManager.Instance.CreateCache("MTestCache6", new CacheProperties() { Capacity = 100, CacheItemExpireDuration = 1, IsSlidingExpiration = true, CacheItemExpireFunction = ExpireFuncForCacheItem }, myDictionary);
            Console.WriteLine(CacheManager.Instance.GetCache("MTestCache6").GetValue<string>("key7"));

            CacheManager.Instance.CreateOrGetCache("MUlkeler");
            CacheManager.Instance.CreateOrGetCache("MUlkeler", new CacheProperties() { CacheCollectionType = CacheCollectionType.MemoryCache, Capacity = 100, CacheItemExpireDuration = 1, IsSlidingExpiration = true, CacheItemExpireFunction = ExpireFuncForCacheItem }, myDictionary);

            CacheManager.Instance.CreateCache("TestCache101", new CacheProperties() { CacheCollectionType = CacheCollectionType.MemoryCache, CacheExpireDuration = 1, CacheExpireFunction = ReloadCache });
            CacheManager.Instance.GetCache("TestCache101").Add("test10Key", "test10Value");
            CacheManager.Instance.GetCache("TestCache101").Add("test11Key", "test11Value");
            Console.WriteLine(CacheManager.Instance.GetCache("TestCache101").GetValue<string>("test10Key"));
            // Cache te veri islemleri CRUD

            //Create
            CacheManager.Instance.GetCache("MUlkeler").Add("tr", "Turkiye");
            CacheManager.Instance.GetCache("MUlkeler").AddAll(myDictionary);

            //Read
            object myObj = CacheManager.Instance.GetCache("MUlkeler").GetValue<string>("tr");
            IList<string> ls1 = CacheManager.Instance.GetCache("MUlkeler").GetValues<string>(new List<string>() { "tr", "en" });

            ////Delete
            CacheManager.Instance.GetCache("MUlkeler").Remove("tr");
            CacheManager.Instance.GetCache("MUlkeler").RemoveKeys(new List<string>() { "tr", "en" });
            CacheManager.Instance.GetCache("MUlkeler").Clear();

        }

        public static void ExampleGetValue()
        {
            Dictionary<string, object> exampleDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                exampleDictionary.Add("key" + i, "value" + i);


            ICache ch1 = CacheManager.Instance.CreateCache("MyCache", null, exampleDictionary);
            Console.WriteLine("val:" + ch1.GetValue<string>("key100") + " retrieved from cache");

            object val2 = CacheManager.Instance.GetCache("MyCache").GetValue<string>("key102");
            Console.WriteLine("val:" + val2 + " retrieved from cache");

            string val3 = CacheManager.Instance.GetCache("MyCache").GetValue<string>("key103");
            Console.WriteLine("val:" + val3 + " retrieved from cache");
            
        }
        public static void ExampleModelUsage()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("username_" + i, CreateUser("username_" + i));


            ICache ch1 = CacheManager.Instance.CreateCache("MyCacheM1", null, myDictionary);
            Console.WriteLine("val:" + ch1.GetValue<UserDto>("username_100").UserName + " retrieved from cache");


        }
        public static void ExampleModelUsage2()
        {
            IList<UserDto> yourList = new List<UserDto>();
            for (int i = 0; i < 10000; i++)
            {
                yourList.Add(CreateUser("UserName_" + i));
            }

            ICache ch1 = CacheManager.Instance.CreateCache("MyCacheM2", null, yourList.ToDictionary(x => x.UserName, x => (object)x));

            UserDto user1 = ch1.GetValue<UserDto>("UserName_100");
            Console.WriteLine("val:" + user1.UserName + " retrieved from cache");

            UserDto user2 = ch1.GetValues<UserDto>().Where(a => a.UserName == "UserName_101").FirstOrDefault();
            Console.WriteLine("val:" + user2.UserName + " retrieved from cache");

            ICache ch2 = CacheManager.Instance.CreateCache("MyCacheM3", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache }, yourList.ToDictionary(x => x.UserName, x => (object)x));

            UserDto user3 = ch1.GetValue<UserDto>("UserName_100");
            Console.WriteLine("val:" + user3.UserName + " retrieved from cache");

            UserDto user4 = ch1.GetValues<UserDto>().Where(a => a.UserName == "UserName_101").FirstOrDefault();
            Console.WriteLine("val:" + user4.UserName + " retrieved from cache");

        }

        public static void ExampleGetCacheItems1()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("username_" + i, CreateUser("username_" + i));


            ICache ch1 = CacheManager.Instance.CreateCache("MyCacheM542", new CacheProperties(CacheCollectionType.DictionaryCache), myDictionary) ;

            string cacheJsonValue = ch1.DictionaryCache["username_101"].Value.ToString();

            UserDto user = GeneralUtil.FromJson<UserDto>(cacheJsonValue);

            Console.WriteLine("val:" + user.UserName + " retrieved from cache");


        }
        public static void ExampleGetCacheItems2()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("username_" + i, CreateUser("username_" + i));


            ICache ch1 = CacheManager.Instance.CreateCache("MyCacheM23", new CacheProperties(CacheCollectionType.MemoryCache), myDictionary);

            string cacheJsonValue = ch1.MemoryCache.Get("username_101").ToString();
            UserDto user = GeneralUtil.FromJson<UserDto>(cacheJsonValue);

            Console.WriteLine("val:" + user.UserName + " retrieved from cache");


        }
        public static void ExampleGetCacheItems3()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("username_" + i, CreateUser("username_" + i));


            ICache ch1 = CacheManager.Instance.CreateCache("MyCacheM21", new CacheProperties(CacheCollectionType.MemoryCache), myDictionary);
            System.Runtime.Caching.MemoryCache ch = ch1.CacheItems<System.Runtime.Caching.MemoryCache>();

            UserDto user = GeneralUtil.FromJson<UserDto>(ch.Get("username_101").ToString());
            Console.WriteLine("val:" + user.UserName + " retrieved from cache");

        }
        public static void ExampleGetCacheItems4()
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>();
            for (int i = 0; i < 10000; i++)
                myDictionary.Add("username_" + i, CreateUser("username_" + i));

            ICache ch1 = CacheManager.Instance.CreateCache("MyCache123", new CacheProperties(CacheCollectionType.DictionaryCache), myDictionary);
            ConcurrentDictionary<string, CacheItem> ch = ch1.CacheItems<ConcurrentDictionary<string, CacheItem>>();

            UserDto user = GeneralUtil.FromJson<UserDto>(ch["username_100"].Value.ToString());
            Console.WriteLine("val:" + user.UserName + " retrieved from cache");
        }



        public static void Example2()
        {
            Dictionary<string, string> exampleDictionary = new Dictionary<string, string>();
            for (int i = 0; i < 10000; i++)
                exampleDictionary.Add("key" + i, "value" + i);



            //usage
            ICache ch = CacheManager.Instance.CreateCache("myCache1");
            foreach (var item in exampleDictionary)
            {
                ch.Add(item.Key, item.Value);
            }

            for (int i = 0; i < 10000; i++)
            {
                Console.WriteLine(ch.GetValue<string>("key" + i) + " retrieved from cache");
            }

        }
        public static void Example2Performans()
        {
            ThreadPoolHandler tpHandle1 = ThreadPoolManager.Instance.CreatePool("Pool-1", 100, CacheManagerPutData);
            for (int i = 0; i < 10000; i++) tpHandle1.addJob(CreateUser("Username_"+ i));

            ThreadPoolHandler tpHandle2 = ThreadPoolManager.Instance.CreatePool("Pool-2", 100, CacheManagerTakeData);
            for (int i = 0; i < 10000; i++) tpHandle2.addJob(CreateUser("Username_" + i));

            tpHandle1.Start();
            tpHandle2.Start();
        }
        private static void CacheManagerPutData(object _job)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            ICache ch1 = CacheManager.Instance.CreateOrGetCache("Cache1", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache });
            watch1.Stop();
            Console.WriteLine("get dictionaryContainer elapsed:{0}ms", watch1.ElapsedMilliseconds);

            JobData jobData = _job as JobData;


            Stopwatch watch = new Stopwatch();
            watch.Start();

            UserDto user = jobData.Job as UserDto;
            ch1.Add(user.UserName, jobData);

            watch.Stop();
            Console.WriteLine("Poolname:{0}, Thread Number:{1}, Add:{2}, Sure:{3}", jobData.PoolName, jobData.ThreadInfo.ThreadNumber, user.UserName, watch.ElapsedMilliseconds);

        }
        private static void CacheManagerTakeData(object _job)
        {
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            ICache ch1 = CacheManager.Instance.CreateOrGetCache("Cache1", new CacheProperties() { CacheCollectionType = CacheCollectionType.DictionaryCache });
            watch1.Stop();
            Console.WriteLine("get dictionaryContainer elapsed:{0}ms", watch1.ElapsedMilliseconds);

            JobData jobData = _job as JobData;


            Stopwatch watch = new Stopwatch();
            watch.Start();

            UserDto user = jobData.Job as UserDto;
            JobData res = null;
            if (ch1.ContainsKey(user.UserName))
            {
                res = ch1.GetValue<JobData>(user.UserName);
            }

            watch.Stop();
            Console.WriteLine("Poolname:{0}, Thread Number:{1}, getValue:{2}, res:{4} Sure:{3}", jobData.PoolName, jobData.ThreadInfo.ThreadNumber, jobData.Job.ToString(), watch.ElapsedMilliseconds, res != null ? res.Job.ToString() : "bos");

        }
        public static bool ExpireFuncForCacheItem(CacheItem item)
        {
            return true;
        }
        public static bool ReloadCache(ICache item)
        {
            Console.WriteLine("Checking reloatind");
            //reload process
            item.Add("key1", "value1");
            item.Add("key2", "value2");
            item.Add("key3", "value3");
            item.Add("key4", "value4");
            item.Add("key5", "value5");
            return true;
        }

        private static UserDto CreateUser(string username)
        {
            return new UserDto()
            {
                UserName = username,
                ActivationCode = "SKJDHFA4359324L2K3J4LKJHRSD",
                UpdatedBy = 3,
                DtLastLogin = DateTime.Now,
                DtUpdated = DateTime.Now,
                IsActive = true,
                DtCreated = DateTime.Now,
                IsUserActive = true,
                OldPassword = "testpassword",
                CreatedBy = 3,
                Password = "9876SAD98F7ASD98F7A9DS7FA98DS7F98SADF",
                PasswordTry = 3,
                Pk = 0,
                Theme = "Silver",
                TotalLogin = 3
            };
        }
    }
}

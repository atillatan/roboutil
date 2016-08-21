using RoboUtil.Common;
using RoboUtil.managers;
using RoboUtil.utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboUtil.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPoolManagerExample.Example1();
            //ThreadPoolManagerExample.Example2();

            //ThradPoolExample.Example1();

            //CacheManagerExample.ExampleCreateCache1();
            //CacheManagerExample.ExampleCreateCache2();
            //CacheManagerExample.ExampleGetValue();
            //CacheManagerExample.ExampleModelUsage();
            //CacheManagerExample.ExampleModelUsage2();
            //CacheManagerExample.ExampleGetCacheItems1();
            //CacheManagerExample.ExampleGetCacheItems2();
            //CacheManagerExample.ExampleGetCacheItems3();
            //CacheManagerExample.ExampleGetCacheItems4();
            //CacheManagerExample.Example2Performans();

            //UserDto dto1 = new UserDto() { UserName="atilla"};
            //UserDto dto2 = dto1.Copy<UserDto


          
            SqlConnection Connection = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=EXA;Integrated Security=True;Pooling=True");

            using (SqlConnection readConnection = new SqlConnection(@"Data Source=.\SQLEXPRESS; Initial Catalog=EXA;Integrated Security=True;Pooling=True"))
            {
                dynamic x = DynamicDbUtil.List(readConnection, "select Id,StringVar,IntVar,DateTimeVar from Example");
                dynamic x21 = DynamicDbUtil.List(readConnection, "select Id,StringVar,IntVar,DateTimeVar from Example where Id={0}",1);

                UserDto y = ExpandoObjectMapperUtil.Map<UserDto>(x);

                UserDto xy = DynamicDbUtil.Get<UserDto>(readConnection, @"
                SELECT 
                       Ad [Adi]
                      ,Soyad [Soyadi]
                      ,KullaniciAd [UserName]
                      ,Sifre [Password]
                      ,kd.AnneAdi [KullaniciDetay.AnneAdi]                   
                  FROM campus.dbo.Kullanici k
                  join dbo.KullaniciDetay kd on k.No=kd.No
                  where k.No={0}
                ", 124);


                //TODO: Mapper will change
                //dynamic x1 = DatabaseUtil.Get(readConnection, @"
                //SELECT 
                //       Ad [Adi]
                //      ,Soyad [Soyadi]
                //      ,KullaniciAd [UserName]
                //      ,Sifre [Password]
                //      ,kd.AnneAdi [KullaniciDetay.AnneAdi]                   
                //  FROM campus.dbo.Kullanici k
                //  join dbo.KullaniciDetay kd on k.No=kd.No
                //  where k.No={0}
                //", 124);


                UserDto y1 = ExpandoObjectMapperUtil.Map<UserDto>(x);

                //TODO: Mapper will change
                //UserDto x2 = DatabaseUtil.Get<UserDto>(readConnection, @"
                //SELECT                     
                //      KullaniciAd [UserName]
                //      ,Sifre [Password]
                //      ,1 [OldPassword]   
                //      ,getdate() [DtLastLogin]          
                //      ,2 [TotalLogin]       
                //      ,getdate() [DtLastLogin]        
                //      ,1 [PasswordTry]     
                //      ,1 [ActivationCode]     
                //      ,1 [Theme]         
                //      ,1 [IsUserActive]
                //     ,No [Id]
                //     ,getdate() [DtCreated] 
                //     ,1 [CreatedBy] 
                //     ,getdate() [DtUpdated] 
                //     ,1 [UpdatedBy] 
                //     ,1 [IsActive]  
                //  FROM campus.dbo.Kullanici k               
                //  where k.No={0}
                //", 124);


                //TODO: Mapper will change
                //List<UserDto> x3 = DatabaseUtil.List<UserDto>(readConnection, @"
                //SELECT                     
                //      KullaniciAd [UserName]
                //      ,Sifre [Password]
                //      ,1 [OldPassword]   
                //      ,getdate() [DtLastLogin]          
                //      ,2 [TotalLogin]       
                //      ,getdate() [DtLastLogin]        
                //      ,1 [PasswordTry]     
                //      ,1 [ActivationCode]     
                //      ,1 [Theme]         
                //      ,1 [IsUserActive]
                //     ,No [Id]
                //     ,getdate() [DtCreated] 
                //     ,1 [CreatedBy] 
                //     ,getdate() [DtUpdated] 
                //     ,1 [UpdatedBy] 
                //     ,1 [IsActive]  
                //  FROM campus.dbo.Kullanici k               
                //  where k.No<{0}
                //", 124);
            }


            //GeneralUtil.Clone<string>("asdfadsf");



            Console.ReadKey();
        }
    }
}

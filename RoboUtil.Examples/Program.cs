using RoboUtil.dto;
using RoboUtil.managers;
using RoboUtil.utils;
using RoboUtil.Utils;
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
            //ThreadPoolManagerExample.Example1();
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

            
            SqlConnection Connection = new SqlConnection("Data Source=80.251.41.233;Initial Catalog=campus;Persist Security Info=True;User ID=sa;Password=Au2014*-;MultipleActiveResultSets=True;");

            using (SqlConnection readConnection = new SqlConnection("Data Source=80.251.41.233;Initial Catalog=campus;Persist Security Info=True;User ID=sa;Password=Au2014*-;MultipleActiveResultSets=True;"))
            {



                dynamic x = DynamicDbUtil.Get(readConnection, @"
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


                UserDto y = ExpandoObjectMapper.Map<UserDto>(x);

                 


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



                dynamic x1 = DatabaseUtil.Get(readConnection, @"
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


                UserDto y1 = ExpandoObjectMapper.Map<UserDto>(x);


                UserDto x2 = DatabaseUtil.Get<UserDto>(readConnection, @"
            SELECT                     
                  KullaniciAd [UserName]
                  ,Sifre [Password]
                  ,1 [OldPassword]   
                  ,getdate() [DtLastLogin]          
                  ,2 [TotalLogin]       
                  ,getdate() [DtLastLogin]        
                  ,1 [PasswordTry]     
                  ,1 [ActivationCode]     
                  ,1 [Theme]         
                  ,1 [IsUserActive]
                 ,No [Id]
                 ,getdate() [DtCreated] 
                 ,1 [CreatedBy] 
                 ,getdate() [DtUpdated] 
                 ,1 [UpdatedBy] 
                 ,1 [IsActive]  
              FROM campus.dbo.Kullanici k               
              where k.No={0}
            ", 124);

                List<UserDto> x3 = DatabaseUtil.List<UserDto>(readConnection, @"
            SELECT                     
                  KullaniciAd [UserName]
                  ,Sifre [Password]
                  ,1 [OldPassword]   
                  ,getdate() [DtLastLogin]          
                  ,2 [TotalLogin]       
                  ,getdate() [DtLastLogin]        
                  ,1 [PasswordTry]     
                  ,1 [ActivationCode]     
                  ,1 [Theme]         
                  ,1 [IsUserActive]
                 ,No [Id]
                 ,getdate() [DtCreated] 
                 ,1 [CreatedBy] 
                 ,getdate() [DtUpdated] 
                 ,1 [UpdatedBy] 
                 ,1 [IsActive]  
              FROM campus.dbo.Kullanici k               
              where k.No<{0}
            ", 124);
            }


            //GeneralUtil.Clone<string>("asdfadsf");



            Console.ReadKey();
        }
    }
}

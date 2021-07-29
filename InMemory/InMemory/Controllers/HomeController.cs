using InMemory.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InMemory.Controllers
{
    public class HomeController : Controller
    {
        //Cache kullanımı için ekledik
        private readonly IMemoryCache _memoryCache;

        public HomeController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            //Cache tarafınde key e ait data var mı diye kontrol ediyoruz
            //out parametresi sayesinde zamanCache parametremize cache de ki değerimiz atanacaktır
            if (!_memoryCache.TryGetValue<string>("zaman", out string zamanCache))
            {

                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                //Oluşturulan Cache nin ne kadar süre sonra sona ereceğini kararlaştıran yapıdır
                options.AbsoluteExpiration = DateTime.Now.AddSeconds(20);


                //10 Sn içerisinde data ya ulaşırsak sayaç sıfırlanacaktır.
                //10 sn içinde erişmezsek data silinecektir.
                //Absolute ile kullanılması tavsiye edilir.
                options.SlidingExpiration = TimeSpan.FromSeconds(10);

                //Dataları önem sırasına göre etiketleyebiliyoruz.
                //Net Core bellek taşmasını önlemek için silme işlemi yapacağı dataya buraya göre karar verecektir.Mümkün mertebe NeverRemove kullanmamaya çalışmak bizler için iyi olur.
                options.Priority = CacheItemPriority.High;

                //Bizden 4 parametreli delege bekliyor,metodu bu şekilde de ekleyebilirim
                //Cache in neden silindiği bilgisine ulaşmamızı sağlar
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set<string>("callback", $"{key}-{value}-{reason}-{state}");
                });

                //Generic olarak içerisinde tutacağımız datayı belirliyoruz ve ona erişebilmek için key ataması yapıyoruz.
                //Cache te data tutmak demek sunucunun ram belleğinde data tutmak demektir.
                //Cache süresini options olarak belirtip parametre olarak tanımladık
                //Var olan aynı key ile data set edersek üzerine yazacaktır
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options);
            }


            return RedirectToAction("Privacy");
        }

        public IActionResult Privacy()
        {
            //belirlenen key e ait data varsa getiriyor yok ise oluşturup geri dönüyor
            _memoryCache.GetOrCreate<string>("zaman", entry =>
             {
                 return DateTime.Now.ToString();
             });

            //atadığımız key e ait datayı alıyoruz
            var zaman = _memoryCache.Get<string>("zaman");

            //Key e ait data yı silebiliriz.
            _memoryCache.Remove("zaman");

            //Cache in neden silindiği bilgisine bu şekilde erişebiliriz.
            //Index Action unda zaman isim cache bilgisinin neden silindiği bilgisini alıyoruz.
            ViewBag.CallBack = _memoryCache.Get<string>("callback");

            //String değişken olduğu için View ismi olarak algılıyor.Bundan kaynaklı objeye çevirdik
            return View((object)zaman);
        }



        public IActionResult ComplexType()
        {
            var product = new Product { Id = 1, Name = "Samsung", Price = 5000 };

            //Set metodu value type olarak obje kabul edebilir.Product objemizi verdik
            _memoryCache.Set<Product>("productKey", product);

            //Cast İşlemi otomatik olarak yapılır :)
            var productCache = _memoryCache.Get<Product>("productKey");

            return View(productCache);
        }


    }
}

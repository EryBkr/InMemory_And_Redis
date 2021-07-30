using Microsoft.AspNetCore.Mvc;
using RedisAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    public class HashTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _redisDb;

        //Liste ismimizi tutacağımız field
        private readonly string listKey = "hashnames";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;

            //4 numaralı dB alındı
            _redisDb = _redisService.GetDb(4);
        }

        public IActionResult Index()
        {
            //Listeye ekledik
            //Dictionary tipi gibi key value çifti olarak ekliyoruz
            _redisDb.HashSet(listKey, "id:1","berkay");


            //Listeden key değeri ile silme işlemi yapıyoruz
            _redisDb.HashDelete(listKey, "id:1");

            //Data setimiz için bir AbsoluteTime süresi verdik
            //Bu süre sonunda verilerimiz silinecektir
            //Bu absolute time özelliği bu metoda her uğrandığında sıfırlanacaktır
            //Bu durum sliding time gibi davranmasına neden olacaktır
            //_redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(80));

            //sliding time gibi davranmasını istemiyorsak böyle bir çözüm işimizi görecektir
            //if (!_redisDb.KeyExists(listKey))
            //_redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(80));


            return View();
        }

        public IActionResult Show()
        {
           //Redis HashSet veri tipi C# taki Dictionary gibi çalışır
            Dictionary<string,string> names = new Dictionary<string,string>();

            //Key e ait liste mevcut mu
            if (_redisDb.KeyExists(listKey))
            {
                //Rediste bulunan Set listemizden dataları ve keyleri alıp names listemize ekledik
                foreach (var item in _redisDb.HashGetAll(listKey).ToDictionary())
                {
                    names.Add(item.Key.ToString(), item.Value.ToString());
                }

                //Belli bir keye ait bir data yı da alabiliriz.
                //var data = _redisDb.HashGet("","key");
            }
            return View(names);
        }
    }
}

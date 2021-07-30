using Microsoft.AspNetCore.Mvc;
using RedisAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    public class SortedSetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _redisDb;

        //Liste ismimizi tutacağımız field
        private readonly string listKey = "sortedsetnames";

        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;

            //3 numaralı dB alındı
            _redisDb = _redisService.GetDb(3);
        }

        public IActionResult Index()
        {
            //Listeye score değeriyle ile birlikte ekledik
            //Score değeri listemizin sıralamasını belirleyecektir
            _redisDb.SortedSetAdd(listKey, "berkay",10);
            _redisDb.SortedSetAdd(listKey, "eray", 9);

            //Silme işlemi
            _redisDb.SortedSetRemove(listKey,"eray");

            //30 sn lik absolute time tanımladık
            //Data setimiz için bir AbsoluteTime süresi verdik
            //Bu süre sonunda verilerimiz silinecektir
            //Bu absolute time özelliği bu metoda her uğrandığında sıfırlanacaktır
            //Bu durum sliding time gibi davranmasına neden olacaktır
            _redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(30));

            //sliding time gibi davranmasını istemiyorsak böyle bir çözüm işimizi görecektir
            //if (!_redisDb.KeyExists(listKey))
            //_redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(80));

            return View();
        }

        public IActionResult Show()
        {
            
            List<string> names = new List<string>();

            //Key e ait liste mevcut mu
            if (_redisDb.KeyExists(listKey))
            {
                //Rediste bulunan Sorted Set listemizden dataları alıp names listemize ekledik
                //_redisDb.SortedSetScan(listKey).ToList().ForEach(value=> 
                //{
                //    names.Add(value.ToString());
                //});

                //Score değerine göre tersten sıralamış olduk
                //Ayrıca Take metodu gibi kaç adet data alacağımızı belirtebiliyoruz tabi bu metoda özgü birşey değil
                _redisDb.SortedSetRangeByRank(listKey,0,3, order: Order.Descending).ToList().ForEach(value => 
                {
                    names.Add(value.ToString());
                });
            }

            return View(names);
        }
    }
}

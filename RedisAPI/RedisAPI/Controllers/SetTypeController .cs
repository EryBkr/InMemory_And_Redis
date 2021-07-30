using Microsoft.AspNetCore.Mvc;
using RedisAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    public class SetTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _redisDb;

        //Liste ismimizi tutacağımız field
        private readonly string listKey = "setnames";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;

            //2 numaralı dB alındı
            _redisDb = _redisService.GetDb(2);
        }

        public IActionResult Index()
        {
            //Listeye ekledik
            _redisDb.SetAdd(listKey, "berkay");
            _redisDb.SetAdd(listKey, "eray");

            //Listeden eray isimli kaydı siliyoruz
            _redisDb.SetRemove(listKey, "eray");

            //Data setimiz için bir AbsoluteTime süresi verdik
            //Bu süre sonunda verilerimiz silinecektir
            //Bu absolute time özelliği bu metoda her uğrandığında sıfırlanacaktır
            //Bu durum sliding time gibi davranmasına neden olacaktır
            _redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(80));

            //sliding time gibi davranmasını istemiyorsak böyle bir çözüm işimizi görecektir
            //if (!_redisDb.KeyExists(listKey))
            //_redisDb.KeyExpire(listKey, DateTime.Now.AddSeconds(80));


            return View();
        }

        public IActionResult Show()
        {
            //içerisinde tutacağı değerler unique 'tir ve sırasız bir şekilde ekler
            //Bunu kullanma zorunluluğumuz yok aslında
            HashSet<string> names = new HashSet<string>();

            //Key e ait liste mevcut mu
            if (_redisDb.KeyExists(listKey))
            {
                //Rediste bulunan Set listemizden dataları alıp names listemize ekledik
                _redisDb.SetMembers(listKey).ToList().ForEach(value=> 
                {
                    names.Add(value);
                });
            }

            return View(names);
        }
    }
}

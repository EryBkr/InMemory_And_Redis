using Microsoft.AspNetCore.Mvc;
using RedisAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    public class ListTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _redisDb;

        //Liste ismimizi tutacağımız field
        private readonly string listKey = "names";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;

            //1 numaralı dB alındı
            _redisDb = _redisService.GetDb(1);
        }

        public async Task<IActionResult> Index()
        {
            //Listenin sonuna ekleme
            _redisDb.ListRightPush(listKey, "berkay");
            _redisDb.ListRightPush(listKey, "test");
            _redisDb.ListLeftPush(listKey,"başa eklendi");

            //listemizden  test isimli datayı siliyoruz
            await _redisDb.ListRemoveAsync(listKey, "test");

            //Listenin başından datayı siler
            _redisDb.ListLeftPop(listKey);

            //Listenin sonundan siler
            _redisDb.ListRightPop(listKey);

            return View();
        }

        public IActionResult Show()
        {
            List<string> names = new List<string>();

            //Key e ait liste mevcut mu
            if (_redisDb.KeyExists(listKey))
            {
                //Listede ki bütün dataları alıyoruz ve listemize ekliyoruz
                _redisDb.ListRange(listKey, 0, -1).ToList().ForEach(value=> 
                {
                    names.Add(value.ToString());
                });
            }

            return View(names);
        }
    }
}

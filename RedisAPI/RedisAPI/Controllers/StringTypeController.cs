using Microsoft.AspNetCore.Mvc;
using RedisAPI.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase _redisDb;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;

            //0 numaralı dB alındı
            _redisDb = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            //Redis String Data Add
            _redisDb.StringSet("name", "eray");
            _redisDb.StringSet("client", "blackerback");
            _redisDb.StringSet("count", 1000);

            return View();
        }

        public async Task<IActionResult> Show()
        {
            //Redis veritabanımızdan key e ait datayı aldık.
            RedisValue value = _redisDb.StringGet("name");

            //Count değerimizi 1 arttırdık
            _redisDb.StringIncrement("count");

            //Count değerimizi asenkron olarak 3 azalttık
            await _redisDb.StringDecrementAsync("count",3);

            //Datanın belli bir kısmını aldık
            RedisValue client = _redisDb.StringGetRange("client", 0, 3);

            RedisValue clientLength = _redisDb.StringLength("client");

            //Value dolu mu
            if (value.HasValue)
            {
                ViewBag.Value = value.ToString();
                ViewBag.Count = _redisDb.StringGet("count");
                ViewBag.Client = client;
                ViewBag.ClientLength = clientLength;
            }
            else
            {
                ViewBag.Value = "Boş geldi";
            }

            return View();
        }
    }
}

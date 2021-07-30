using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisAPI.Services
{
    public class RedisService
    {
        private readonly string _redisHost;
        private readonly string _redisPort;

        //Redis server  haberleşme
        private ConnectionMultiplexer _redisServer;

        //Redis StacExchange
        public IDatabase _db { get; set; }

        //appsettings.json dosyamız daki redis serverin host ve portlarına eriştik
        public RedisService(IConfiguration configuration)
        {
            _redisHost= configuration["Redis:Host"];
            _redisPort = configuration["Redis:Port"];
        }

        //Redis Bağlantısı Açılıyor
        public void Connect()
        {
            //Server bilgilerini aldık exp:localhost:6380
            var configString = $"{_redisHost}:{_redisPort}";

            //redis server a bağlandık
            _redisServer = ConnectionMultiplexer.Connect(configString);
        }


        //Rediste databaseler numaralar şeklinde tutulur.Hangi db ye erişmek istediğimize karar verip ona bağlanıyoruz
        public IDatabase GetDb(int dB)
        {
            return _redisServer.GetDatabase(dB);
        }
    }
}

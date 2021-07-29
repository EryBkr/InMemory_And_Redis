# Redis CLI Komutları
### Ortamın Kurulması

```ruby
`docker run --name some-redis -d -p 6380:6379 redis redis-server --appendonly yes` 
komutuyla redis container ı docker ortamında 6380 portunda çalıştırıyoruz
```

```ruby
`docker exec -it [container id] sh` 
komutuyla redis arayüzüne geçiş yapıyoruz
```
### Redis String
```ruby
`set [key] [value]` 
ile kayıt işlemi yapıyoruz.key ve value tip bağımsızdır
```

```ruby
`get [key]` 
ile belirlediğimiz key e ait kaydı çağırabiliriz.
```

```ruby
`getrange  [key] 0 3` 
ile belirlediğimiz key e ait kaydın belirlenen indislerini  çağırabiliriz.
```

```ruby
`incr [key]` 
key e atanmış integer bir datanın değerini 1 arttırabiliriz
```

```ruby
`incrby  [key] [integer value]` 
key e atanmış integer bir datanın değerini value kadar arttırabiliriz
```

```ruby
`decr  [key]` 
key e atanmış integer bir datanın değerini 1 azaltabiliriz
```

```ruby
`decrby   [key] [integer value]` 
key e atanmış integer bir datanın değerini value kadar azaltabiliriz
```

```ruby
`append    [key] [value]` 
key e atanmış  bir dataya string ekleme işlemi yapabiliriz
```
### Redis List
```ruby
`lpush  [array name] [value]` 
Listenin başına değer ekledik
```

```ruby
`rpush   [array name] [value]` 
Listenin sonuna değer ekledik
```

```ruby
`lrange    [array name] 0 -1` 
Listenin tamamını ekrana bastırdık.{-1} yerine başka bir değer kullansaydık istediğimiz sayıda datayı da alabilirdik
```

```ruby
`lpop   [array name]` 
Listenin başından eleman çıkardık
```

```ruby
`rpop    [array name]` 
Listenin snonudan eleman çıkardık
```

```ruby
`lindex     [array name] [index]` 
Listeden belirli index e sahip datayı aldık
```
### Redis Set (Set veri tipinde datalar rastgele eklenir sıra sistemi yoktur)
```ruby
`sadd [array name] [value]` 
Listeye eleman ekledik.
```

```ruby
`smembers  [array name]` 
Listede ki elemanları çağırdık
```
```ruby
`srem   [array name] [value]` 
Listeden verilen değer çıkartılır
```

### Redis SortedList (Score numarasına göre sıralanır)
```ruby
`zadd    [array name] [scroe number] [value]` 
Listeye eleman score numarasına göre eklenir.Küçükten büyüğe doğru sıralanır
```

```ruby
`zrange  [array name] [score number]  0 -1` 
Listedeki elemanlar listelenir `withscores` parametresi eklenebilir
```

```ruby
`zrem   [array name] [value]` 
Listeden eleman çıkartılır 
```

```ruby
`zrem   [array name] [value]` 
Listeden eleman çıkartılır 
```

### Redis Hash (C# taki Dictionary veri seti gibi çalışır)

```ruby
`hmset [dictionary name] [key] [value]` 
Lieteye key value pair eklenir
```

```ruby
`hget  [dictionary name] [key]` 
Lieteden key ile eleman çağırılır
```


```ruby
`hdel   [dictionary name] [key]` 
Lieteden belirlenen key e ait veri silinir
```

```ruby
`hgetall   [dictionary name]` 
Lietedeki elemanlar çağırılır
```

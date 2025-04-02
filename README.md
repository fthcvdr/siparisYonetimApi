API Kullanımı:
Yeni Sipariş Ekleme: POST /api/order/create
Request Body: Sipariş bilgileri ve sipariş kalemleri.
Bu işlem stok kontrolü yapar ve eğer yeterli stok yoksa hata döner.
Siparişleri Listeleme: GET /api/order/list/{userId}
Belirtilen kullanıcıya ait tüm siparişler listelenir.
Sipariş Detayını Getirme: GET /api/order/detail/{id}
Belirtilen sipariş ID’sine ait detaylar döndürülür.
Sipariş Silme: DELETE /api/order/delete/{id}
Belirtilen sipariş ID’sine sahip sipariş veritabanından silinir.

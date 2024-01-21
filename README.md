# MASRAF ÖDEME SİSTEMİ

<br />

Proje için görsel ve açıklamaların bulunduğu pdf için aşağıdaki bağlantıya tıklayanız: <br />
[Final Case Projesi PDF Dosyası](https://drive.google.com/file/d/1TZI31tD0riKb7YSTc9zUuHk77Xi9Aw2O/view?usp=sharing)

<br />

## Proje Tanımı

Bir şirket özelinde sahada çalışan personeli için masraf kalemlerinin takibi ve yönetimi için bir uygulama talep edilmektedir. Bu uygulama ile saha da çalışan personel masraflarını anında sisteme girebilecek ve işveren bunu aynı zamanda hem takip edip edebilecek hem de vakit kaybetmeden harcamayı onaylayıp personele ödemesini yapabilecektir. Çalışan hem evrak fiş vb. toplamaktan kurtulmuş olacak hem de uzun süre sahada olduğu durumda gecikmeden ödemesini alabilecektir.

<br />

## Projeyi Çalıştırma
Projeyi kendi bilgisayarınıza klonlayarak veya indirdiğiniz .zip uzantılı dosyadan çıkararak doğrudan çalıştırabilirsiniz. Herhangi bir migration komutu çalıştırmanıza gerek yoktur.

<br />

## Tech

Projede kullanılan teknolojiler aşağıda listelenmiştir.

- Veri tabanı olarak SQL Server
- Kimlik doğrulama ve yetkilendirme için [JWT](https://jwt.io/) token ve [ASP.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0&tabs=visual-studio) kütüphanesi
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- Repository ve Unit Of Work Design Pattern
- Raporlar için [Dapper](https://github.com/DapperLib/Dapper)

<br />

## Veri Tabanı Diyagramları

### Kullanıcı tabloları

![image](https://github.com/mustafakarasu/book-store-sln/assets/15230250/cb8b635b-67b7-4bfc-9a7a-a2953d0f0f57)

<br>

------------

### Proje için kullanılan tablolar

![image](https://github.com/mustafakarasu/book-store-sln/assets/15230250/70d4b427-746f-4f09-889f-e459a413dab0)

<br />

## Default Admin Bilgileri

|                |     Email Adresi                |     Şifre                |
|----------------|---------------------------------|--------------------------|
|     Admin 1    |     mustafa.karasu@admin.com    |     Patika2024Mustafa    |
|     Admin 2    |     karasu.mustafa@admin.com    |     Akbank2024Mustafa    |

<br />

## Sisteme Giriş
Sisteme giriş için **email** ve **şifre** ile giriş yapılabilir.

Kullanıcılar sisteme giriş yapabilmek için aşağıdaki endpoint adresini kullanılır:

|     Http Method    |     Path                         |
|--------------------|----------------------------------|
|     `POST`           |    ` /api/authentication/login`    |

<br />

## Personel Ekleme

**Admin** rolündeki kullanıcılar personel bilgilerini kendileri girecek ve ekleyecektir. Bu nedenle sisteme personel kendini kayıt edemez.
>NOT: Testleri kolaylaştırmak için personel şifresini kayıt ederken verilir ve sisteme bu şifre ile Employee rolünden giriş yapılabilir.

<br />
Admin personel kayıt edebilmek için aşağıdaki endpoint adresini kullanır:

|     Http Method    |     Path                            |
|--------------------|-------------------------------------|
|     `POST`           |     `/api/authentication/employee`    |

>NOT: Test için şifre kuralı basit tutulmuştur. Sadece en az 6 karakter kuralı vardır.

<br>

### Personel oluştururken uyulması gereken için kurallar:

*FirstName*, *LastName*, *Email*, *Password* alanları boş geçilemez. *PhoneNumber* alanı ise isteğe bağlıdır.

<br />

## Masraflar için kategori işlemleri

Masraf kategorisi işlemlerini sadece **Admin** rolündeki kullanıcılar gerçekleştirebilir.

Kategoriler için aşağıdaki enpoint adresleri kullanılır:

|     Method    |     Path                   |     Açıklama                                                                                                                 |
|---------------|----------------------------|------------------------------------------------------------------------------------------------------------------------------|
|     `GET`       |     `/api/categories `        |     Bütün kategorileri getirir.                                                                                              |
|     `POST`      |    ` /api/categories`         |     Yeni bir kategori   oluşturur. Tek kural Name   alanı gereklidir.                                                    |
|     `PUT`       |     `/api/categories/{id}`    |     Belirtilen id değerine göre kategori bilgilerini günceller.                                                            |
|     `DELETE `   |     `/api/categories/{id}`    |     Belirtilen id değerine   göre kategoriyi siler.     *NOT: Eğer bu kategori   ile masraf yapılmışsa kategori silenemez.*    |

<br />

## Masraflar için ödeme yöntemleri işlemleri

Yapılan harcamalardaki ödeme yöntemine ilişkin işlemler için sadece **Admin** rolündeki kullanıcılar işlemler yapabilir.

Ödeme yöntemleri için aşağıdaki endpoint adresleri kullanılır:


|     Method    |     Path                         |     Açıklama                                                                                                                                                                      |
|---------------|-----------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|     `GET`       |  `/api/payment-methods`  &nbsp; &nbsp; &nbsp; |     Bütün ödeme   yöntemleri getirir.                                                                                                                                             |
|     `GET`       |  `/api/payment-methods/{id}`  &nbsp; &nbsp; &nbsp; |     Belirtilen id değerine göre ödeme yöntemini   getirir.                                                                                                                        |
|     `POST`      |  `/api/payment-methods`  &nbsp; &nbsp; &nbsp; |     Yeni bir   ödeme yöntemi oluşturur.      Tek kural Name   alanı gereklidir.                                                                                                   |
|     `PUT`       |  `/api/payment-methods/{id}`  &nbsp; &nbsp; &nbsp; |     Belirtilen id değerine göre ödeme yöntemi   bilgilerini günceller.                                                                                                            |
|     `DELETE`    |  `/api/payment-methods/{id}`  &nbsp; &nbsp; &nbsp; |     Belirtilen   id değerine göre ödeme yöntemini siler.  <br/>NOT: Eğer bu ödeme   yöntemi ile masraf girişi yapılmışsa ödeme yöntemi ilgili masraflar   silinmeden silenemez.    |

<br />

## Masraf İşlemleri

Bir masrafı sadece **Employee** rolündeki kullanıcı oluşturabilir.

Masraf işlemleri için aşağıdaki endpoint adresleri kullanılır:

|     Method    |     Path                                   |     Açıklama                                                                                                                                                                                                                                                      |
|---------------|--------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|     `GET`       |     `/api/employees/expenses   `             |     Sadece istek yapan personelin masraf bilgilerini   getirir.                                                                                                                                                                                                   |
|     `GET`       |     `/api/employees/expenses/{expenseId}`    |     Personelin belirtilen id değerine göre   masraf bilgilerini getirir.                                                                                                                                                                                          |
|     `POST`      |     `/api/employees/expenses `               |     Personel gerekli masraf bilgilerini girerek ödeme için   masraf girişi yapabilir.                                                                                                                                                                             |
|     `PUT`       |    ` /api/employees/expenses/{expenseId}`    |     Masrafın id değerine göre masraf   bilgilerini tek bir istisna dışında ekleme kurallarında olduğu gibi   günceller. <br />    *NOT: Çok önemli bir kural olarak eğer Admin bu masraf için ödeme onayı veya   reddi yaptıysa artık masraf bilgileri güncellenemez.*    |
|     `DELETE`    |     `/api/employees/expenses/{expenseId} `   |     Masrafın id değerine göre masraf bilgilerini siler.  <br/>*NOT: Çok önemli bir kural olarak eğer Admin bu masraf   için ödeme onayı veya reddi yaptıysa masraf bilgileri silenemez. *                                                                            |
|     `GET`       |     `/api/employees/payments `               |     Personelin talep ettiği masraflar için   onay ve red alan bütün taleplerini listeleyebilir.                                                                                                                                                                   |
|     `GET`       |     `/api/employees/payments/{paymentId}`    |     Personelin ödeme veya red aldığı ödeme detayını   getirir.                                              |

<br />

### Masraf bilgileri alanları için önemli kurallar:

<br />

|     Özellik Adı        |     Açıklama                                                                                                                                                                                                                                                                                                                                                                                                 |
|------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|     `Amount`             |     Gereklidir. Yapılan masraf değeridir. Bütün   masraflar varsayılan Türk Lirası (TRY) döviz cinsi türünden yapılabilir.                                                                                                                                                                                                                                                                                   |
|     `Location`           |     Gereklidir. Harcama   yapılan konum bilgisidir. En fazla 45 karakter olabilir.                                                                                                                                                                                                                                                                                                                           |
|     `Description`        |     İsteğe bağlıdır. En fazla 450 karakter   olabilir.                                                                                                                                                                                                                                                                                                                                                       |
|     `CategoryId`         |     Yapılan harcama   için kategorinin Id değeridir. Eğer verilen CategoryId değeri database   üzerinde kayıtlı değilse kayıt işlemi gerçekleşmez.                                                                                                                                                                                                                                                           |
|     `PaymentMethodId`    |     Yapılan harcama için ödeme yönteminin Id   değeridir. Eğer verilen PaymentMethodId değeri database üzerinde kayıtlı   değilse kayıt işlemi gerçekleşmez.                                                                                                                                                                                                                                                 |
|     `Documents`          |     İsteğe bağlıdır. Personel   yaptığı harcama için makbuz, fiş, fatura gibi belge sunmak isterse bu kısımda   dosyaları yükleyebilir.      Kullanıcının   yüklediği dosyalar `“WebApi”` projesindeki `“Resources\Documents”`   dosya yolundaki klasöre dosya adı birlikte kaydedilir.  Veri tabanında ise `Documents`   tablosuna `FolderPath` alanına `“Resources\Documents\DosyaAdı”`   şeklinde kaydedilir.    |

<br />

## Masraflar için Ödeme İşlemleri

**Admin**, personel tarafından oluşturulan masraflar için onay veya açıklama ile birlikte red verebilir.

Masraf ödemeleri için aşağıdaki endpoint adresleri kullanılır:

|     Method    |     Path                         |     Açıklama                                                                                                                                                                      |
|---------------|----------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|     `GET`       |    ` /api/payments`                |     Bütün personelin ödeme işlemleri için yani   onay veya red olunan masraflar için ödeme detaylarını getirir.                                                                   |
|     `GET`       |     `/api/payments/{id}  `         |     Ödeme id değerine   göre ödeme bilgilerini getirir.                                                                                                                           |
|     `POST`      |     `/api/payments/{expenseId} `   |     Verilen masraf id değerine göre personel   tarafından oluşturulan masraf için onay veya red verebilecektir.                                                                   |
|     `DELETE`    |     `/api/payment-methods/{id} `   |     Belirtilen id   değerine göre ödeme yöntemini siler.  <br></br*NOT:>*NOT: Eğer bu   ödeme yöntemi ile masraf girişi yapılmışsa ödeme yöntemi ilgili masraflar   silinmeden silenemez. *   |

<br />

### Ödeme işlemleri için önemli noktalar

|     Özellik Adı    |     Açıklama                                                                                                                                                                                                                                                                   |
|--------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
|     `IsApproved`     |     `true` değeri onay verildiği   anlamına gelir ve personelin oluşturduğu masraf bilgisindeki harcama miktarı   para gönderimini simule etmek için database üzerindeki “`PaymentActivity`”   adlı tabloya kayıt edilir. Onay durumunda açıklama girilmek zorunda   değilidir.    |
|                    |     `false` değeri red   verildiği anlamında gelir. Bu durumda `“Description"` alanı mutlaka   girilmelidir. Red durumda para gönderimi olmayacağından `PaymentActivity`   tablosuna herhangi bir kayıt eklenmez.                                                                     |
|     `Description`    |     En fazla 450 karakter girilebilir. Onay durumda isteğe bağlı, red   durumda ise **mutlaka** girilmelidir.                                                                                                                                                                      |

<br />

## Admin için bütün masrafları getirme

**Admin** masrafları görüntüleyebilmek için aşağıdaki endpoint adresleri kullanılır:

|     Method    |     Path                  |     Açıklama                                                                     |
|---------------|---------------------------|----------------------------------------------------------------------------------|
|     `GET`       |     `/api/expenses`         |     Bütün personelin girmiş olduğu onay veya red   olunan masrafları getirir.    |
|     `GET`       |     `/api/expenses/{id}`    |     Belirtilen id   değerine göre masraf bilgilerini getirir.                    |

<br />

## Şirket ve Personel için Raporlar

**Şirket** için bizim projemizde onu temsil eden **Admin** rolü ve personeller onu temsil eden **Employee** rolü kullanılmıştır.

Raporlar için projemizde en son oluşturulan migration dosyasına eklenen stored procedure ve function'lar kullanılır. Veri tabanından raporları alabilmek için micro-ORM kütüphanesi **Dapper** kullanılmıştır.

Rapolar **günlük** (daily), **haftalık** (weekly), **aylık** (monthly) olarak endpoint adresinden alınabilir.

Rapor için dönen model olarak **PaymentDensity** nesnesi kullanılır. nesne bilgi olarak *Ödenen Miktar*, *Reddedilen Miktar*, *Beklyen Ödeme Miktarı* ve tarih aralıkları döndürülür.


------------



|     Method    |     Path                              |     Açıklama                                               |
|---------------|---------------------------------------|------------------------------------------------------------|
|     `GET`       |     `/api/reports/admin/daily `        |     Şirket için   günlük ödeme yoğunluğunu getirir.        |
|     `GET`       |     `/api/reports/admin/weekly`        |     Şirket için haftalık ödeme yoğunluğunu getirir.        |
|     `GET`       |     `/api/reports/admin/monthly`       |     Şirket için   aylık ödeme yoğunluğunu getirir.         |
|     `GET`       |     `/api/reports/employee/daily`      |     Personel için   günlük ödeme yoğunluğunu getirir.      |
|     `GET`       |     `/api/reports/employee/weekly`     |     Personel için haftalık ödeme yoğunluğunu   getirir.    |
|     `GET`       |    ` /api/reports/employee/monthly `   |     Personel için aylık   ödeme yoğunluğunu getirir.       |


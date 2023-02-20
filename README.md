# Block Game

## Proje Mimarisi

- ServerData; drive klasörümde bulunan json dosyasına web request yollayarak level bilgilerini çekmeye çalışıyor, başarılı olması durumunda aşağıdaki değerleri liste halinde alıyor ve böylece json dosyasında bulunan eleman başına level ekleyebiliyor ve bu levelleri değiştirebiliyoruz. Web Request’in başarılı olmadığı durumda ise default olarak oyuna eklenmiş level üzerinden devam ediyoruz.
    - piceAmount(Parça sayısı)
    - gridAmoumt(Oyun alanı büyüklüğü)
- GameManager; Start fonksiyonunda interface aracılığı ile referans aldığı class’ları çalıştırır ve ihtiyaç duydukları parametreleri alt class’lara iletir, class’ları kod’da belirtmiş olduğumuz önem sırasına göre sırayla çalıştırır ve bir kare bekler, şu anda önce gridHandler’ı çağırarak oyun alanını yaratıryor ardından ise  bunun dışında ise partGenerator’ı çağırarak parçaları yaratıyor ve hepsine gerekli referansları atıyor. Diğer işlevleri şunlardır;
    - Üretilen parçaları ekranda belirli değerler arasında rastgele seçilen bir konuma parçaları belirli bir sürede taşır.
    - Level değişimleri, levelin kaydedilmesi
    - Seçilen reklerin sıraya göre veya rastgele seçilerek iletilmesi
    - Oyunun bitip bitmediğinin kontrol edilmesi
- GridGenerator: Oyun alanının maksimum ve minimum alanlarını belirler ve oyun alanının ölçeğini ayarlar. GenerateDots metodu ile verdiğimiz oyun sahası ölçüsüne göre noktaların pozisyonlarını belli eden yeni objeleri sahneye yerleştirir ve x,y kordinatlarını iki boyutlu bir array’e eşitler, böylece noktaların pozisyonunu ve kordinatına erişimimiz olur.
- PartGenerator; Yeni bir parça **Voronoi diagram’ın** algoritması kullanılarak oluşturulur. Özel olarak açıklamak gerekirse belirlediğimiz boyutda bir 2D texture oluşturuyoruz, oyunumuzda 512x512 boyutunu kullanıyoruz. Bu 2D texture’ün üzerinde parça sayımız kadar rastgele noktalar belirliyoruz ve her bir parçaya bir renk atıyoruz. Her bir piksel sırayla dönüde oluşturluyor bir piksele ona hangi önceden belirlediğimiz nokta yakınsa onun rengini atıyoruz ve o pikseli ayrı bir listeye alıyoruz, döngünün sonunda elimizde parça sayısı uzunluğunda bir liste oluyor ve her bir listenin içinde ise aynı renge mensup pikseller bir array içinde bulunuyor. Elimizdeki Color arrayleri farklı texturelere yerleştirerek parça sayısı kadar texture elde ediyoruz, bu texture’leri kullanarak da sprite’lar oluşturuyoruz.  Elde etiğimiz parçaları yeni bir listeye alıyoruz ve bu listenin elemanlarında sırayla sahneye yerleştiriyoruz. Parça için bir parent transformu oluşturur, bu parent parçanın collider pozisyonunun ortasına yerleştirilir ve parçaya parent olarak atanır. Parçaların boyutu ve Hangi nokta ile temas ederse oyunun bitmeye uygun olacağı, isimi, sprite renderer değerlerinin alır, parçanın değerlerinin atamasını yapar ve parçayı haritanın üstünde rastgele bir konuma yerleştirir.
    - [Voronoi diagram](https://en.wikipedia.org/wiki/Voronoi_diagram) 
 - Game Pice: Parçanın input alarak haraket etmesi güncel durmunu, çarpışma testi ve konumunu kontrol eder. Eğer uygun şartlar sağlanıyor ise doğru noktaya snap mekaniği uygulayarak kendi pozisyonunu nokta ile eşitler. Sürüklenip bırakıldıktan sonra System linq komutları ile hangi parçanın en üstte duracağını karar veren yapıyı sağlar.
- InputManager: Mouse aracılığı ile raycast ve input işlemlerini gerçekleştirir. Hedef seçmemize de yarar.
- UI Manager: UI güncellemelerini yapar, buton işlevlerinin atamasını yapar.
- IInitializeable: Start fonksiyonunu aradan çıkarılıp verdiğimiz değere göre class’ların dorğu sırada çağrılması için oluşturduğum interface.
- MonoSingleton: Miras alma işlemi yaparak kullandığım Singleton pattern’i.

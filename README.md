# Dataedo - zadanie rekrutacyjne

## 1. Pomyślne przetworzenie wszystkich plików

### Źródło problemu

Błędy z przetworzeniem plików wynikały z różnic w strukturze nagłówków.

#### SampleFile1.csv

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/07952285-5203-435f-bb63-a39fe2b39780/017935d3-67d3-4d6e-bd62-d88f6ac063fe/Untitled.png)

#### SampleFile2.csv

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/07952285-5203-435f-bb63-a39fe2b39780/21b9732a-9907-4450-afc5-636fd63d588a/Untitled.png)

#### SampleFile3.csv

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/07952285-5203-435f-bb63-a39fe2b39780/483acaf2-4bcf-48e5-ac66-720600979c52/Untitled.png)

### Rozwiązanie problemu

Metoda **ExtractHeader** dodaje elementy do słownika **Header**, który przechowuje obiekty w postaci **<string, sbyte>**, gdzie **string** jest nazwą atrybutu np. **Name**, a **sbyte** jest indeksem tego atrybutu (kolejnością w jakiej występuje w pliku **.csv**).

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/07952285-5203-435f-bb63-a39fe2b39780/341245dc-6c54-417b-9e37-b4e2e3b4ac67/Untitled.png)

Metoda **GetValue**, na podstawie nazwy atrybutu pobiera jego indeks ze słownika **Header** i na podstawie indeksu pobiera odpowiednią wartość atrybutu. Dzięki temu import wartości staje się niezależny od kolejności atrybutów i odpowiadających im wartości w pliku.

![Untitled](https://prod-files-secure.s3.us-west-2.amazonaws.com/07952285-5203-435f-bb63-a39fe2b39780/1804066f-6609-46dc-87cd-e682421943c4/Untitled.png)

## 2. Poprawa wyświetlania obiektów

### Puste tytuły, opisy, linijki i dziwne kropki - źródło problemu

W przypadku braku tytułu, opisu, schema lub wartości jakiegokolwiek innego atrybutu program wypisywał pustą wartość. W efekcie po wypisaniu całej struktury pojawiały się puste linie (brak opisu), kropki na początku obiektu (brak schema) i puste nawiasy (brak tytułu).

### Rozwiązanie problemu

Została przygotowana osobna klasa - **Formatter** - której odpowiedzialnością jest tylko i wyłącznie odpowiednie sformatowanie konkretnego obiektu w strukturze. Klasa zawiera metody formatujące dla każdego elementu w hierarchii tj.:

1. Data Source
2. Group
3. Child
4. Sub Group
5. Sub Child
6. Description

Aktualna implementacja w przypadku braku wartości wypisuje informację o jej braku, natomiast wydzielenie formatowania pozwala na dowolne dostosowanie zachowania programu np. całkowite ignorowanie braku wartości lub logowanie informacji o braku wartości.

### Brak termów i areas - źródło problemu

Nie udało mi się poprawnie zidentyfikować źródła tego problemu. Nie jestem również pewien czy dostatecznie dobrze go rozumiem. Znalazłem jednak kilka potencjalnych (potencjalnych ze względu na to, że nie znam wszystkich założeń i być może jest to poprawne zachowanie programu) błędów, które mogą być powodem tego problemu.

1. Metoda odpowiedzialna za oczyszczanie usuwa spacje z nazw wieloczłonowych, przez co nie udaje się dopasować obiektu z pliku **dataSource.csv**. Usuwanie spacji zostało usunięte.

Photo

2. Podczas ładowania danych z pliku **dataSource.csv** do właściwości **ParentId** przypisywana jest wartość **1** w przypadku kiedy wartość ta w pliku jest pusta. Zamiast przypisywania wartości **1**, domyślnie będzie przypisywana wartość **default** czyli w przypadku typu **int** - 0.

Photo

3. Metoda **MatchAndUpdate** wykorzystuje **FirstOrDefault** do pobrania pasującego obiektu z pliku **dataSource.csv**. W niektórych sytuacjach w pliku **dataSource.csv** może jednak znajdować się więcej niż jeden obiekt z takimi samymi **Type**, **Name** i **Schema**.

Photo

Program został zmodyfikowany w taki sposób, aby pobierać wszystkie pasujące obiekty z pliku **dataSource.csv**. W przypadku kiedy zostanie znalezione więcej niż jedno dopasowanie, wybierane jest to, które posiada takiego samego rodzica jak importowany obiekt.

W przypadku kiedy nie uda się znaleźć dopasowania z takim samym rodzicem, wartości tytułu, opisu i custom fields nie są aktualizowane.

4. W plikach **sampleFileX.csv** znajdowały się obiekty o typie **GLOSSARY_ENTRY**. Obiektów o takim typie nie ma w pliku **dataSource.csv** przez co program nie znajduje dopasowania w metodzie **MatchAndUpdate**.

Photo

Photo

## 3. Ignorowanie błędów (logger)

### Źródło problemu

W przypadku braku dopasowania program przechodził do kolejnej iteracji bez jakiegokolwiek poinformowania użytkownika o pominiętym obiekcie.

### Rozwiązanie problemu

Został wykorzystany wbudowany logger który umożliwia logowanie do konsoli. Podczas pętli, która próbuje znaleźć dopasowania dla każdego importowanego obiektu, każde niepowodzenie jest zapisywane w globalnej kolekcji. Po wyjściu z pętli wykonywane jest logowanie wszystkich przechwyconych niepowodzeń z danymi importowanego obiektu.

Logger wykrywa trzy scenariusze:

1. Brak dopasowania - importowany obiekt nie ma swojego odpowiednika w pliku **dataSource.csv**,
2. Brak dopasowania rodzica - importowany obiekt ma swojego odpowiednika w pliku **dataSource.csv**, ale dane ich rodziców różnią się,
3. Wiele dopasowań - zostało znalezione wiele dopasowań dla importowanego pliku, ale żadne z nich nie posiada pasującego rodzica.

## 4. Performance i czytelność kodu

### Performance

W celu zwiększenia wydajności tam gdzie to możliwe zostały usunięte zbędne pętle, dzięki czemu redukujemy złożoność czasową wykonania się pętli.

W miejscach w których wykonywane są operacje na typie string, został wykorzystany StringBuilder.

W metodzie **MatchAndUpdate** została wykorzystana pętla **Paraller.ForEach**. Dzięki temu w przypadku większego zbioru danych uzyskujemy skrócenie czasu wykonywania. W przypadku mniejszych zbiorów danych skrócenie czasu jest stosunkowo niew

# Dataedo - zadanie rekrutacyjne

## 1. Pomyślne przetworzenie wszystkich plików

### Źródło problemu

Błędy z przetworzeniem plików wynikały z różnic w strukturze nagłówków.

#### SampleFile1.csv

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo1.png)

#### SampleFile2.csv

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo2.png)

#### SampleFile3.csv

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo3.png)

### Rozwiązanie problemu

Metoda **ExtractHeader** dodaje elementy do słownika **Header**, który przechowuje obiekty w postaci **<string, sbyte>**, gdzie **string** jest nazwą atrybutu np. **Name**, a **sbyte** jest indeksem tego atrybutu (kolejnością w jakiej występuje w pliku **.csv**).

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo4.png)

Metoda **GetValue**, na podstawie nazwy atrybutu pobiera jego indeks ze słownika **Header** i na podstawie indeksu pobiera odpowiednią wartość atrybutu. Dzięki temu import wartości staje się niezależny od kolejności atrybutów i odpowiadających im wartości w pliku.

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo5.png)

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

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo6.png)

2. Podczas ładowania danych z pliku **dataSource.csv** do właściwości **ParentId** przypisywana jest wartość **1** w przypadku kiedy wartość ta w pliku jest pusta. Zamiast przypisywania wartości **1**, domyślnie będzie przypisywana wartość **default** czyli w przypadku typu **int** - 0.

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo7.png)

3. Metoda **MatchAndUpdate** wykorzystuje **FirstOrDefault** do pobrania pasującego obiektu z pliku **dataSource.csv**. W niektórych sytuacjach w pliku **dataSource.csv** może jednak znajdować się więcej niż jeden obiekt z takimi samymi **Type**, **Name** i **Schema**.

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo8.png)

Program został zmodyfikowany w taki sposób, aby pobierać wszystkie pasujące obiekty z pliku **dataSource.csv**. W przypadku kiedy zostanie znalezione więcej niż jedno dopasowanie, wybierane jest to, które posiada takiego samego rodzica jak importowany obiekt.

W przypadku kiedy nie uda się znaleźć dopasowania z takim samym rodzicem, wartości tytułu, opisu i custom fields nie są aktualizowane.

4. W plikach **sampleFileX.csv** znajdowały się obiekty o typie **GLOSSARY_ENTRY**. Obiektów o takim typie nie ma w pliku **dataSource.csv** przez co program nie znajduje dopasowania w metodzie **MatchAndUpdate**.

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo9.png)

![Untitled](https://github.com/ArturPiatek/dataedo-recruitment-task/blob/main/ConsoleApp/StaticFiles/ExplanationImages/photo10.png)

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

W metodzie **MatchAndUpdate** została wykorzystana pętla **Paraller.ForEach**. Dzięki temu w przypadku większego zbioru danych uzyskujemy skrócenie czasu wykonywania. W przypadku mniejszych zbiorów danych skrócenie czasu jest stosunkowo niewielkie.

### Czytelność kodu

1. Główne metody zostały wyciągnięte do swoich własnych klas,
2. Część kodu odpowiedzialna za wypisywanie struktury w konsoli została rozbita na:
    1. Formatter - odpowiedzialny za sformatowanie obiektu i przygotowanie go do wypisania w konsoli,
    2. Write - odpowiedzialny za wypisanie obiektu w konsoli.
3. Klasy zostały pogrupowane i umieszczone w odpowiednich folderach.

## 5. Dodatkowe informacje

1. W przypadku pliku **sampleFile1.csv** logger wypisuje bardzo dużo obiektów, nie jestem pewny z czego to wynika, ale zakładam, że gdzieś popełniłem błąd, którego nie widzę. W takim wypadku potrzebowałbym więcej informacji o tym jak dokładnie ma działać algorytm i na takiej podstawie mógłbym spróbować znaleźć przyczynę problemu oraz zweryfikować czy także w przypadku pozostałych plików wszystko działa tak jak powinno.
2. Ze względu na ograniczenia logowania do konsoli nie są wypisywane wszystkie błędy w przypadku pliku **sampleFile1.csv**, natomiast infrastruktura do logowania jest już przygotowana. W przypadku konieczności logowania do pliku lub wykorzystania bardziej rozbudowanego systemu wystarczy wpiąć się w przygotowaną metodę odpowiednim providerem i wykonać logowanie.

# Currency Test Application

Aplikacja została stworzona z myślą o rekrutacji do firmy **AVENEO**. 
Stos technologiczny:

 - .NET Core 3.1
 - PostgresSQL 10 + Npgsql
 - Autofac
 - Automapper
 - EF Core
 
# Architektura
Architektura systemu jest klasyczną architekturą 3-warstową. Wybór takiej architektury spowodowany był charakterystyką problemu, który należało zaimplementować. Mieliśmy tutaj do czynienia z aplikacją, w której brak było encji, których stanem mogliśmy w jakiś sposób zarządzać (co wymusiłoby wykorzystanie jakiejś bardziej wyrafinowanej architektury). 
Zastosowany podział na warstwę danych i logiki biznesowej sprawia, że w dość łatwy i elastyczny sposób możliwe jest skorzystanie z różnych systemów baz danych oraz różnych API dostarczających informacje o kursach walut.

Składa się ona z 3 głównych komponentów:

 - Api: w którym obsługiwane są żądania użytkowników wraz z generowaniem oraz weryfikacją klucza API.
 - App: w nim znajduje się cała logika biznesowa, zaimplementowany jest mechanizm zbierania danych z zewnętrznych API oraz z lokalnych danych znajdujących się w cache,
 - Infrastructure: tutaj znajdują się funkcjonalności związane z dostępem do danych, zarówno do bazy danych jak również integracja do zewnętrznych serwisów, które udostępniają te dane.
 
Ponadto istnieje mniejsza biblioteka CurrencyApplication.Database, w których zawarte są mapowania encji, definicja DbContextu oraz odpowiednie konfiguracje połączenia z bazą danych. 

# Zewnętrzne API pobierające kursy walut

Skorzystałem z serwisu [https://exchangeratesapi.io/](https://exchangeratesapi.io/). Jest to api łatwe w obsłudze, zwracające dane w prostej do przetworzenia formie.
Aby móc skorzystać z implementacji innego modułu dostarczajcego dane z zewnętrznego API, należy zaimplementować interfejs **IExchangeRateProvider** a następnie zadeklarować jego użycie w kontenerze IOC. 

# Dostępne optymalizacje

Dane pobierane są z API dopiero w ostateczności. Aby przyspieszyć odpowiedź z serwera, zaimplementowane zostały następujące optymalizacje:

 - Zapisywanie pobranych wyników w bazie danych, przy kolejnej próbie odczytania wyniku, będzie on najpierw poszukiwany w bazie danych.
 - Cachowanie odpowiedzi na poziomie kontrolera ASP.NET. Jest ona usuwana z cache po upływie 1 minuty.  
 
 Dodanie (bądź deaktywacja) optymalizacji - takich jak np. przechowywanie danych w bazie danych w pamięci (np. Redis) jest dość elastyczne. Wystarczy jedynie zaimplementować interfejs **IFetchExchangeRateStep**, za pomocą property Order ustawić mu kolejność wykonania, oraz zarejestrować go w kontenerze. Dzięki temu dany krok wykona się w odpowiednim momencie.
 Obecna sekwencja operacji:
 
 - Pobranie danych z lokalnej bazy danych
 - Pobranie danych z API
 - Jeśli danych nie ma w API, to wykonujemy ponowną próbę, tylko tym razem o 3 dni więcej do tyłu, aby trafić ostatnią zdefiniowaną wartość.
 
 # Dostępne endpointy
 - **/api/Login/GetApiKey** - służąca do wygenerowania i pobrania ApiKey
 -  **/api/ExchangeRate/GetExchangeRates**- służąca do pobrania informacji o kursach walut w kontekście zakresu dat. Parametry takie jak w opisie zadania ;)

W razie wszelkich pytań/wątpliwości zapraszam do kontaktu, chętnie na wszystkie udzielę odpowiedzi.
 

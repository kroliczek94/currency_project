# Currency Test Application

Aplikacja zosta³a stworzona z myœl¹ o rekrutacji do firmy **AVENEO**. 
Stos technologiczny:

 - .NET Core 3.1
 - PostgresSQL 10 + Npgsql
 - Autofac
 - Automapper
 - EF Core
# Architektura
Architektura systemu jest klasyczn¹ architektur¹ 3-warstowa. Wybór spowodowany typowych schematem dzia³ania klient-serwer. Taka architektura by³a wystarczaj¹ca do zaimplementowania danych, ciê¿ko mówiæ w przypadku tego zadania o jakiœ encjach, których stanem moglibyœmy w jakiœ sposób zarz¹dzaæ.

Sk³ada siê ona z 3 g³ównych komponentów:

 - Api: w którym obs³ugiwane s¹ ¿¹dania u¿ytkowników wraz z generowaniem oraz weryfikacj¹ klucza API.
 - App: w nim znajduje siê ca³a logika biznesowa, zaimplementowany jest mechanizm zbierania danych z zewnêtrznych API oraz z lokalnych danych znajduj¹cych siê w cache,
 - Infrastructure: tutaj znajduj¹ siê funkcjonalnoœci zwi¹zane z dostêpem do danych, zarówno do bazy danych jak równie¿ integracja do zewnêtrznych serwisów, które udostêpniaj¹ te dane.
 
Ponadto istnieje mniejsza biblioteka CurrencyApplication.Database, w których zawarte s¹ mapowania encji, definicja DbContextu oraz odpowiednie konfiruguracje po³¹czenie z baz¹ danych. 

# Zewnêtrzne API pobieraj¹ce kursy walut

Skorzysta³em z serwisu [https://exchangeratesapi.io/](https://exchangeratesapi.io/). 
Aby móc skorzystaæ z implementacji innego API, nale¿y zaimplementowaæ interfejs **IExchangeRateProvider** a nastêpnie zadeklarowaæ jego u¿ycie w kontenerze IOC. 

# Dostêpne optymalizacje

Dane pobierane s¹ z API dopiero w ostatecznoœci. Aby przyspieszyæ odpowiedŸ z serwera, zaimplementowane zosta³y nastêpuj¹ce optymalizacje:

 - Zapisywanie pobranych wyników w bazie danych, przy kolejnej próbie odczytania wyniku, bêdzie on najpierw poszukiwany w bazie danych.
 - Cachowanie odpowiedzi na poziomie kontrolera ASP.NET. Jest ona usuwana z cache po up³ywie 1 minuty.  
 
 Dodanie (b¹dŸ deaktywacja) optymalizacji - takich jak np. przechowywanie danych w bazie danych w pamiêci (np. Redis) jest doœæ elastyczne. Wystarczy jedynie zaimplementowaæ interfejs **IFetchExchangeRateStep**, za pomoc¹ property Order ustawiæ mu kolejnoœæ wykonania, oraz zarejestrowaæ go w kontenerze. Dziêki temu dany krok wykona siê w odpowiednim momencie.
 Obecna sekwencja operacji:
 
 - Pobranie danych z lokalnej bazy danych
 - Pobranie danych z API
 - Jeœli danych nie ma w API, to wykonujemy ponown¹ próbê, tylko tym razem o 3 dni wiêcej do ty³u, aby trafiæ ostatni¹ zdefiniowan¹ wartoœæ.
 
 # Dostêpne endpointy
 - **/api/Login/GetApiKey** - s³u¿¹ca do wygenerowania i pobrania ApiKey
 -  **/api/ExchangeRate/GetExchangeRates**- s³u¿¹ca do pobrania informacji o kursach walut w kontekœcie zakresu dat. Parametry takie jak w opisie zadania ;)

W razie wszelkich pytañ/w¹tpliwoœci zapraszam do kontaktu, chêtnie na wszystkie udzielê odpowiedzi.
 

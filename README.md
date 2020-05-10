# Currency Test Application

Aplikacja zosta�a stworzona z my�l� o rekrutacji do firmy **AVENEO**. 
Stos technologiczny:

 - .NET Core 3.1
 - PostgresSQL 10 + Npgsql
 - Autofac
 - Automapper
 - EF Core
# Architektura
Architektura systemu jest klasyczn� architektur� 3-warstowa. Wyb�r spowodowany typowych schematem dzia�ania klient-serwer. Taka architektura by�a wystarczaj�ca do zaimplementowania danych, ci�ko m�wi� w przypadku tego zadania o jaki� encjach, kt�rych stanem mogliby�my w jaki� spos�b zarz�dza�.

Sk�ada si� ona z 3 g��wnych komponent�w:

 - Api: w kt�rym obs�ugiwane s� ��dania u�ytkownik�w wraz z generowaniem oraz weryfikacj� klucza API.
 - App: w nim znajduje si� ca�a logika biznesowa, zaimplementowany jest mechanizm zbierania danych z zewn�trznych API oraz z lokalnych danych znajduj�cych si� w cache,
 - Infrastructure: tutaj znajduj� si� funkcjonalno�ci zwi�zane z dost�pem do danych, zar�wno do bazy danych jak r�wnie� integracja do zewn�trznych serwis�w, kt�re udost�pniaj� te dane.
 
Ponadto istnieje mniejsza biblioteka CurrencyApplication.Database, w kt�rych zawarte s� mapowania encji, definicja DbContextu oraz odpowiednie konfiruguracje po��czenie z baz� danych. 

# Zewn�trzne API pobieraj�ce kursy walut

Skorzysta�em z serwisu [https://exchangeratesapi.io/](https://exchangeratesapi.io/). 
Aby m�c skorzysta� z implementacji innego API, nale�y zaimplementowa� interfejs **IExchangeRateProvider** a nast�pnie zadeklarowa� jego u�ycie w kontenerze IOC. 

# Dost�pne optymalizacje

Dane pobierane s� z API dopiero w ostateczno�ci. Aby przyspieszy� odpowied� z serwera, zaimplementowane zosta�y nast�puj�ce optymalizacje:

 - Zapisywanie pobranych wynik�w w bazie danych, przy kolejnej pr�bie odczytania wyniku, b�dzie on najpierw poszukiwany w bazie danych.
 - Cachowanie odpowiedzi na poziomie kontrolera ASP.NET. Jest ona usuwana z cache po up�ywie 1 minuty.  
 
 Dodanie (b�d� deaktywacja) optymalizacji - takich jak np. przechowywanie danych w bazie danych w pami�ci (np. Redis) jest do�� elastyczne. Wystarczy jedynie zaimplementowa� interfejs **IFetchExchangeRateStep**, za pomoc� property Order ustawi� mu kolejno�� wykonania, oraz zarejestrowa� go w kontenerze. Dzi�ki temu dany krok wykona si� w odpowiednim momencie.
 Obecna sekwencja operacji:
 
 - Pobranie danych z lokalnej bazy danych
 - Pobranie danych z API
 - Je�li danych nie ma w API, to wykonujemy ponown� pr�b�, tylko tym razem o 3 dni wi�cej do ty�u, aby trafi� ostatni� zdefiniowan� warto��.
 
 # Dost�pne endpointy
 - **/api/Login/GetApiKey** - s�u��ca do wygenerowania i pobrania ApiKey
 -  **/api/ExchangeRate/GetExchangeRates**- s�u��ca do pobrania informacji o kursach walut w kontek�cie zakresu dat. Parametry takie jak w opisie zadania ;)

W razie wszelkich pyta�/w�tpliwo�ci zapraszam do kontaktu, ch�tnie na wszystkie udziel� odpowiedzi.
 

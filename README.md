# CatalogOnline - Proiect PAW
Acest proiect este realizat în ASP.NET MVC și simulează un sistem de management al unei facultăți. Aplicația dispune de patru tipuri de utilizatori: Student, Profesor, Secretar și Admin, fiecare având pagini și funcționalități distincte, în funcție de rolul său.

## Roluri și Funcționalități ale utilizatorilor
### Student
- vizualizează cursurile la care este înscris și notele sale.
- generează adeverința.
### Profesor
- vizualizează cursurile pe care le predă.
- vizualizează studenții înscriși pentru fiecare curs.
- adaugă sau șterge notele studenților.
### Secretar
- vizualizează toate cursurile existente.
- vizualizează profesorul care predă fiecare curs.
- vizualizează toți studenții și notele lor pentru fiecare curs.
- generareaza in format PDF cataloagele cu note pentru cursuri.
### Admin
- vizualizează toate cursurile.
- adaugă sau șterge utilizatori și cursuri.
- asociază studenți cu cursuri.
- resetează întreaga bază de date prin apăsarea butonului roșu, ștergând toate informațiile din baza de date.

## Caracteristici
- autentificare și autorizare bazată pe roluri.
- paginile și funcționalitățile sunt separate pentru fiecare rol de utilizator.
- management dinamic al cursurilor și notelor.

## Tehnologii Utilizate
- ASP.NET MVC
- Entity Framework
- SQL Server
- JavaScript (pentru filtre si generare PDF-uri)
- jsPDF (pentru generarea de PDF-uri)

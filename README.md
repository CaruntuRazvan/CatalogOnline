# CatalogOnline - Proiect PAW
Acest proiect este realizat în ASP.NET MVC și simulează un sistem de management al Facultății de Matematica și Informatica din București, sectia CTI. Aplicația este conectată la o bază de date relațională în care sunt stocate informațiile necesare pentru funcționarea sistemului. Prin intermediul acestor tabele și al relațiilor dintre ele, aplicația poate gestiona informațiile legate de cursuri, înscrieri și note pentru utilizatorii sistemului, oferind funcționalități distincte în funcție de rolul fiecărui utilizator.
### Users
- stochează informații despre utilizatori, inclusiv numele, prenumele, rolul și alte detalii relevante. Rolul utilizatorului poate fi unul din următoarele: Student, Profesor, Secretar sau Admin.
### Courses
- conține detalii despre cursuri, inclusiv numele lor. De asemenea, această tabelă include o cheie străină care se referă la id-ul utilizatorului de tip Profesor, indicând cine predă cursul respectiv.
### Enrollments
- are rolul de a asocia studenții cu cursurile la care sunt înscriși. Această tabelă conține două chei străine, una care indică id-ul studentului și alta care indică id-ul cursului la care este înscrierea.
### Grades
 - este utilizată pentru a stoca notele obținute de studenți la cursurile respective. Asemenea tabelului Enrollments, această tabelă conține două chei străine: una care indică id-ul studentului și alta care indică id-ul cursului la care a fost obținută nota.

## Roluri și funcționalități ale utilizatorilor
### Student
- vizualizează cursurile la care este înscris și notele sale
- generează adeverința
### Profesor
- vizualizează cursurile pe care le predă
- vizualizează studenții înscriși pentru fiecare curs
- adaugă sau șterge notele studenților
### Secretar
- vizualizează toate cursurile existente
- vizualizează profesorul care predă fiecare curs
- vizualizează toți studenții și notele lor pentru fiecare curs
- generareaza in format PDF cataloagele cu note pentru studenti
### Admin
- vizualizează toate cursurile si o lista cu studentii inscrisi la acel curs
- adaugă sau șterge utilizatori și cursuri
- asociază studenți cu cursuri
- sterge asocieri intre studenti si cursuri
- resetează întreaga bază de date, ștergând toate informațiile din baza de date

## Tehnologii Utilizate
- ASP.NET MVC
- Entity Framework
- SQL Server
- JavaScript (pentru filtre si generare PDF-uri)
- jsPDF (pentru generarea de PDF-uri)

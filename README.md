# Dokumentacja
FilePicker.cs zawiera:
- <b>GetFilePath()</b>

Zwraca ścieżkę do pliku jako string, albo "NullPath" jeżeli plik nie zostanie wybrany
- <b>GetFileContent()</b>

Zwraca zawartość pliku jako string, albo String.Empty w przypadku złapania wyjątku przy otwieraniu pliku

# Założenia projektu
Pani Aleksandro, Aplikację się buduje, a dokładniej rozbudowywuje o dodatkowe komponenty. Dlatego też nie ma granicy zagadnienia z mojej strony, to dana grupa je okresli. Według moich założeń aplikacja powinna pozwalać na: 
- L1 Wygenerowanie wykresu XY w czasie rzeczywistym (na podstawie danych z pliku i ich zapętlenia) - zmiana sygnału analogowego U(t), ;
- L2 Zdefiniowanie narzędzia pomiarowego - nazwy, typu, zakresu pomiarowego i sygnału wyjściowego uzytego czujnika - sygnał analogowy U(t); Przeprowadzenie kalibracji czujnika pomiarowego (funkcja liniowa + ew. inna np. kwadratowa) - tzn. WF(t) = a x U(t) + b, sygnał analogowy U(t) Wygenerowanie wykresu XY w czasie rzeczywistym (na podstawie danych z pliku i ich zapętlenia) i po przeprowadzeniu kalibracji - wykres zmiany WF(t) na podstawie pliku danych
- L3 Wygenerowanie wykresu XY w czasie rzeczywistym (na podstawie danych z pliku i ich zapętleniu) i po przxeorwadzeniu kalibracji dla sygnału, przy czym sygnał jest oparty o liczbę impulsów pojawiających się w czasie, co wymaga innego przeliczania w stosunku do punktu 1 i 2.
- L4 Połączenie efektów z punktu 2 i 3 w postaci aplikacji umożliwiającej generowanie kliku wykresów jednocześnie dla różnych czujników, porównanie wyników Zdefiniowanie danych firmy i klienta, czasu, a więc przez kogo, dla którego i kiedy realziowany był dany pomiar a w konsekwencji raport. (ekstra) Generowanie gotowego raportu np. w postaci pliku PDF lub word (extra) Inne zaproponowane przez grupę będące indywidualnym pomysłem. Pozdrawiam


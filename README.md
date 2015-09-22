Реализовать HTTP сервер в виде консольного приложения. Без использования WCF, ASP.NET.

Порт, по которому будут приниматься запросы, должен читаться из конфигурационного файла. 

Информацию о запросах необходимо писать на консоль.

1) На любой запрос из браузера отдавать «Hello world!»

2) Реализовать гостевую книгу с поддержкой двух функций:

 при запросе GET /Guestbook/ отдавать все записи;

 при запросе POST /Guestbook/ добавлять запись в гостевую книгу, принимая параметры 

user и message.

Сообщения хранить в XML файле.

3) Второй вариант хранения данных гостевой книги. 

Сохранять сообщения в базу SQLite организовав там таблицы Users и Messages. 

(Управление вариантами хранения через конфигурационный файл.)

4) При запросе /Proxy/ с параметром url, HTTP сервер должен возвращать содержимое страницы 

расположенной по указанному url.
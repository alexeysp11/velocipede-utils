# Description

[English](Description.md) | [Русский](Description.ru.md)

## Требования

### Функциональные требования

- Приватный чат.
- Групповой чат.
- Прочтите квитанцию.
- Статус онлайн.
- Уведомления.
- Делитесь мультимедиа.

### Системные требования

- Низкая задержка.
- Высокая надежность.
- Высокая доступность.
- Web, mobile, desktop.
- История чата.
- High BLOB store.
- E2E-шифрование.
- Веб-сокеты.

### Планирование мощности

- Всего активных пользователей: 500M.
- Каждый пользователь отправляет 30 сообщений в день (в среднем).
- Общее количество сообщений в день: `500M * 30 = 1500M = 1,5B`.
- Сообщений в секунду: `1,5Б/(3600*24) = 18К`

### Оценка хранилища

Как рассчитать размер базы данных?

- Всего сообщений в день: 1,5 млрд.
- Каждое сообщение: 50 КБ.
- Общий объем памяти: `1,5Б * 50КБ = 75 ПБ`.

## Сервисы

- [Messaging service](Services/MessagingService.ru.md)
- [Group service](Services/MessagingService.ru.md)
- [Authentication service](Services/AuthenticationService.ru.md)
- [Last seen service](Services/LastSeenService.ru.md)
- [Asset service](Services/AssetService.ru.md)

Диаграмма коммуникации между сервисами:

![SystemOverview](img/SystemOverview.png)

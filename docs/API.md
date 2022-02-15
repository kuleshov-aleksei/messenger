## Содержание:

**Работа с чатами:**

[Получить список чатов](#получить-список-чатов)

[Получить список пользователей в чате](#получить-список-пользователей-в-чате)

[Создать чат](#создать-чат)

[Пригласить пользователя в чат](#пригласить-пользователя-в-чат)

[Пригласить пользователя в чат используя никнейм](#пригласить-пользователя-в-чат-используя-никнейм)

**Работа с сообщениями:**

[Отправить сообщение пользователя](#отправить-сообщение-пользователя)

[Получить последние сообщения в чате](#получить-последние-сообщения-в-чате)

[Получить сообщения с определенной временной отметки](#получить-сообщения-с-определенной-временной-отметки)

**Авторизация + регистрация:**

[Зарегистрировать пользователя](#зарегистрировать-пользователя)

[Авторизация](#авторизация)

[Обновление Access token](#обновление-access-token)

**Информация о пользователе**

[Получить информацию о пользователе](#получить-информацию-о-пользователе)

[Изменить имя](#изменить-имя])

[Изменить фамилию](изменить-фамилию)

[Изменить почту](#изменить-почту)

[Изменить пароль](#изменить-пароль)

Все запросы имеют префикс:

http://api.encamy.keenetic.pro

## Работа с чатами

### Получить список чатов

URL: /chat/get_chat_list

POST, content-type = "application/json"

Запрос:
```json
{
	"acces_token": "value"
}
```

Ответ:
```json
{
    "chats": [
        {
            "title": "Test chat",
            "image_medium": null,
            "image_small": null,
            "last_message": null
        },
        {
            "title": "Тестовый чат",
            "image_medium": null,
            "image_small": null,
            "last_message": null
        },
        {
            "title": "Проба пера",
            "image_medium": null,
            "image_small": null,
            "last_message": null
        }
    ]
}
```

### Получить список пользователей в чате

URL: /chat/get_chat_members

POST, content-type = "application/json"

Запрос:
```json
{
	"chat_id": 1,
        "acces_token": "value"
}
```

Ответ:
```json
{
    "chat_members": [
        {
            "name": "Michael",
            "surname": "Bobrov",
            "joined_at": "2020-10-05T21:54:34",
            "invited_by_name": null,
            "invited_by_surname": null
        },
        {
            "name": "Ivan",
            "surname": "Popov",
            "joined_at": "2020-10-05T22:34:54",
            "invited_by_name": "Michael",
            "invited_by_surname": "Bobrov"
        }
    ]
}
```

### Создать чат

URL: /chat/create_chat

POST, content-type = "application/json"

Запрос:
```json
{
	"user_id": 3,
	"title": "Chat from API"
}
```

При успехе будет возвращен код 200, при ошибке 400 или 500

### Пригласить пользователя в чат

URL: /chat/invite_to_chat

POST, content-type = "application/json"

Запрос:
```json
{
	"invited_user_id": 1,
	"acces_token": "value"
	"chat_id": 5
}
```

### Пригласить пользователя в чат используя никнейм

URL: /chat/invite_to_chat_username

POST, content-type = "application/json"

Запрос:
```json
{
	"invited_username": "someuser",
	"acces_token": "value"
	"chat_id": 5
}
```

## Работа с сообщениями

### Отправить сообщение пользователя

URL: /messenger/put_message

POST, content-type = "application/json"

Запрос:
```json
{
	"acces_token": "value"
	"message": "Hello world!",
	"chat_id": 1
}
```

При успехе будет возвращен код 200, при ошибке 400+ или 500

### Получить последние сообщения в чате

Данный запрос должен выполняться при первоначальной загрузке чата  
Загружаются только последние 30 сообщений  
**Для получения остальных сообщений необходимо использовать метод `TODO`**

Обязательно при отрисовке сообщений нужно проверять id чата возращенного от сервера и id который сейчас отрисован у пользователя!

URL: /messenger/get_last_messages

POST, content-type = "application/json"

Запрос:
```json
{
	"chat_id": 1,
        "acces_token": "value"
}
```

Ответ (количество сообщений в примере урезано):
```json
{
    "messages": [
        {
            "unix_time": 1602339798,
            "text": "Ехал грека через реку видит грека в реке рак",
            "author_name": "Michael",
            "author_surname": "Bobrov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337282,
            "text": "Сообщение 35",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337281,
            "text": "Сообщение 34",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337280,
            "text": "Сообщение 33",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337278,
            "text": "Сообщение 32",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        }
    ],
    "chat_id": 1
}
```

### Получить сообщения с определенной временной отметки

URL: /messenger/get_messages_from_time

POST, content-type = "application/json"

Запрос:
```json
{
	"chat_id": 1,
	"unix_time": 1602337199,
        "acces_token": "value"
}
```

Ответ в таком же формате как в [Получить последние сообщения в чате](#получить-последние-сообщения-в-чате)


## Авторизация + регистрация

### Зарегистрировать пользователя:

**Перед использованием обязательно прочитать [вот это](Скрипты-mysql#создать-пользователя)**

URL: /register/register

POST, content-type = "application/json"

Запрос:
```json
{
	"username": "username_api",
	"name": "postman",
	"surname": "rect",
	"email": "example_api@example.com",
	"password": "my_password"
}
```

При успехе ответ `200 OK`, или `400 Bad request` на неправильный запрос или `409 Conflict` если пользователь с таким логином или почтой уже существует.

Также, при 409 будет отправлено это сообщение:
```json
{
    "error_message": "User with this username or email already exists"
}
```

### Авторизация 

URL: /auth/auth

POST, content-type = "application/json"

Поле login может принимать как username так и почту

Запрос:
```json
{
	"login": "example_api@example.com",
	"password": "my_password",
	"device_name": "postman"
}
```

При успехе будет возвращен 200 ОК, а также установлены соответствующие куки

### Обновление Access token

Когда Access token протухает необходимо выполнить данный запрос

URL: /auth/refresh
POST, content-type = "application/json"

```json
{
	"refresh_token": "sometoken",
}
```

## Информация о пользователе

### Получить информацию о пользователе

URL: /user/get_info
POST, content-type = "application/json"

Запрос:
```json
{
    "access_token": "123"
}
```

Ответ:
```json
{
  "name": "postman",
  "surname": "rect",
  "email": "example_api@example.com",
  "username": "username_api",
  "roles": [
    {
      "title": "admin",
      "description": "Администратор",
      "date_assigned": "2020-11-01T16:18:25",
      "assigned_by_name": "SYSTEM",
      "assigned_by_surname": "SYSTEM",
      "assigned_by_username": "SYSTEM"
    },
    {
      "title": "regular",
      "description": "Пользователь",
      "date_assigned": "2020-11-01T17:03:31",
      "assigned_by_name": "SYSTEM",
      "assigned_by_surname": "SYSTEM",
      "assigned_by_username": "SYSTEM"
    }
  ],
  "image_large": null,
  "image_medium": null,
  "image_small": "user_avatars/demo3.png"
}
```

### Изменить имя

URL: /user/change_name
POST, content-type = "application/json"

Запрос:
```json
{
    "access_token": "123",
    "new_name": "NAME"
}
```

Ответ:
200 OK или ошибка в обычном формате

### Изменить фамилию

URL: /user/change_surname
POST, content-type = "application/json"

Запрос:
```json
{
    "access_token": "123",
    "new_surname": "SURNAME"
}
```

Ответ:
200 OK или ошибка в обычном формате

### Изменить почту

URL: /user/change_email
POST, content-type = "application/json"

Запрос:
```json
{
    "access_token": "123",
    "new_email": "user@example.com"
}
```

Ответ:
200 OK или ошибка в обычном формате

### Изменить пароль

URL: /user/change_password
POST, content-type = "application/json"

Запрос:
```json
{
    "access_token": "123",
    "current_password": "password",
    "new_password": "new_password"
}
```

Ответ:
200 OK или ошибка в обычном формате
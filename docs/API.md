## Table of contents:

**Chats:**

[Get chat list](#get-chat-list)

[Get list of users in chat](#get-chat-members)

[Create a chat](#create-a-chat)

[Invite user in chat](#Invite-user-to-chat)

[Invite user in chat by nickname](#Invite-user-to-chat-by-nickname)

**Messages:**

[Send message to user](#send-message)

[Get last messages](#get-last-messages)

[Get last messages by timestamp](#get-last-messages-by-timestamp)

**Authorize + register:**

[Register a user](#register-a-user)

[Authorize](#authorize)

[Refresh Access token](#refresh-access-token)

**Information about user**

[Get information about user](#get-information-about-user)

[Change name](#change-name)

[Change surname](#change-surname)

[Change email](#change-email)

[Change password](#change-password)

Base url:

https://messenger.encamy.com

## Chats:

### Get chat list

URL: /chat/get_chat_list

POST, content-type = "application/json"

Request:
```json
{
	"acces_token": "value"
}
```

Response:
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
            "title": "Test chat 2",
            "image_medium": null,
            "image_small": null,
            "last_message": null
        },
        {
            "title": "Test chat 3",
            "image_medium": null,
            "image_small": null,
            "last_message": null
        }
    ]
}
```

### Get chat members

URL: /chat/get_chat_members

POST, content-type = "application/json"

Request:
```json
{
	"chat_id": 1,
        "acces_token": "value"
}
```

Response:
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

### Create a chat

URL: /chat/create_chat

POST, content-type = "application/json"

Request:
```json
{
	"user_id": 3,
	"title": "Chat from API"
}
```

In case of success, 200 will be returned, otherwise 400 or 500

### Invite user to chat

URL: /chat/invite_to_chat

POST, content-type = "application/json"

Request:
```json
{
	"invited_user_id": 1,
	"acces_token": "value",
	"chat_id": 5
}
```

### Invite user to chat by nickname

URL: /chat/invite_to_chat_username

POST, content-type = "application/json"

Request:
```json
{
	"invited_username": "someuser",
	"acces_token": "value",
	"chat_id": 5
}
```

## Messages:

### Send message

URL: /messenger/put_message

POST, content-type = "application/json"

Request:
```json
{
	"acces_token": "value",
	"message": "Hello world!",
	"chat_id": 1
}
```

In case of success, 200 will be returned, otherwise 400 or 500

### Get last messages

This method should be executed at chat load  
Loaded only last 30 messages  
**If you want to get older messages, use [Get last messages by timestamp](#get-last-messages-by-timestamp)**

URL: /messenger/get_last_messages

POST, content-type = "application/json"

Request:
```json
{
	"chat_id": 1,
        "acces_token": "value"
}
```

Response (trimmed version):
```json
{
    "messages": [
        {
            "unix_time": 1602339798,
            "text": "Roads? Where we're going we don't need roads.",
            "author_name": "Michael",
            "author_surname": "Bobrov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337282,
            "text": "Message 35",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337281,
            "text": "Message 34",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337280,
            "text": "Message 33",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        },
        {
            "unix_time": 1602337278,
            "text": "Message 32",
            "author_name": "Ivan",
            "author_surname": "Popov",
            "author_image_link_small": null
        }
    ],
    "chat_id": 1
}
```

### Get last messages by timestamp

URL: /messenger/get_messages_from_time

POST, content-type = "application/json"

Request:
```json
{
	"chat_id": 1,
	"unix_time": 1602337199,
        "acces_token": "value"
}
```

Reponse is the same as in [Get last messages](#get-last-messages)


## Authorize + register

### Register a user:

URL: /register/register

POST, content-type = "application/json"

Request:
```json
{
	"username": "username_api",
	"name": "postman",
	"surname": "rect",
	"email": "example_api@example.com",
	"password": "my_password"
}
```

In case of success `200 OK` will be returned, otherwise `400 Bad request` or `409 Conflict`.

If result is 409, you can expect this respose:
```json
{
    "error_message": "User with this username or email already exists"
}
```

### Authorize 

URL: /auth/auth

POST, content-type = "application/json"

Both username and email can be used as `login` field.

Request:
```json
{
	"login": "example_api@example.com",
	"password": "my_password",
	"device_name": "postman"
}
```

In case of success, 200 ОК will be returned. As well as login cookies.

### Refresh Access token

When access token expires, token need to be refreshed:

URL: /auth/refresh
POST, content-type = "application/json"

```json
{
	"refresh_token": "sometoken",
}
```

## Information about user

### Get information about user

URL: /user/get_info
POST, content-type = "application/json"

Request:
```json
{
    "access_token": "123"
}
```

Response:  
```json
{
  "name": "postman",
  "surname": "rect",
  "email": "example_api@example.com",
  "username": "username_api",
  "roles": [
    {
      "title": "admin",
      "description": "Administrator",
      "date_assigned": "2020-11-01T16:18:25",
      "assigned_by_name": "SYSTEM",
      "assigned_by_surname": "SYSTEM",
      "assigned_by_username": "SYSTEM"
    },
    {
      "title": "regular",
      "description": "User",
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

### Change name

URL: /user/change_name
POST, content-type = "application/json"

Request:
```json
{
    "access_token": "123",
    "new_name": "NAME"
}
```

Response:  
In case of success, 200 OK will be returned, 400 or 500 otherwise

### Chage surname

URL: /user/change_surname
POST, content-type = "application/json"

Request:
```json
{
    "access_token": "123",
    "new_surname": "SURNAME"
}
```

Response:  
In case of success, 200 OK will be returned, 400 or 500 otherwise

### Change email

URL: /user/change_email
POST, content-type = "application/json"

Request:
```json
{
    "access_token": "123",
    "new_email": "user@example.com"
}
```

Response:  
In case of success, 200 OK will be returned, 400 or 500 otherwise

### Change password

URL: /user/change_password
POST, content-type = "application/json"

Request:
```json
{
    "access_token": "123",
    "current_password": "password",
    "new_password": "new_password"
}
```

Response:  
In case of success, 200 OK will be returned, 400 or 500 otherwise
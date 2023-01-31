# API для книжного магазина

Мое первое API на ASP.NET Core.

Изучил на этом проекте:
- Основы работы EF Core с базой данных Postgresql
- Основные принципы ASP.NET CORE Web API
- Применение JWT токенов

# Endpoints

## Sign Up

### Регистрация пользователя.
Пароль хэшируется. 

```js
POST /api/signup
```

Пример тела запроса:

```json
{
  "username": "string",
  "password": "string",
  "name": "string",
  "surname": "string",
  "eMail": "string"
}
```

Ответы:

200 &mdash; пользователь зарегистрирован

400 &mdash; Пользователь с таким логином уже существует(сообщение об ошибке выдается на клиент)

## Log in

### Вход в систему

```js
POST /api/login
```

Пример тела запроса:
```json
{
  "username": "string",
  "password": "string"
}
```

Пример тела ответа:
```json
{
    "token": "string",
    "username": "string",
    "name": "string",
    "surname": "string",
    "email": "string",
    "role": "string"
}
```

Ответы:

200 &mdash; пользователь прошел авторизацию

404 &mdash; Пользователь не найден

## Profile

### Обновление профиля пользователя

```js
PUT /api/profile
```

Пример тела запроса:

```json
{
  "token": "string",
  "username": "string",
  "name": "string",
  "surname": "string",
  "eMail": "string",
  "role": "string"
}
```

Ответы:

200 &mdash; пользователь прошел авторизацию

404 &mdash; Пользователь не найден / Информация о пользователе не найдена

## Book
### Получение списка всех книг

```js
GET /api/book
```

Пример тела ответа:
```json
[
  {
    "bookId": 0,
    "name": "string",
    "author": "string",
    "price": 10000000
  }
]
```

Ответы:

200 &mdash; книги найдены

### Добавление книги

```js
POST /api/book/
```

Пример тела запроса:
```json
{
  "name": "string",
  "author": "string",
  "price": 10000000
}
```

Ответы:

200 &mdash; книга успешно добавлена

500 &mdash; что-то пошло не так при сохранении

### Удаление книги

```js
DELETE /api/book/{bookId}
```

Примеры ответа:

200 &mdash; книга упешно удалена

404 &mdash; книга не найдена

### Обновление информации о книге
```js
PUT /api/book/{bookId}
```

Пример тала запроса:

```json
{
  "name": "string",
  "author": "string",
  "price": 10000000
}
```
Примеры ответа:

200 &mdash; книга упешно удалена

404 &mdash; книга не найдена

## Cart

### Получение корзины пользователя

Информация о пользователе берется из токена.

```js
GET /api​/cart
```

Примеры ответа:

200 &mdash; корзина получена

404 &mdash; пользователь/корзина не найдена

### Добавление книги в корзину

```js
POST /api​/cart
```

Пример тела запроса:

```json
{
  "bookId": 0,
  "name": "string",
  "author": "string",
  "price": 10000000
}
```

Пример ответа:

200 &mdash; предмет добавлен в корзину

400 &mdash; пользователь не найден

## Order

### Получение списка заказов

```js
GET /api​/order
```

Пример тела ответа:
```json
{ 
    "book": 
    {
        { 
            "bookId": 1,
            "name": "Война и мир",
            "author": "Лев Николаевич Толстой",
            "price": 154     
        },
        "count": 3
    }
}
```

Пример ответа:

200 &mdash; заказ получен

400 &mdash; пользователь не найден

### Сделать заказ

```js
POST /api​/order
```

Данные заказа берутся из корзины

Примеры ответа:

200 &mdash; заказ оформлен

400 &mdash; пользователь не найден

400 &mdash; корзина пуста

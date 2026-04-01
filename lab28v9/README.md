# lab28v9 – Серіалізація об'єктів у JSON

## Тема
Серіалізація об’єктів у JSON

## Мета
Навчитися серіалізувати та десеріалізувати складні об’єкти у форматі JSON, зберігати дані у файл та завантажувати їх.

---

## Структура проєкту

- Models/
  - Post.cs
  - Comment.cs
- Repository/
  - PostRepository.cs
- Program.cs
- data.json

---

## Опис класів

### Post
Містить інформацію про пост:
- Id
- Title
- Content
- Список коментарів (Comments)

### Comment
Містить:
- Id
- Author
- Text

### PostRepository
Реалізує роботу з даними:
- Add() — додавання поста
- GetAll() — отримати всі пости
- GetById(int id) — отримати по ID
- SaveToFileAsync() — збереження в JSON
- LoadFromFileAsync() — завантаження з JSON

---

## Використані технології

- C#
- .NET 8
- System.Text.Json
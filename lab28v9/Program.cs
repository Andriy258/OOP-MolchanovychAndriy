using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using lab28v9.Models;
using lab28v9.Repository;

namespace lab28v9.Models;
    public class Program
    {
        static async Task Main()
    {
        var repo = new PostRepository();

        // створення постів
        var post1 = new Post
        {
            Id = 1,
            Title = "Перший пост",
            Content = "Це мій перший блог пост",
            Comments = new List<Comment>
            {
                new Comment { Id = 1, Author = "Andriy", Text = "Круто!" },
                new Comment { Id = 2, Author = "User", Text = "Цікаво" }
            }
        };

        var post2 = new Post
        {
            Id = 2,
            Title = "Другий пост",
            Content = "JSON це легко",
            Comments = new List<Comment>
            {
                new Comment { Id = 3, Author = "Admin", Text = "Супер" }
            }
        };

        repo.Add(post1);
        repo.Add(post2);

        // збереження
        await repo.SaveToFileAsync("data.json");
        Console.WriteLine("Дані збережено у JSON");

        // очищення (щоб показати завантаження)
        repo = new PostRepository();

        // завантаження
        await repo.LoadFromFileAsync("data.json");

        Console.WriteLine("\nЗавантажені дані:");

        foreach (var post in repo.GetAll())
        {
            Console.WriteLine($"\nPost: {post.Title}");
            Console.WriteLine(post.Content);

            foreach (var comment in post.Comments)
            {
                Console.WriteLine($"  Comment ({comment.Author}): {comment.Text}");
            }
        }
    }
    }
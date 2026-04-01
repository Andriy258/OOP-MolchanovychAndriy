using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using lab28v9.Models;



namespace lab28v9.Repository;

    public class PostRepository
    {private List<Post> _posts = new();

    public void Add(Post post)
    {
        _posts.Add(post);
    }

    public List<Post> GetAll()
    {
        return _posts;
    }

    public Post? GetById(int id)
    {
        return _posts.FirstOrDefault(p => p.Id == id);
    }

    public async Task SaveToFileAsync(string filename)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };

        using var stream = new FileStream(filename, FileMode.Create);

#pragma warning disable IL2026, IL3050
        await JsonSerializer.SerializeAsync(stream, _posts, options);
#pragma warning restore IL2026, IL3050
    }

    public async Task LoadFromFileAsync(string filename)
    {
        if (!File.Exists(filename))
            return;

        using var stream = new FileStream(filename, FileMode.Open);

#pragma warning disable IL2026, IL3050
        var posts = await JsonSerializer.DeserializeAsync<List<Post>>(stream);
#pragma warning restore IL2026, IL3050

        if (posts != null)
            _posts = posts;
    }
}
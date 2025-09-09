namespace LAB1v9
{
    class Movie
    {
        private string title;
        private string genre;
        public double Rating { get; set; }

        // Конструктор
        public Movie(string title, string genre, double rating)
        {
            this.title = title;
            this.genre = genre;
            this.Rating = rating;
            Console.WriteLine($"Movie created: {title} ({genre}), Rating: {rating}");
        }

        // Метод
        public void Play()
        {
            Console.WriteLine($"Now playing: {title} - Genre: {genre}, Rating: {Rating}");
        }

        // Деструктор (рідко використовується, лише для демонстрації)
        ~Movie()
        {
            Console.WriteLine($"Movie {title} is being destroyed.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Movie movie1 = new Movie("Inception", "Sci-Fi", 8.8);
            Movie movie2 = new Movie("The Godfather", "Crime", 9.2);
            Movie movie3 = new Movie("Interstellar", "Adventure", 8.6);

            movie1.Play();
            movie2.Play();
            movie3.Play();

            Console.WriteLine("All movies played.");
        }
    }
}

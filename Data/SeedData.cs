using System.Text.Json;
using MyLibrary.api.Entities;

namespace MyLibrary.api.Data
{
    public static class SeedData
    {
       
        public static async Task LoadMovieData(MyLibraryContext context)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (context.Movies.Any()) return;

            var json = System.IO.File.ReadAllText("Data/json/movie.json");

            var movie = JsonSerializer.Deserialize<List<Movie>>(json, options);

            if(movie is not null && movie.Count > 0)
            {
                await context.Movies.AddRangeAsync(movie);
                await context.SaveChangesAsync();
            }
        }
        public static async Task LoadSerieData(MyLibraryContext context)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (context.Series.Any()) return;

            var json = System.IO.File.ReadAllText("Data/json/serie.json");

            var series = JsonSerializer.Deserialize<List<Serie>>(json, options);

            if(series is not null && series.Count > 0)
            {
                await context.Series.AddRangeAsync(series);
                await context.SaveChangesAsync();
            }
        }
        
    }
}
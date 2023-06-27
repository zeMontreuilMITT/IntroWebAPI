using System.Text;
using TestAPI.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Movies Endpoint
app.MapGet("/movies/all", (string? title) =>
{
    HashSet<Movie> results = new HashSet<Movie>();

    if (!String.IsNullOrEmpty(title))
    {
        results.Add(DataContainer.Movies.FirstOrDefault(m => m.Title.ToLower().StartsWith(title.ToLower())));
    } else
    {
        results = DataContainer.Movies.ToHashSet();
    }

    return results;
});

app.Run();

static class DataContainer
{
    private static int s_idCount = 1;

    public static HashSet<Movie> Movies = new HashSet<Movie>()
    {
        new Movie(s_idCount++, "Spider Man 2", 2023),
        new Movie(s_idCount++, "Transformers", 2023),
        new Movie(s_idCount++, "Gone with the Wind", 1939),
        new Movie(s_idCount++, "Scarface", 1987)
    };
}
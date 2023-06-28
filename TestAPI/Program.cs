using Microsoft.AspNetCore.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using TestAPI.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Movies Endpoint
app.MapGet("/movies", (int? id) =>
{
    HashSet<Movie> results = new HashSet<Movie>();

    if (id != null)
    {
        Movie found = DataContainer.Movies.FirstOrDefault(m => m.Id == id);
        if(found != null)
        {
            return Results.Ok(found);
        } else
        {
            return Results.NotFound();
        }

      
    } else
    {
        results = DataContainer.Movies.ToHashSet();
    }

    return Results.Ok(results);
});


app.MapPost("/movies", (Movie movie) =>
{
    try
    {
        DataContainer.Movies.Add(movie);
        return Results.Created($"movies/{movie.Id}", movie);
    } catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapDelete("/movies/{id}", (int id) =>
{
    Movie? toDelete = DataContainer.Movies.FirstOrDefault(m => m.Id == id);

    if(toDelete == null)
    {
        return Results.NotFound();
    } else
    {
        DataContainer.Movies.Remove(toDelete);
        return Results.NoContent();
    }
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


    static DataContainer()
    {

    }

}


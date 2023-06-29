using Microsoft.AspNetCore.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TestAPI.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

WebApplication app = builder.Build();


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

    foreach (Movie m in results)
    {
        // find related roles and add them to the movie's list of roles
        m.Roles.UnionWith(DataContainer.Roles.Where(r => r.MovieId == m.Id));
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

app.MapPut("/movies/edit/{id}", (int id, string? title, int? year) =>
{
    // if the movie is found, update values
    Movie movie;
    movie = DataContainer.Movies.FirstOrDefault(m => m.Id == id);

    if(movie == null)
    {
        if(title != null && year  != null)
        {
            // resource not found, but can create new one
            try
            {
                movie = new Movie(DataContainer.GetNewId(), title, (int)year);
                DataContainer.Movies.Add(movie);
                return Results.Created($"/movies/{movie.Id}", movie);
            } catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }

        } else
        {
            // resource not found and cannot create new resource
            return Results.BadRequest();
        }
    } else
    {
        try
        {
            if (title != null)
            {
                movie.Title = title;
            }

            if (year != null)
            {
                movie.Year = (int)year;
            }
            // respond with updated Movie object
            return Results.Ok(movie);
        }
        catch(Exception ex) { 
            // respond with error in trying to update movie
            return Results.Problem(ex.Message);
        }
    }
});


// Roles Endpoint
app.MapGet("/roles", (int actorId) =>
{
    // query the actor that we're looking for roles on
    try
    {
        Actor actor = DataContainer.Actors.First(a => a.Id == actorId);
        
        HashSet<Role> roles = DataContainer.Roles.Where(r => r.ActorId == actorId).ToHashSet<Role>();

        return Results.Ok(new { Actor = actor, Roles = roles });

    } catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapPost("/roles", (int actorId, int movieId, string credit) =>
{
    try
    {
        Actor actor = DataContainer.Actors.First(a => a.Id == actorId);
        Movie movie = DataContainer.Movies.First(m => m.Id == movieId);

        Role newRole = DataContainer.CreateRole(actor, movie, credit);

        return Results.Created($"/roles?actorId={actor.Id}", newRole);

    } catch(Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

static class DataContainer
{
    private static int s_idCount = 1;

    public static int GetNewId()
    {
        return s_idCount++;
    }

    // Seed method
    public static HashSet<Movie> Movies = new HashSet<Movie>();
    public static HashSet<Actor> Actors = new HashSet<Actor>();
    public static HashSet<Role> Roles = new HashSet<Role>();

    public static Role CreateRole(Actor actor, Movie movie, string roleName)
    {
        // create the new middle object and give it the IDs and Object References to its related objects
        Role newRole = new Role() { CreditedTitle = roleName, 
            ActorId = actor.Id, 
            MovieId = movie.Id };

        // add references to the "inner" object on the "outer" ones

        // Add to our "database"
        Roles.Add(newRole);
        return newRole;
    }


    static DataContainer()
    {
        Movie movie1 = new Movie(s_idCount++, "Transformers", 2023);
        Movie movie2 = new Movie(s_idCount++, "Gone with the Wind", 1939);
        Movie movie3 = new Movie(s_idCount++, "Scarface", 1987);
        Movie movie4 = new Movie(s_idCount++, "Serpico", 1973);

        Actor actor1 = new Actor() { Id = s_idCount++, Name = "Al Pacino" };

        Movies.Add(movie1);
        Movies.Add(movie2);
        Movies.Add(movie3);
        Movies.Add(movie4);

        Actors.Add(actor1 );

        CreateRole(actor1, movie3, "Tony Montana");
        CreateRole(actor1, movie4, "Frank Serpico");
    }

}

// Create endpoints that show all of the roles for an Actor, and allows us to add a role for an Actor to a Movie
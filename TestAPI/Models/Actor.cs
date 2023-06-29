namespace TestAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public HashSet<Role> Roles { get; set; } = new HashSet<Role>();
    }
}

// HashSet<Movie> KeanuMovies = Movies.Where(m => m.Roles.Any(r => r.Actor.Name.StartsWith("Keanu")));

/*
 * SELECT * FROM Actor, Role, Movie
 * INNER JOIN Role 
 * ON Role.ActorId == Actor.Id
 * INNER JOIN Movie
 * ON Movie.Id == Role.MovieId
 * WHERE Actor.Name LIKE 'Keanu%'
 */
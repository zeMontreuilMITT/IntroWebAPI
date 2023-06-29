namespace TestAPI.Models
{
    public class Role
    {
        public int ActorId { get; set; }
        public Actor Actor { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public string CreditedTitle { get; set; }
    }
}

// Actor.Roles.Foreach(r => Console.WriteLine(r.Movie.Title));
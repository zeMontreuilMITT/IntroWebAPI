namespace TestAPI.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public HashSet<Role> Roles { get; set; } = new HashSet<Role>();
        public string Name { get; set; }

        public Actor(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

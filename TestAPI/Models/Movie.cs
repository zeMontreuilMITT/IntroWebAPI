namespace TestAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }

        private int _year;
        public int Year { get
            {
                return _year;
            }
            set
            {
                if (value < 1900)
                {
                    _year = 1900;
                } else if (value > DateTime.Now.Year)
                {
                    _year = DateTime.Now.Year;
                } else
                {
                    _year = value;
                }
            } }

        public HashSet<Role> Roles { get; set; } = new HashSet<Role>();
        public Movie(int id, string title, int year)
        {
            Id = id;
            Title = title;
            Year = year;
        }
    }
}

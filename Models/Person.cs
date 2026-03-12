namespace Models
{
    public class Person : Entity
    {

        public Person Parent { get; set;  }
        public ICollection<Person> Children { get; set; } = [];

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;


        public string FullName => $"{FirstName} {LastName}";
        public string? NullString { get; set; }
        public string EmptyString { get; set; } = string.Empty;
        public int DefaultInt { get; set; }
        public DateTime DefaultDateTime { get; set; }

        public bool ShouldSerializeEmptyString()
        {
            return !string.IsNullOrEmpty(EmptyString);
        }
    }
}

using Models.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Person : Entity
    {

        public Person? Parent { get; set;  }
        public ICollection<Person> Children { get; set; } = [];

        [Required]
        [MyValidationAnnotation(Value = "a")]
        [MyValidationAnnotation(Value = "b")]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(15)]
        public string LastName { get; set; } = string.Empty;

        [Range(18, 65)]
        public int Age { get; set; }

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

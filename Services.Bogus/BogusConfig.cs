using System.ComponentModel.DataAnnotations;

namespace Services.Bogus
{
    public class BogusConfig
    {
        public int NumberOfResources { get; set;  }
        public int NumberOfNestedResources { get; set; }
        [Required]
        public string Language { get; set; }
    }
}

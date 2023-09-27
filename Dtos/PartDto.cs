using System.ComponentModel.DataAnnotations;

namespace PartsWebApi.Dtos
{
    public class PartDto
    {
        public Guid Id { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}\d{4}$", ErrorMessage = "The Name field must be in the format 'ABC1234'.")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Za-z]{3}-\d{3}$", ErrorMessage = "The Component field must be in the format 'ABC-123'.")]
        public string Component { get; set; }

        [RegularExpression(@"^[A-Z]\d$", ErrorMessage = "The Destination field must be in the format 'P1'.")]
        public string Destination { get; set; }
    }
}

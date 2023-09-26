using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Part
    {
        public Guid Id { get; set; }

        [StringLength(7)]
        public string Name { get; set; }

        [StringLength(7)]
        public string Component { get; set; }

        [StringLength(2)]
        public DestinationType? Destination { get; set; }
    }
}

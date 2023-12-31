﻿using System.ComponentModel.DataAnnotations;

namespace PartsWebApi.Entities
{
    public class DestinationType
    {
        [Key, StringLength(2)]
        public string DestinationTypeId { get; set; }

        public int Order { get; set; }

        public ICollection<Part> Parts { get; set; }
    }
}

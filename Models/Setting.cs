﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Amado.Models
{
    public class Setting
    {
        public int ID { get; set; }
        public string Key { get; set; }

        [StringLength(400)]
        public string Value { get; set; }
        
        [NotMapped]
        public IFormFile File { get; set; }
    }
}

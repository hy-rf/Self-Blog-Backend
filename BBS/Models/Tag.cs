﻿using System.ComponentModel.DataAnnotations;

namespace BBS.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}

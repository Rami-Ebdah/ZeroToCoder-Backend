﻿using System;
using System.ComponentModel.DataAnnotations;


namespace ZeroToCoder.Models
{
    public class FAQ
    {
        [Key]
        public int FAQID { get; set; }

        [MaxLength(1000)]
        public string? Question { get; set; }

        [MaxLength(1000)]
        public string? Answer { get; set; }

    }
}

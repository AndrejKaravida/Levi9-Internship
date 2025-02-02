﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Code9.Amazon.WebAPI.Core.Models
{
    public class Model
    {
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public Make Make { get; set; }
        public int MakeId { get; set; }
    }
}

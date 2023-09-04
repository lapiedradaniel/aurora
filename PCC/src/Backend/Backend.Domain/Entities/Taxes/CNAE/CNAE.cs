﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Domain.Entities.Taxes.CNAE
{
    public class CNAE
    {
        [Key]
        public Guid Id { get; set; }
        public string? Description { get; set; }
    }
}
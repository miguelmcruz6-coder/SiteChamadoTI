using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Enums;

namespace BackEnd.DTOs
{
    public class AtualizarStatusDto
    {
        [Required]
        public StatusChamado Status { get; set; }
    }
}
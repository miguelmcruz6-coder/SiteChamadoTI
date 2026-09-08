using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Enums;

namespace BackEnd.DTOs
{
    public class CriarChamadoDto
    {
        [Required]
        [MinLength(5)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public CategoriaChamado Categoria { get; set; }

        [Required]
        public PrioridadeChamado Prioridade { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Enums;

namespace BackEnd.DTOs
{
    public class ChamadoResponseDto
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public CategoriaChamado Categoria { get; set; }

        public PrioridadeChamado Prioridade { get; set; }

        public StatusChamado Status { get; set; }

        public DateTime DataCriacao { get; set; }
    }
}
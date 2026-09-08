using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Enums;

namespace BackEnd.Models
{
    public class Chamado
    {
        public int Id { get; set; }

        public string Titulo { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public CategoriaChamado Categoria { get; set; }

        public PrioridadeChamado Prioridade { get; set; }

        public StatusChamado Status { get; set; }

        public DateTime DataCriacao { get; set; }

        
        public int? ResponsavelId { get; set; }

        public TipoUsuario? TipoResponsavel { get; set; }
    }
}
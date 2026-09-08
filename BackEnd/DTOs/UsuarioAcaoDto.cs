using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Enums;

namespace BackEnd.DTOs
{
    public class UsuarioAcaoDto
    {
        public int UsuarioId { get; set; }

        public TipoUsuario TipoUsuario { get; set; }
    }
}
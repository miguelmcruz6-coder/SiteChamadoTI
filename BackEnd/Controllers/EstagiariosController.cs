using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.DTOs;
using BackEnd.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstagiariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EstagiariosController(AppDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // GET: api/estagiarios/chamados
        // Lista chamados
        // ==========================================

        [HttpGet("chamados")]
        public async Task<ActionResult<IEnumerable<ChamadoResponseDto>>> ListarChamados(
            [FromQuery] StatusChamado? status)
        {
            var query = _context.Chamados.AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            var chamados = await query
                .OrderByDescending(c => c.DataCriacao)
                .Select(c => new ChamadoResponseDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Descricao = c.Descricao,
                    Categoria = c.Categoria,
                    Prioridade = c.Prioridade,
                    Status = c.Status,
                    DataCriacao = c.DataCriacao
                })
                .ToListAsync();

            return Ok(chamados);
        }


        // ==========================================
        // PUT: api/estagiarios/chamados/5/status
        // Estagiário pode alterar o status
        // ==========================================

        [HttpPut("chamados/{id}/status")]
        public async Task<IActionResult> AlterarStatus(
            int id,
            AtualizarStatusDto dto)
        {
            var estagiario = await _context.Estagiarios
                .FirstOrDefaultAsync(e => e.Ativo);

            if (estagiario == null)
            {
                return Forbid();
            }

            if (!Enum.IsDefined(typeof(StatusChamado), dto.Status))
            {
                return BadRequest(new
                {
                    mensagem = "Status inválido."
                });
            }

            var chamado = await _context.Chamados
                .FindAsync(id);

            if (chamado == null)
            {
                return NotFound(new
                {
                    mensagem = "Chamado não encontrado."
                });
            }

            chamado.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Status alterado com sucesso.",
                chamado.Id,
                chamado.Status
            });
        }
    }
}
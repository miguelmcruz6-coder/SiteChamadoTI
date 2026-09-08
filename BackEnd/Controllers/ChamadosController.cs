using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.DTOs;
using BackEnd.Enums;
using BackEnd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ChamadosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/chamados
        // GET: api/chamados?status=Pendente
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChamadoResponseDto>>> GetChamados(
            [FromQuery] StatusChamado? status)
        {
            var query = _context.Chamados.AsQueryable();

            // Filtro opcional por status
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


        // POST: api/chamados
        [HttpPost]
        public async Task<ActionResult<ChamadoResponseDto>> CriarChamado(
            CriarChamadoDto dto)
        {
            // Validação da categoria
            if (!Enum.IsDefined(typeof(CategoriaChamado), dto.Categoria))
            {
                return BadRequest(new
                {
                    mensagem = "Categoria inválida."
                });
            }

            // Validação da prioridade
            if (!Enum.IsDefined(typeof(PrioridadeChamado), dto.Prioridade))
            {
                return BadRequest(new
                {
                    mensagem = "Prioridade inválida."
                });
            }

            var chamado = new Chamado
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Categoria = dto.Categoria,
                Prioridade = dto.Prioridade,

                // Todo chamado começa como pendente
                Status = StatusChamado.Pendente,

                DataCriacao = DateTime.Now
            };

            _context.Chamados.Add(chamado);

            await _context.SaveChangesAsync();

            var resposta = new ChamadoResponseDto
            {
                Id = chamado.Id,
                Titulo = chamado.Titulo,
                Descricao = chamado.Descricao,
                Categoria = chamado.Categoria,
                Prioridade = chamado.Prioridade,
                Status = chamado.Status,
                DataCriacao = chamado.DataCriacao
            };

            return CreatedAtAction(
                nameof(GetChamado),
                new { id = chamado.Id },
                resposta
            );
        }


        // GET: api/chamados/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ChamadoResponseDto>> GetChamado(int id)
        {
            var chamado = await _context.Chamados
                .Where(c => c.Id == id)
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
                .FirstOrDefaultAsync();

            if (chamado == null)
            {
                return NotFound(new
                {
                    mensagem = "Chamado não encontrado."
                });
            }

            return Ok(chamado);
        }


        // PUT: api/chamados/1/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(
            int id,
            AtualizarStatusDto dto)
        {
            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
            {
                return NotFound(new
                {
                    mensagem = "Chamado não encontrado."
                });
            }

            // Verifica se o status enviado existe no Enum
            if (!Enum.IsDefined(typeof(StatusChamado), dto.Status))
            {
                return BadRequest(new
                {
                    mensagem = "Status inválido."
                });
            }

            chamado.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                mensagem = "Status atualizado com sucesso.",
                chamado.Id,
                chamado.Status
            });
        }


        // DELETE: api/chamados/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarChamado(int id)
        {
            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
            {
                return NotFound(new
                {
                    mensagem = "Chamado não encontrado."
                });
            }

            _context.Chamados.Remove(chamado);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarChamado(
            int id,
            [FromQuery] int usuarioId,
            [FromQuery] TipoUsuario tipoUsuario)
        {
            // Somente Admin pode excluir
            if (tipoUsuario != TipoUsuario.Admin)
            {
                return Forbid();
            }

            var admin = await _context.Admins
                .FirstOrDefaultAsync(a =>
                    a.Id == usuarioId &&
                    a.Ativo);

            if (admin == null)
            {
                return Forbid();
            }

            var chamado = await _context.Chamados.FindAsync(id);

            if (chamado == null)
            {
                return NotFound(new
                {
                    mensagem = "Chamado não encontrado."
                });
            }

            _context.Chamados.Remove(chamado);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
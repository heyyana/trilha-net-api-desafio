using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TarefaController : ControllerBase
	{
		private readonly OrganizadorContext _context;

		public TarefaController(OrganizadorContext context)
		{
			_context = context;
		}

		[HttpGet("{id}")]
		public IActionResult ObterPorId(int id)
		{
			if (id == 0)
			{
				return BadRequest("O ID não pode ser zero.");
			}

			var tarefa = _context.Tarefas.Find(id);

			if (tarefa is null)
			{
				return NotFound();
			}
			return Ok(tarefa);
		}

		[HttpGet("ObterTodos")]
		public IActionResult ObterTodos()
		{
			var tarefa = _context.Tarefas.ToList();

			if (tarefa == null || !tarefa.Any())
			{
				return NoContent();
			}

			return Ok(tarefa);
		}

		[HttpGet("ObterPorTitulo")]
		public IActionResult ObterPorTitulo(string titulo)
		{
			if (string.IsNullOrEmpty(titulo))
			{
				return BadRequest("O título não poder ser vazio ou nulo.");
			}

			var tarefas = _context.Tarefas
								  .Where(t => t.Titulo.Contains(titulo))
								  .ToList();

			if (tarefas == null || !tarefas.Any())
			{
				return NotFound("Nenhuma tarefa encontrada com o título informado.");
			}

			return Ok(tarefas);
		}

		[HttpGet("ObterPorData")]
		public IActionResult ObterPorData(DateTime data)
		{
			var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
			return Ok(tarefa);
		}

		[HttpGet("ObterPorStatus")]
		public IActionResult ObterPorStatus(EnumStatusTarefa status)
		{
			var tarefa = _context.Tarefas
								 .Where(x => x.Status == status)
								 .ToList();

			if (tarefa == null || !tarefa.Any())
			{
				return NotFound("Nenhuma tarefa encontrada com o status informado.");
			}

			return Ok(tarefa);
		}

		[HttpPost]
		public IActionResult Criar(Tarefa tarefa)
		{
			if (tarefa.Data == DateTime.MinValue)
				return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

			_context.Tarefas.Add(tarefa);

			_context.SaveChanges();

			return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
		}

		[HttpPut("{id}")]
		public IActionResult Atualizar(int id, Tarefa tarefa)
		{
			var tarefaBanco = _context.Tarefas.Find(id);

			if (tarefaBanco == null)
				return NotFound();

			if (tarefa.Data == DateTime.MinValue)
				return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });


			tarefaBanco.Titulo = tarefa.Titulo;
			tarefaBanco.Descricao = tarefa.Descricao;
			tarefaBanco.Data = tarefa.Data;
			tarefaBanco.Status = tarefa.Status;

			_context.SaveChanges();

			return Ok(tarefaBanco);
		}

		[HttpDelete("{id}")]
		public IActionResult Deletar(int id)
		{
			var tarefaBanco = _context.Tarefas.Find(id);

			if (tarefaBanco == null)
				return NotFound();

			_context.Tarefas.Remove(tarefaBanco);

			_context.SaveChanges();

			return NoContent();
		}
	}
}
using JanganKantoi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JanganKantoi.Controllers
{
	//public class GameController : Controller
	//{
	//	public IActionResult Index()
	//	{
	//		return View();
	//	}


	//}
	[ApiController]
	[Route("api/game")]
	public class GameController : ControllerBase
	{
		private readonly MyDbContext _context;

		public GameController(MyDbContext context)
		{
			_context = context;
		}

		[HttpPost("start")]
		public async Task<IActionResult> StartGame([FromBody] GameStartRequest request)
		{
			try
			{
				var category = await _context.Categories
					.AsNoTracking()
					.FirstOrDefaultAsync(x => x.categories_name == request.CategoryName);

				if (category == null)
					return BadRequest("Category not found");

				var wordsQuery = _context.Words
					.AsNoTracking()
					.Where(x => x.word_category == category.categories_id);

				var wordCount = await wordsQuery.CountAsync();

				if (wordCount == 0)
					return BadRequest("No words found");

				var randomIndex = Random.Shared.Next(wordCount);

				var word = await wordsQuery
					.Skip(randomIndex)
					.FirstOrDefaultAsync();

				if (word == null)
					return BadRequest("No words found");

				var players = new List<object>();

				for (int i = 0; i < request.PlayerCount; i++)
				{
					players.Add(new
					{
						role = "Civilian",
						word = word.word_name
					});
				}

				for (int i = 0; i < request.ImposterCount; i++)
				{
					int index;

					do
					{
						index = Random.Shared.Next(request.PlayerCount);
					}
					while (players[index].GetType().GetProperty("role")?.GetValue(players[index])?.ToString() == "Imposter");

					players[index] = new
					{
						role = "Imposter",
						word = word.word_hint
					};
				}

				return Ok(new
				{
					players = players
				});
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}

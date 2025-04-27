using Microsoft.AspNetCore.Mvc;
using TennisStatsAPI.Services;

namespace TennisStatsAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TennisPlayersController : ControllerBase
	{
		private readonly ITennisPlayerService _tennisPlayerService;
		private readonly ITennisPlayersStatsService _tennisPlayersStatsService;
		public TennisPlayersController(ITennisPlayerService tennisPlayerService, ITennisPlayersStatsService tennisPlayersStatsService)
		{
			_tennisPlayerService = tennisPlayerService;
			_tennisPlayersStatsService = tennisPlayersStatsService;
		}

		/// <summary>
		/// Créer un endpoint qui retourne les joueurs.
		/// La liste doit être triée du meilleur au moins bon.
		/// </summary>
		/// <returns>liste des joueurs</returns>
		[HttpGet()]
		public IActionResult GetAllPlayers()
		{
			return Ok(_tennisPlayerService.GetAllPlayers());
		}


		/// <summary>
		/// Créer un endpoint qui permet de retourner les informations d’un joueur grâce à son ID.
		/// </summary>
		/// <param name="id">Id</param>
		/// <returns>Player</returns>
		[HttpGet("{id}")]
		public IActionResult GetPlayerById(int id)
		{
			var player = _tennisPlayerService.GetPlayerById(id);
			if (player == null)
				return NotFound(new { Message = "Player not found." });

			return Ok(player);
		}

		/// <summary>
		/// Créer un endpoint qui retourne les statistiques suivantes :
		///- Pays qui a le plus grand ratio de parties gagnées
		///- IMC moyen de tous les joueurs
		///- La médiane de la taille des joueurs
		/// </summary>
		/// <returns></returns>
		[HttpGet("Stats")]
		public IActionResult GetStats()
		{
			var stats = _tennisPlayersStatsService.GetPlayersStats();

			return Ok(stats);
		}
	}
}
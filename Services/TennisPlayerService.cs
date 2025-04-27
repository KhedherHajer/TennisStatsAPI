using System.Text.Json;
using TennisStatsAPI.Models;

namespace TennisStatsAPI.Services
{
	public class TennisPlayerService : ITennisPlayerService
	{
		private readonly ILogger<TennisPlayerService> _logger;
		private readonly List<Player> _players;

		public TennisPlayerService(IWebHostEnvironment env, ILogger<TennisPlayerService> logger)
		{
			_logger = logger;
			try
			{
				var filePath = Path.Combine(env.ContentRootPath, "Database", "players.json");

				if (!File.Exists(filePath))
					throw new FileNotFoundException($"The players database file was not found at {filePath}");

				var json = File.ReadAllText(filePath);
				if (string.IsNullOrWhiteSpace(json))
					_players = new List<Player>();

				var jsonData = JsonSerializer.Deserialize<PlayersList>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (jsonData == null || jsonData.Players == null)
					_players = new List<Player>();

				_players = jsonData.Players;
				_logger.LogInformation("Successfully loaded {Count} players from database.", _players.Count);
			}
			catch (FileNotFoundException ex)
			{
				_logger.LogError(ex, "File error: {Message}", ex.Message);
				_players = new List<Player>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error while loading players: {Message}", ex.Message);
				_players = new List<Player>();
			}
		}


		public IEnumerable<Player> GetAllPlayers()
		{
			return _players.OrderBy(p => p.Data.Rank);
		}

		public Player? GetPlayerById(int id)
		{
			return _players.FirstOrDefault(p => p.Id == id);
		}

	}
}
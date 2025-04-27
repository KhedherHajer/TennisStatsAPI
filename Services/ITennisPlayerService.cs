using TennisStatsAPI.Models;

namespace TennisStatsAPI.Services
{
	public interface ITennisPlayerService
	{
		IEnumerable<Player> GetAllPlayers();
		Player? GetPlayerById(int id);
	}
}

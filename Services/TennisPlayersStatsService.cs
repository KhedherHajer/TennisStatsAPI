using TennisStatsAPI.Models;

namespace TennisStatsAPI.Services
{
	public class TennisPlayersStatsService: ITennisPlayersStatsService
	{
		private readonly ILogger<TennisPlayersStatsService> _logger;
		private ITennisPlayerService _tennisPlayerService;

		public TennisPlayersStatsService(ITennisPlayerService tennisPlayerService, ILogger<TennisPlayersStatsService> logger)
		{
			_tennisPlayerService = tennisPlayerService;
			_logger = logger;
		}

		public PlayersStats GetPlayersStats()
		{
			try
			{
				var players = _tennisPlayerService.GetAllPlayers();
				if (!players.Any())
				{
					_logger.LogWarning("No players found.");
					return new PlayersStats();
				}

				//Pays qui a le plus grand ratio de parties gagnées
				var bestCountry = FindBestCountryByWinsRatio(players);

				//Le calcul de l'indice de masse corporelle ou IMC : poids divisé par la taille au carré
				var averageImc = CalculateAverageImc(players);

				//Médiane des tailles des joueurs 
				var heights = players.Select(p => p.Data.Height);
				double medianHeight = CalculateMedian(heights);

				return new PlayersStats
				{
					BestCountry = bestCountry,
					AverageIMC = Math.Round(averageImc, 2),
					MedianPlayersHeight = medianHeight
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while getting player stats.");
				throw new ApplicationException("An error occurred while calculating player stats.", ex);
			}
		}

		public Country? FindBestCountryByWinsRatio(IEnumerable<Player> players)
		{
			var bestCountryWinRatio = players.GroupBy(p => p.Country.Code)
				.Select(g =>
				{
					var games = g.Sum(p => p.Data.Last.Count);
					var wins = g.Sum(p => p.Data.Last.Sum());
					return
					new
					{
						winRation = wins / games,
						countryCode = g.Key
					};
				}).OrderByDescending(y => y.winRation).FirstOrDefault();
			
			if (bestCountryWinRatio == null)
			{
				_logger.LogWarning("No country data available for calculating the win ratio.");
				return null;
			}

			return players.FirstOrDefault(p => p.Country.Code.Equals(bestCountryWinRatio.countryCode))?.Country;
		}

		public double CalculateAverageImc(IEnumerable<Player> players)
		{
			return players.Select(p =>
			{
				var heightInMeters = p.Data.Height / 100.0;
				var weightInKilograms = p.Data.Weight / 1000.0;
				return weightInKilograms / Math.Pow(heightInMeters, 2);
			}).Average();
		}

		public static int CalculateMedian(IEnumerable<int> elements)
		{
			elements.Order();
			int mid = elements.Count() / 2;
			int median = 0;
			if (mid % 2 != 0)
			{
				median = elements.ElementAt(mid);
			}
			else
			{
				median = (elements.ElementAt(mid - 1) + elements.ElementAt(mid)) / 2;
			}

			return median;
		}
	}
}

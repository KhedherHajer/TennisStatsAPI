namespace TennisStatsAPI.Models
{
	/// <summary>
	/// statistiques suivantes :
	///- Pays qui a le plus grand ratio de parties gagnées
	///- IMC moyen de tous les joueurs
	///- La médiane de la taille des joueurs
	/// </summary>
	public class PlayersStats
	{
		public Country? BestCountry { get; set; }
		public double AverageIMC { get; set; }
		public double MedianPlayersHeight { get; set; }
	}
}

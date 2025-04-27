using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using TennisStatsAPI.Models;
using TennisStatsAPI.Services;

public class TennisPlayerServiceTests
{
	[Fact]
	public void GetAllPlayers_ShouldReturnPlayersOrderedByRank()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<TennisPlayerService>>();
		var envMock = new Mock<IWebHostEnvironment>();
		envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

		// Simuler un fichier JSON valide
		var testPlayers = new PlayersList
		{
			Players = new List<Player>
			{
				new Player { Id = 1, Firstname = "Test1", Data = new PlayerData { Rank = 2 } },
				new Player { Id = 2, Firstname = "Test2", Data = new PlayerData { Rank = 1 } },
			}
		};

		var dbPath = Path.Combine(envMock.Object.ContentRootPath, "Database");
		Directory.CreateDirectory(dbPath);
		File.WriteAllText(Path.Combine(dbPath, "players.json"), JsonSerializer.Serialize(testPlayers));

		var service = new TennisPlayerService(envMock.Object, loggerMock.Object);

		// Act
		var players = service.GetAllPlayers().ToList();

		// Assert
		Assert.Equal(2, players.Count);
		Assert.Equal(2, players[0].Id); // ID 2 (Rank 1) doit être premier
		Assert.Equal(1, players[1].Id); // ID 1 (Rank 2) doit être deuxième
	}

	[Fact]
	public void GetPlayerById_ShouldReturnCorrectPlayer()
	{
		// Arrange
		var loggerMock = new Mock<ILogger<TennisPlayerService>>();
		var envMock = new Mock<IWebHostEnvironment>();
		envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());

		var testPlayers = new PlayersList
		{
			Players = new List<Player>
			{
				new Player { Id = 1, Firstname = "Test1", Data = new PlayerData { Rank = 2 } }
			}
		};

		var dbPath = Path.Combine(envMock.Object.ContentRootPath, "Database");
		Directory.CreateDirectory(dbPath);
		File.WriteAllText(Path.Combine(dbPath, "players.json"), JsonSerializer.Serialize(testPlayers));

		var service = new TennisPlayerService(envMock.Object, loggerMock.Object);

		// Act
		var player = service.GetPlayerById(1);

		// Assert
		Assert.NotNull(player);
		Assert.Equal("Test1", player.Firstname);
	}
}

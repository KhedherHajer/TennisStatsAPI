using Microsoft.Extensions.Logging;
using Moq;
using TennisStatsAPI.Models;
using TennisStatsAPI.Services;

public class TennisPlayersStatsServiceTests
{
	[Fact]
	public void GetPlayersStats_ShouldReturnValidStats()
	{
		// Arrange
		var players = new List<Player>
		{
			new Player
			{
				Id = 1,
				Country = new Country { Code = "FRA" },
				Data = new PlayerData
				{
					Last = new List<int> { 1, 1, 0, 1 },
					Height = 180,
					Weight = 75000
				}
			},
			new Player
			{
				Id = 2,
				Country = new Country { Code = "ESP" },
				Data = new PlayerData
				{
					Last = new List<int> { 0, 0, 1, 0 },
					Height = 185,
					Weight = 80000
				}
			}
		};

		var tennisPlayerServiceMock = new Mock<ITennisPlayerService>();
		tennisPlayerServiceMock.Setup(x => x.GetAllPlayers()).Returns(players);

		var loggerMock = new Mock<ILogger<TennisPlayersStatsService>>();

		var service = new TennisPlayersStatsService(tennisPlayerServiceMock.Object, loggerMock.Object);

		// Act
		var stats = service.GetPlayersStats();

		// Assert
		Assert.NotNull(stats);
		Assert.NotNull(stats.BestCountry);
		Assert.True(stats.AverageIMC > 0);
		Assert.True(stats.MedianPlayersHeight > 0);
	}

	[Fact]
	public void CalculateBestCountryByWinsRatio_ShouldReturnCountryWithHighestWinRatio()
	{
		var players = new List<Player>
		{
			new Player
			{
				Id = 1,
				Country = new Country { Code = "FRA" },
				Data = new PlayerData { Last = new List<int> { 1, 1, 1, 1 } }
			},
			new Player
			{
				Id = 2,
				Country = new Country { Code = "ESP" },
				Data = new PlayerData { Last = new List<int> { 0, 0, 1, 0 } }
			}
		};

		var tennisPlayerServiceMock = new Mock<ITennisPlayerService>();
		tennisPlayerServiceMock.Setup(x => x.GetAllPlayers()).Returns(players);

		var loggerMock = new Mock<ILogger<TennisPlayersStatsService>>();

		var service = new TennisPlayersStatsService(tennisPlayerServiceMock.Object, loggerMock.Object);

		var bestCountry = service.FindBestCountryByWinsRatio(players);

		Assert.NotNull(bestCountry);
		Assert.Equal("FRA", bestCountry.Code);
	}

	[Fact]
	public void CalculateAverageImc_ShouldReturnCorrectAverage()
	{
		var players = new List<Player>
		{
			new Player
			{
				Id = 1,
				Data = new PlayerData { Height = 180, Weight = 72000 }
			},
			new Player
			{
				Id = 2,
				Data = new PlayerData { Height = 190, Weight = 80000 }
			}
		};

		var tennisPlayerServiceMock = new Mock<ITennisPlayerService>();
		var loggerMock = new Mock<ILogger<TennisPlayersStatsService>>();
		var service = new TennisPlayersStatsService(tennisPlayerServiceMock.Object, loggerMock.Object);

		var averageImc = service.CalculateAverageImc(players);

		Assert.True(averageImc > 0);
	}

	[Fact]
	public void CalculateMedian_ShouldReturnCorrectMedian()
	{
		var heights = new List<int> { 180, 185, 190 };

		var median = TennisPlayersStatsService.CalculateMedian(heights);

		Assert.Equal(185, median);
	}
}

using Moq;
using ShipmentDataDiscountManager.Loggers;

namespace ShipmentDataDiscountManager.Tests
{
	[TestClass]
	public class LoggerTests
	{
		private Mock<ILogger> _loggerMock;
		
		[TestInitialize]
		public void TestInitialize()
		{
			_loggerMock = new Mock<ILogger>();
		}

		[TestMethod]
		public async Task CallingLogInfoOnce()
		{
			//arrange
			await _loggerMock.Object.LogInfoAsync("Test information");

			//act & assert
			_loggerMock.Verify(l => l.LogInfoAsync(It.IsAny<string>()), Times.Once);
		}

		[TestMethod]
		public async Task CallingLogErrorOnce()
		{
			//arrange
			await _loggerMock.Object.LogErrorAsync("Test error");

			//act & assert
			_loggerMock.Verify(l => l.LogErrorAsync(It.IsAny<string>()), Times.Once);
		}
	}
}
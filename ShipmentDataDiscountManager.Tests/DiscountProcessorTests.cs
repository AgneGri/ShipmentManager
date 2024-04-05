using Moq;
using ShipmentDataDiscountManager.DiscountSchemas;
using ShipmentDataDiscountManager.FileRepository;
using ShipmentDataDiscountManager.Models;

namespace ShipmentDataDiscountManager.Tests
{
	[TestClass]
	public class DiscountProcessorTests
	{
		private Mock<IFileReader> _readerMock;
		private List<Mock<IDiscountSchema>> _schemasMock;
		private List<Price> _prices;
		private InitialData _initialData;
		private Configuration _configuration;
		private DiscountProcessor _processor;

		[TestInitialize]
		public void TestInitialize()
		{
			_readerMock = new Mock<IFileReader>();
			_schemasMock = new List<Mock<IDiscountSchema>>
			{
				new Mock<IDiscountSchema>(),
				new Mock<IDiscountSchema>()
			};

			_prices = new List<Price>
			{
				new Price("S", "LP", 1.50m)
			};

			_initialData = new InitialData(10.00m, 0, -1);

			_configuration = new Configuration();
			_configuration.InitialData = _initialData;
			_configuration.Prices = _prices;

			_processor = new DiscountProcessor(
				_readerMock.Object,
				_schemasMock.Select(s => s.Object).ToList());
		}

		[TestMethod]
		public async Task CallingProcessAsyncWithCorrectlyFormattedLineWhenADiscountIsAppliedReturnsCorrectlyFormattedLineWithAppliedDiscount()
		{
			//arrange
			var inputLines = new List<string> { "2023-03-10 S MR" };
			var expectedLine = "2023-03-10 S MR 1.50 0.50";

			_readerMock.Setup(r => r.GetDataLinesAsync()).ReturnsAsync(inputLines);

			foreach (var schemaMock in _schemasMock)
			{
				schemaMock.Setup(
					s => s.Apply(
						It.IsAny<DateOnly>(), 
						It.IsAny<string>(), 
						It.IsAny<string>(), 
						It.IsAny<decimal>(), 
						It.IsAny<decimal>(), 
						ref It.Ref<decimal>.IsAny, 
						ref It.Ref<int>.IsAny)
					)
					.Returns("2023-03-10 S MR 1.50 0.50");
			}

			//act
			var resultLines = await _processor.ProcessAsync(_configuration);

			//assert
			Assert.IsTrue(resultLines.Contains(expectedLine));
		}

		[TestMethod]
		public async Task CallingProcessAsyncWithCorrectlyFormattedLineWhenNoDiscountIsAppliedReturnsCorrectlyFormattedLineWhichEndsWithHyphen()
		{
			//arrange
			var inputLines = new List<string> { "2023-03-10 L LP" };

			_readerMock.Setup(r => r.GetDataLinesAsync()).ReturnsAsync(inputLines);

			foreach (var schemaMock in _schemasMock)
			{
				schemaMock.Setup(
					s => s.Apply(
						It.IsAny<DateOnly>(), 
						It.IsAny<string>(), 
						It.IsAny<string>(), 
						It.IsAny<decimal>(), 
						It.IsAny<decimal>(), 
						ref It.Ref<decimal>.IsAny, 
						ref It.Ref<int>.IsAny)
					)
					.Returns((string)null);
			}

			//act
			var resultLines = await _processor.ProcessAsync(_configuration);

			//assert
			Assert.IsTrue(resultLines.All(line => !line.Contains("Ignored") && line.EndsWith("-")));
		}

		[TestMethod]
		public async Task CallingProcessAsyncWithInvalidDataReturnsFormattedLineWithProcessedInvalidDataAndAppendedWordIgnored()
		{
			//arrange
			var inputLines = new List<string> { "Invalid input line" };

			_readerMock.Setup(r => r.GetDataLinesAsync()).ReturnsAsync(inputLines);

			//act
			var resultLines = await _processor.ProcessAsync(_configuration);

			//assert
			Assert.IsTrue(resultLines.Any(line => line.Contains("Ignored")));
		}
	}
}
using ShipmentDataDiscountManager.DiscountSchemas;

namespace ShipmentDataDiscountManager.Tests
{
	[TestClass]
	public class SmallSizeDiscountSchemaTests
	{
		private SmallSizeDiscountSchema _schema;
		private decimal totalDiscountPerMonth;
		private int counter;

		[TestInitialize]
		public void TestInitialize()
		{
			_schema = new SmallSizeDiscountSchema();
			totalDiscountPerMonth = 10.00m;
			counter = 0;
		}

		[TestMethod]
		public void CallingApplyOnceAppliesDiscountForSmallPackageSizeAndReturnsCorrectDataInFormattedLine()
		{
			//arrange
			var date = new DateOnly(2022, 12, 14);
			var size = "S";
			var provider = "MR";
			var providersPrice = 2.00m;
			var lowestPrice = 1.50m;

			//act
			string result = _schema.Apply(
				date,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2022-12-14 S MR 1.50 0.50", result);
		}

		[TestMethod]
		[DataRow("2023, 1, 4", "S", "MR", "2.00", "1.50")]
		[DataRow("2023, 3, 17", "S", "MR", "2.00", "1.50")]
		[DataRow("2023, 6, 14", "S", "MR", "2.00", "1.50")]
		[DataRow("2023, 9, 24", "S", "MR", "2.00", "1.50")]
		public void CallingApplyFourTimesAppliesDiscountForSmallPackageSizeAndReturnsCorrectDataInFormattedLine(
			string dateString,
			string size,
			string provider,
			string providersPriceString,
			string lowestPriceString)
		{
			//arrange
			DateOnly.TryParse(dateString, out DateOnly date);
			Decimal.TryParse(providersPriceString, out decimal providersPrice);
			Decimal.TryParse(lowestPriceString, out decimal lowestPrice);

			var expectedResult = $"{date:yyyy-MM-dd} {size} {provider} 1.50 0.50";

			//act
			string result = _schema.Apply(
				date,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void CallingApplyWithInsuficientFundsCorrectlyAppliesParcialDiscount()
		{
			//arrange
			var dateOfLastMonth = new DateOnly(2023, 1, 4);
			var size = "S";
			var provider = "MR";
			var providersPrice = 2.00m;
			var lowestPrice = 1.50m;
			var totalDiscountPerMonth = 0.10m;

			//act
			string result = _schema.Apply(
				dateOfLastMonth,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2023-01-04 S MR 1.90 0.10", result);
		}

		[TestMethod]
		public void CallingApplyWithOtherProviderDoNotApllyDiscountButReturnsCorrectDataInFormattedLine()
		{
			//arrange
			var dateOfLastMonth = new DateOnly(2023, 1, 4);
			var size = "S";
			var provider = "LP";
			var providersPrice = 1.50m;
			var lowestPrice = 1.50m;

			//act
			var result = _schema.Apply(
				dateOfLastMonth,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2023-01-04 S LP 1.50 -", result);
		}

		[TestMethod]
		public void CallingApplyWithPriceValuesOfThreeDigitsAfterDecimalPointCorrectlyRoundsPriceAndAppliedDiscount()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 1, 4),
				"S",
				"MR",
				2.123m,
				1.295m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2023-01-04 S MR 1.30 0.83", result);
		}

		[TestMethod]
		public void CallingApplyWithIncorrectPackageSizeReturnsNull()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 2, 14),
				"L",
				"MR",
				4.00m,
				1.50m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.IsNull(result);
		}
	}
}
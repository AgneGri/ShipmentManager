using ShipmentDataDiscountManager.DiscountSchemas;

namespace ShipmentDataDiscountManager.Tests
{
	[TestClass]
	public class LpLargeSizeDiscountSchemaTests
	{
		private LpLargeSizeDiscountSchema _schema;
		private decimal totalDiscountPerMonth;
		private int counter;

		[TestInitialize]
		public void TestInitialize()
		{
			_schema = new LpLargeSizeDiscountSchema();
			totalDiscountPerMonth = 10.00m;
			counter = 0;
		}

	[TestMethod]
		public void CallingApplyAppliesDiscountOnThirdLPackageOfLPProviderAndReturnsCorrectDataInFormattedLine()
		{
			//arrange
			var counter = 2;
			var date = new DateOnly(2023, 2, 14);
			var size = "L";
			var provider = "LP";
			var providersPrice = 6.90m;
			var lowestPrice = 4.00m;

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
			Assert.AreEqual("2023-02-14 L LP 0.00 6.90", result);
		}

		[TestMethod]
		[DataRow("2023, 2, 17", "L", "LP", "6.90", "4.00", 0, "2023-02-17 L LP 6.90 -")]
		[DataRow("2023, 2, 18", "L", "LP", "6.90", "4.00", 1, "2023-02-18 L LP 6.90 -")]
		[DataRow("2023, 2, 19", "L", "LP", "6.90", "4.00", 2, "2023-02-19 L LP 0.00 6.90")]
		[DataRow("2023, 3, 1", "L", "LP", "6.90", "4.00", 3, "2023-03-01 L LP 6.90 -")]
		[DataRow("2023, 3, 12", "L", "LP", "6.90", "4.00", 4, "2023-03-12 L LP 6.90 -")]
		[DataRow("2023, 3, 13", "L", "LP", "6.90", "4.00", 5, "2023-03-13 L LP 0.00 6.90")]
		public void CallingApplyAppliesDiscountOnEveryThirdLPackageOfLPProviderButOnlyOnceInAMonth(
			string dateString,
			string size,
			string provider,
			string providersPriceString,
			string lowestPriceString,
			int initialCounter,
			string expectedResult)
		{
			//arrange
			DateOnly.TryParse(dateString, out DateOnly date);
			Decimal.TryParse(providersPriceString, out decimal providersPrice);
			Decimal.TryParse(lowestPriceString, out decimal lowestPrice);

			//act
			string result = _schema.Apply(
				date,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref initialCounter);
			
			//assert
			Assert.AreEqual(expectedResult, result);
		}

		[TestMethod]
		public void CallingApplyAppliesDiscountAndReturnsCorrectValueOfTotalDiscountPerMonth()
		{
			//arrange
			counter = 2;
			var dateOfLastMonth = new DateOnly(2023, 2, 17);
			var dateOfNewMonth = new DateOnly(2023, 3, 1);
			var size = "L";
			var provider = "LP";
			var providersPrice = 6.90m;
			var lowestPrice = 4.00m;
			var exepectedTotalDiscountPerMonth = totalDiscountPerMonth - providersPrice;

			//act
			_schema.Apply(
				dateOfLastMonth,
				size,
				provider,
				providersPrice,
				lowestPrice,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual(exepectedTotalDiscountPerMonth, totalDiscountPerMonth);
		}

		[TestMethod]
		public void CallingApplyDoNotApplyDiscountForLargeSizePackageOfLPProviderIfCalledOnce()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 2, 14),
				"L",
				"LP",
				6.90m,
				4.00m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2023-02-14 L LP 6.90 -", result);
		}

		[TestMethod]
		public void CallingApplyWithPriceValueOfThreeDigitsAfterDecimalPointCorrectlyRoundsThePrice()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 2, 14),
				"L",
				"LP",
				6.995m,
				4.00m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.AreEqual("2023-02-14 L LP 7.00 -", result);
		}

		[TestMethod]
		public void CallingApplyWithIncorrectProviderReturnsNull()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 2, 14),
				"L",
				"MR",
				6.90m,
				4.00m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.IsNull(result);
		}

		[TestMethod]
		public void CallingApplyWithIncorrectPackageSizeReturnsNull()
		{
			//act
			var result = _schema.Apply(
				new DateOnly(2023, 2, 14),
				"S",
				"LP",
				6.90m,
				4.00m,
				ref totalDiscountPerMonth,
				ref counter);

			//assert
			Assert.IsNull(result);
		}
	}
}
using System.Text.Json.Serialization;

namespace ShipmentDataDiscountManager.Models
{
	/// <summary>
	/// This class encapsulates all necessary initial data required for processing discounts,
	/// including monthly discount limit, counter and the current month.
	/// It also maps the initial data given in the application configuration.
	/// </summary>
	public class InitialData
	{
		public InitialData(
			decimal monthlyDiscount, 
			int counter, 
			int currentMonth)
		{
			MonthlyDiscount = monthlyDiscount;
			Counter = counter;
			CurrentMonth = currentMonth;
		}

		[JsonPropertyName("monthlyDiscount")]
		public decimal MonthlyDiscount { get; }

		[JsonPropertyName("counter")]
		public int Counter { get; }

		[JsonPropertyName("currentMonth")]
		public int CurrentMonth { get; }
	}
}
using System.Text.Json.Serialization;

namespace ShipmentDataDiscountManager.Models
{
	/// <summary>
	/// Represents the price for a specific provider and package size.
	/// This class is used to map the initial price settings provided in the application configuration,
	/// reflecting the initial price table given in the homework assigment.
	/// </summary>
	public class Price
	{
		public Price(
			string provider,
			string size,
			decimal priceValue)
		{
			Provider = provider;
			Size = size;
			PriceValue = priceValue;
		}

		[JsonPropertyName("provider")]
		public string Provider { get; }

		[JsonPropertyName("size")]
		public string Size { get; }

		[JsonPropertyName("priceValue")]
		public decimal PriceValue { get; }
	}
}
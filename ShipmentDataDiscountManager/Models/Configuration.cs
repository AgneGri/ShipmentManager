using System.Text.Json.Serialization;

namespace ShipmentDataDiscountManager.Models
{
	/// <summary>
	/// Represents the structure of the configuration settings derived from the appsettings.json file.
	/// </summary>
	public class Configuration
	{
		[JsonPropertyName("InitialData")]
		public InitialData InitialData { get; set; }

		[JsonPropertyName("Prices")]
		public List<Price> Prices { get; set; }

		public string InputDirectory { get; set; }
		public string ResultDirectory { get; set; }
	}
}
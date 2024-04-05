using ShipmentDataDiscountManager.FileRepository;
using ShipmentDataDiscountManager.Models;
using System.Text.Json;

namespace ShipmentDataDiscountManager
{
	public class ConfigurationProvider : IConfigurationProvider
	{
		private readonly IFileReader _fileReader;

		public ConfigurationProvider(IFileReader fileReader)
		{
			_fileReader = fileReader;

		}

		/// <summary>
		/// Asynchronously reads and deserializes the JSON configuration 
		/// from "appsettings.json" file into the Configuration object.
		/// </summary>
		/// <returns></returns>
		public async Task<Configuration> GetAsync()
		{
			string jsonString = await _fileReader.GetAllTextAsync();

			return JsonSerializer.Deserialize<Configuration>(jsonString);
		}
	}
}
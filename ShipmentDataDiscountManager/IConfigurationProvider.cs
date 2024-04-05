using ShipmentDataDiscountManager.Models;

namespace ShipmentDataDiscountManager
{
	public interface IConfigurationProvider
	{
		Task<Configuration> GetAsync();
	}
}
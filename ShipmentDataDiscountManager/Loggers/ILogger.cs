namespace ShipmentDataDiscountManager.Loggers
{
	public interface ILogger
	{
		Task LogInfoAsync(string message);
		Task LogErrorAsync(string message);
	}
}
using ShipmentDataDiscountManager.Loggers;

namespace ShipmentDataDiscountManager
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var logger = new Logger("logInfo.txt", "logError.txt");

			try
			{
				await logger.LogInfoAsync("The application started.");

				await new App().RunAsync(args);

				await logger.LogInfoAsync("The application ended successfully.");
			}
			catch (Exception ex)
			{
				await logger.LogErrorAsync($"An error occurred: {ex.Message}");

				Console.WriteLine("An error occurred while running the application.");
			}
		}
	}
}
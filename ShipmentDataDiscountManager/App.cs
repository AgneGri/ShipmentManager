using ShipmentDataDiscountManager.DiscountSchemas;
using ShipmentDataDiscountManager.FileRepository;
using ShipmentDataDiscountManager.Loggers;

namespace ShipmentDataDiscountManager
{
	public class App
	{
		/// <summary>
		/// Asynchronously executes the main application logic.
		/// It reads configuration settings, processes the input data and outputs the results to a specified directory 
		/// as well as to the screen.
		/// </summary>
		/// <param name="args">Command-line arguments passed to the application, which includes the input file name.</param>
		/// <remarks>
		/// The method performs the following operations:
		/// 1. Reads the initial settings for the application from the "appsettings.json".
		/// 2. Creates a directory for the output file ("output.txt"), which is given in configuration settings.
		/// 3. Logs the start and successful completion of the application to "logInfo.txt"
		///    or an error if an exception occurs to "logError.txt".
		/// 4. Processes the input file specified by command-line arguments to "input.txt".
		///    The input file must be located in the "InputFiles" directory, which is specified in the configuration settings.
		/// 5. Applies discount rules to the input data using given discount schemas.
		/// 6. Writes processed data to "output.txt" in the specified result directory.
		/// </remarks>
		public async Task RunAsync(string[] args)
		{
			var configProvider = new ConfigurationProvider(new FileReader("appsettings.json"));
			var config = await configProvider.GetAsync();

			//creates a directory in the main project folder. The name of this directory is specified in the appsettings.json file
			//under the ResultDirectory setting. For example, if ResultDirectory is set to 'ResultFiles', this code line will create
			//a folder with that specific name, where files like 'output.txt' can be stored:
			Directory.CreateDirectory(config.ResultDirectory);

			var logger = new Logger("logInfo.txt", "logError.txt");

			try
			{
				await logger.LogInfoAsync("Starting RunAsync method.");

				var initialData = config.InitialData;
				var prices = config.Prices;

				//"input.txt" is passed through arguments from the RunAsync to the Main method of Program class 
				var inputFileName = args.Length > 0 ? args[0] : "input.txt";

				//constructs full path where the input file should be located
				var inputFileFullPath = Path.Combine(config.InputDirectory, inputFileName);

				var reader = new FileReader(inputFileFullPath);

				//constructs full path where the output file should be located
				var outputFileFullPath = Path.Combine(config.ResultDirectory, "output.txt");

				var writer = new FileWriter(outputFileFullPath);

				//new disccounts schemas, which implements IDiscountShema, can be added here:
				var discountSchemas = new List<IDiscountSchema>
				{
					new LpLargeSizeDiscountSchema(),
					new SmallSizeDiscountSchema()
				};

				var processor = new DiscountProcessor(reader, discountSchemas);

				var lines = await processor.ProcessAsync(config);

				await writer.WriteAsync(lines);

				foreach (var line in lines)
				{
					Console.WriteLine(line);
				}

				await logger.LogInfoAsync("RunAsync method execution completed successfully.");
			}
			catch (Exception ex)
			{
				await logger.LogErrorAsync($"An error occurred while running RunAsync method: {ex.Message}");

				Console.WriteLine("An error occurred while running the application.");
			}
		}
	}
}
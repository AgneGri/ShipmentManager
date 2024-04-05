namespace ShipmentDataDiscountManager.Loggers
{
	public class Logger : ILogger
	{
		private readonly string _pathInfo;
		private readonly string _pathError;

		public Logger(string pathInfo, string pathError)
		{
			_pathInfo = pathInfo;
			_pathError = pathError;
		}

		/// <summary>
		/// Asynchronously logs information message to an info log file.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns></returns>
		public async Task LogInfoAsync(string message)
		{
			await LogAsync(_pathInfo, message);
		}

		/// <summary>
		/// Asynchronously logs error message to an error log file.
		/// </summary>
		/// <param name="message">The message to be logged.</param>
		/// <returns></returns>
		public async Task LogErrorAsync(string message)
		{
			await LogAsync(_pathError, message);
		}

		/// <summary>
		/// Asynchronously writes a given message with a date stamp
		/// to the file at the specified path.
		/// Appends the message to the end of the file.
		/// </summary>
		/// <param name="path">The path of the file to which the message will be written.</param>
		/// <param name="message">The message to be logged.</param>
		/// <returns></returns>
		public async Task LogAsync(string path, string message)
		{
			using (StreamWriter writer = new StreamWriter(path, true))
			{
				await writer.WriteLineAsync($"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}");
			}
		}
	}
}
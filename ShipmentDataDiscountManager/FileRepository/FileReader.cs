namespace ShipmentDataDiscountManager.FileRepository
{
	public class FileReader : IFileReader
	{
		private readonly string _path;

		public FileReader(string path)
		{
			_path = path;
		}

		/// <summary>
		/// Asynchronously reads all lines from the file specified by the path.
		/// </summary>
		/// <returns>A list of strings, where each string represents each line from the file.</returns>
		public async Task<List<string>> GetDataLinesAsync()
		{
			return (await File.ReadAllLinesAsync(_path)).ToList();
		}

		/// <summary>
		/// Asynchronously reads all text from the file specified by the path.
		/// </summary>
		/// <returns>A string containing all text from the file.</returns>
		public async Task<string> GetAllTextAsync()
		{
			return await File.ReadAllTextAsync(_path);
		}
	}
}
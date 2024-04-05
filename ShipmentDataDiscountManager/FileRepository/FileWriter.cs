namespace ShipmentDataDiscountManager.FileRepository
{
	public class FileWriter : IFileWriter
	{
		private readonly string _path;

		public FileWriter(string path)
		{
			_path = path;
		}

		/// <summary>
		/// Asynchronously writes a list of strings to the file specified by the path, 
		/// where each string represents a new line.
		/// </summary>
		/// <param name="data">The list of strings.</param>
		/// <returns></returns>
		public async Task WriteAsync(List<string> data)
		{
			await File.WriteAllLinesAsync(_path, data);
		}
	}
}
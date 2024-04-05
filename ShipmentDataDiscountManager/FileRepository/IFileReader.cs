namespace ShipmentDataDiscountManager.FileRepository
{
	public interface IFileReader
	{
		Task<List<string>> GetDataLinesAsync();

		Task<string> GetAllTextAsync();
	}
}
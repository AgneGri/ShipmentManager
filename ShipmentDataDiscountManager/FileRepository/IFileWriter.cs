namespace ShipmentDataDiscountManager.FileRepository
{
	public interface IFileWriter
	{
		Task WriteAsync(List<string> data);
	}
}
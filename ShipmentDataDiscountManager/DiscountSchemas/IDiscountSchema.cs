namespace ShipmentDataDiscountManager.DiscountSchemas
{
	public interface IDiscountSchema
	{
		string Apply(
			DateOnly date,
			string size,
			string provider,
			decimal providersPriceValue,
			decimal lowestPriceValue,
			ref decimal totalDiscountPerMonth,
			ref int counter
		);
	}
}
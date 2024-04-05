namespace ShipmentDataDiscountManager.DiscountSchemas
{
	public class SmallSizeDiscountSchema : IDiscountSchema
	{
		/// <summary>
		/// Applies a discount based on the lowest price of "S" size packages among different providers. 
		/// It tracks the total discount given per month and applies only a partial discount if funds are insufficient.
		/// </summary>
		/// <param name="date">The date the package was processed, used to track total discount per month.</param>
		/// <param name="size">The size of the package. The discount is applied only to "S" size packages.</param>
		/// <param name="provider">The name of the provider.</param>
		/// <param name="providersPriceValue">The price set by the provider for the package size. </param>
		/// <param name="lowestPriceValue">The lowest price found among providers for the "S" size packages.</param>
		/// <param name="totalDiscountPerMonth">The sum of discounts given in the current month, tracked for applying partial or full discounts.</param>
		/// <param name="counter">The parameter is not used in calculation and remains set to zero.</param>
		/// <returns>A string representing the package details and indicating whether a partial or full discount was applied, 
		/// depending on the total discount available for the month.</returns>
		public string Apply(
			DateOnly date,
			string size,
			string provider,
			decimal providersPriceValue,
			decimal lowestPriceValue,
			ref decimal totalDiscountPerMonth,
			ref int counter)
		{
			if (size != "S")
			{
				return null;
			}
			
			var discountAmount = providersPriceValue - lowestPriceValue;

			var appliedDiscount = Math.Min(discountAmount, totalDiscountPerMonth);

			if (appliedDiscount > 0) //visi kiti praeina, nes appliedDiscount tampa 0, kai virsuje var appliedDiscount = 1.50-1.50=0
			{
				totalDiscountPerMonth -= appliedDiscount; //kai totaldiscount = 0.10, cia jis pasidaro lygus 0 ir tada skaiciuojant finalPrice=2-0.10=1.90.

				var finalPrice = providersPriceValue - appliedDiscount;

				return $"{date:yyyy-MM-dd} {size} {provider} {finalPrice:0.00} {appliedDiscount:0.00}";
			}

			return $"{date:yyyy-MM-dd} {size} {provider} {providersPriceValue:0.00} -";
		}
	}
}
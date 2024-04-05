namespace ShipmentDataDiscountManager.DiscountSchemas
{
	public class LpLargeSizeDiscountSchema : IDiscountSchema
	{
		private int lastProcessedMonth = -1;
		private bool discountAppliedThisMonth = false;

		/// <summary>
		/// Applies a discount to every third L size package from the LP provider within a month, but only once per month.
		/// Resets a discount at the start of each new month.
		/// </summary>
		/// <param name="date">The date the package was processed, used to track monthly discounts.</param>
		/// <param name="size">the size of the package. The discount is applied only to "L" size package.</param>
		/// <param name="provider">the name of the provider. The discount is valid only for the "LP" provider.</param>
		/// <param name="providersPriceValue">the price set by the provider for the package size. </param>
		/// <param name="lowestPriceValue">the lowest price found among providers for similar package size.</param>
		/// <param name="totalDiscountPerMonth">the total discount given in the current month, updated when applicable.</param>
		/// <param name="counter">the count of processed L packages from the LP provider.</param>
		/// <returns>A string representing the package details and indicating whether a discount was applied.</returns>
		public string Apply(
			DateOnly date,
			string size,
			string provider,
			decimal providersPriceValue,
			decimal lowestPriceValue,
			ref decimal totalDiscountPerMonth,
			ref int counter)
		{
			if (size != "L" || provider != "LP") //sito negalime perrasyti i viena eilute "ternary operator", nes graziname null, norint padaryti su viena eilute turetume rasyti null : (else), bet cia negalim, nes gi null
			{
				return null;
			}

			if (date.Month != lastProcessedMonth)
			{
				discountAppliedThisMonth = false;

				lastProcessedMonth = date.Month;
			}

			counter++;

			if (counter % 3 == 0 && !discountAppliedThisMonth)
			{
				discountAppliedThisMonth = true;

				totalDiscountPerMonth -= providersPriceValue;

				if (totalDiscountPerMonth < 0) totalDiscountPerMonth = 0; //cia galima parasyti taip, nes mes pakeiciame kintamojo verte, nebereikia else 

				//if (totalDiscountPerMonth < 0)
				//{
				//	totalDiscountPerMonth = 0;
				//}


				return $"{date.ToString("yyyy-MM-dd")} {size} {provider} 0.00 {providersPriceValue:0.00}";
			}

			return $"{date.ToString("yyyy-MM-dd")} {size} {provider} {providersPriceValue:0.00} -";
		}
	}
}
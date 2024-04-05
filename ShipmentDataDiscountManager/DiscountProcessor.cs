using ShipmentDataDiscountManager.DiscountSchemas;
using ShipmentDataDiscountManager.FileRepository;
using ShipmentDataDiscountManager.Models;

namespace ShipmentDataDiscountManager
{
	public class DiscountProcessor
	{
		private readonly IFileReader _reader;
		private readonly List<IDiscountSchema> _discountSchemas;

		public DiscountProcessor(
			IFileReader reader,
			List<IDiscountSchema> discountSchemas)
		{
			_reader = reader;
			_discountSchemas = discountSchemas;
		}

		/// <summary>
		/// Asynchronously processes read lines to verify data validity and formats them into a predefined structure.
		/// Monitors and tracks total discounts and counts of the current month, resetting these values at the start of each new month.
		/// Retrieves each package's price and the lowest price among the providers from initial data, 
		/// and applies available discounts according to predefined schemas.
		/// </summary>
		/// <returns>A list of processed lines with applied discounts and formatted data.</returns>
		public async Task<List<string>> ProcessAsync(Configuration configuration)
		{
			var totalDiscountPerMonth = configuration.InitialData.MonthlyDiscount;
			var counter = configuration.InitialData.Counter;
			var currentMonth = configuration.InitialData.CurrentMonth;
			var lines = new List<string>();

			var inputLines = await _reader.GetDataLinesAsync();

			foreach (var line in inputLines)
			{
				var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

				if (!DateOnly.TryParse(parts[0], out var date) || parts.Length != 3)
				{
					lines.Add($"{line} Ignored");

					continue;
				}

				if (date.Month != currentMonth)
				{
					currentMonth = date.Month;
					totalDiscountPerMonth = configuration.InitialData.MonthlyDiscount;
					counter = configuration.InitialData.Counter;
				}

				ApplySchemas(
					lines,
					date,
					parts[1],
					parts[2],
					ref totalDiscountPerMonth,
					ref counter,
					configuration
				);

			}
			return lines;
		}

		/// <summary>
		/// Applies discount schemas to a list of data lines based on the rules given
		/// in the classes, which implements IDiscountSchema.
		/// </summary>
		/// <param name="lines">An empty data lits to which the resulting lines after discount calculations will be added.</param>
		/// <param name="date">The date represents the current data line, used for discount calculations.</param>
		/// <param name="size">The size of the package to which the discount will be applied.</param>
		/// <param name="provider">The name of the provider to which the discount will be applied.</param>
		/// <param name="totalDiscountPerMonth">The total discount applied for the current month and is updated if the 
		/// discounts, according to given discount schema, are calculated.</param>
		/// <param name="counter">The count of transactions that have been processed and used for certain discount calculations.</param>
		/// <param name="configuration">The configuration settings containing price and initial data.</param>
		/// <returns>A list of data lines with applied discounts.</returns
		private List<string> ApplySchemas(
			List<string> lines,
			DateOnly date,
			string size,
			string provider,
			ref decimal totalDiscountPerMonth,
			ref int counter,
			Configuration configuration)
		{
			var providersPriceValue = FetchProvidersPriceValue(provider, size, configuration);

			var lowestPriceValue = FetchLowestPriceValueForSize(size, configuration);

			var discountApplied = false;

			foreach (var schema in _discountSchemas)
			{
				var resultLine = schema.Apply(
					date,
					size,
					provider,
					providersPriceValue,
					lowestPriceValue,
					ref totalDiscountPerMonth,
					ref counter
				);

				if (resultLine != null)
				{
					lines.Add(resultLine);

					discountApplied = true;
					break;
				}
			}

			if (!discountApplied)
			{
				var formattedLine = $"{date.ToString("yyyy-MM-dd")} {size} {provider} {providersPriceValue:0.00} -";

				lines.Add(formattedLine);
			}

			return lines;
		}

		/// <summary>
		/// Finds the price value of a given provider based on its name and the package size.
		/// </summary>
		/// <param name="provider">The name of the provider to find the price for.</param>
		/// <param name="size">The size of the package to find the price for.</param>
		/// <returns>The price value for the specified provider and package size. 
		/// Returns zero if no matching price is found.</returns>
		private decimal FetchProvidersPriceValue(string provider, string size, Configuration configuration)
		{
			var priceEntry = configuration
				.Prices
				.FirstOrDefault(p => p.Provider == provider && p.Size == size);

			return priceEntry?.PriceValue ?? 0m;

			//if (priceEntry != null)
			//{
			//	return priceEntry.PriceValue;
			//}
			//else
			//{
			//	return 0m;
			//}
		}

		/// <summary>
		///  Finds the lowest price value for a specific package size among different providers.
		/// </summary>
		/// <param name="size">The size of the package to find the lowest price for.</param>
		/// <returns>The lowest price value for the specified package size.
		/// Returns zero if not matching package size is found.</returns>
		private decimal FetchLowestPriceValueForSize(string size, Configuration configuration)
		{
			var pricesForSize = configuration
				.Prices
				.Where(p => p.Size == size)
				.Select(p => p.PriceValue);

			if (!pricesForSize.Any())
			{
				return 0m;
			}

			return pricesForSize.Min();
		}
	}
}
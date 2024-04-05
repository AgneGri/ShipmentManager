# ShipmentDataDiscountManager
## Description

This C# console application is designed to optimize shipping costs by processing package data for different sizes (S, M, L) from various providers such as provider1 (MR) and provider2 (LP). Initially, the program reads from an input file containing information about packages and their respective providers, with entries like '2015-02-01 S MR', indicating the date, size, and provider. It then applies discounts to ensure the lowest rates for small packages and offers a monthly free large package via LP, while ensuring that the total discount does not exceed 10 Eur per month. The processed data, including adjusted costs per package, are outputted to 'output.txt'.

## Installation

Ensure .NET 8 is installed on your machine. If not, download and install from the official [.NET website](https://dotnet.microsoft.com/download/dotnet/8.0).

To set up the project:
1. Clone the repository: `git clone https://github.com/AgneGri/ShipmentManager.git`
2. Before runing the app, make sure that the needed files are present at the following directories:
  1. confuguration file `appsettings.json` at: `cd ShipmentDataDiscountManager`
  2. initial shipment data file `input.txt` at: `cd ShipmentDataDiscountManager/InputFiles`

Note: if one or more files is missing, see the section configuration.

3. Run the app by navigating to the main project directory:
`cd ShipmentDataDiscountManager`

and execute:
`dotnet run input.txt`

After running the application, the output file (`output.txt`) can be found in the following directory:
`cd ShipmentDataDiscountManager/ResultFiles`

After running the application, the log files (`logInfo.txt` or `logErrors.txt`) can be found in the following directory:
`cd ShipmentDataDiscountManager`

## Testing

To run the tests, navigate to the tests directory:
`cd ShipmentDataDiscountManager.Tests`

and execute:
`dotnet test`

## Dependencies

1. .NET 8
2. System.Text.Json for JSON deserialization
3. Moq for unit testing (if applicable)

## Configuration

If one or more files are missing from the project, you need to prepare the necessary configuration files: `appsettings.json` and `input.txt`.

1. `appsettings.json` should contain initial settings, pricing information and paths to initial directories necessary for the application. Replace example values with your actual data:

```json
{
  "InitialData": {
    "monthlyDiscount": "YourMonthlyDiscountAmount",
    "counter": "YourCounterStartValue",
    "currentMonth": "YourStartMonthNumber"
  },
  "Prices": [
    {
      "provider": "YourProviderID",
      "size": "YourPackageSize",
      "priceValue": "YourPriceAmount"
    }
    // Add more pricing information as needed
  ],
  "InputDirectory": "InputFiles",
  "ResultDirectory": "ResultFiles"
}
```

2. `input.txt` should contain data that the application will process. Each valid line should follow this format: "date size provider".
3. Place the `appsettings.json` and `input.txt` files in the project's directories:
1. confuguration file `appsettings.json` at: `ShipmentDataDiscountManager`
2. initial shipment data file `input.txt` at: `ShipmentDataDiscountManager/InputFiles`

## Architecture

The ShipmentDataDiscountManager was structured to maintain a clear separation of concerns and ensure flexibility, keeping in mind that the design should easily accommodate new rules and modifications to existing ones. In order to achieve this goal, the interfaces and easy initial data setup were created.

### Key Components

1. `The Program`: This class serves as the entry point in the application, because it is called when the program is executed and runs the instance of the `App`. I added the logging as it plays a crucial part in documenting the behaviour of the program (success or errors) and it provides clues to what caused the errors.
2. `App`: This class initializes core components like data deserialization, manages the creation of needed directories, file reading and writing, and coordinates the discount calculation process. It is logged as well.
3. `ConfigurationProvider`: To simplify the configuration and ensure clarity in data management, all initial settings are stored in the `appsettings.json` file. This setup prevents confusion regarding where changes need to be made. Thus, this class is essential for loading and parsing the application's settings from the configuration file.
4. `The DiscountProcessor`: This class is responsible for the core logic of discount application. It takes input data, verifies it and applies discount rules according to IDiscountSchema implementations. The class tracks the discounts applied per month and ensures they do not exceed set limits, adjusting discount values based on package size and provider, thereby producing a list of processed lines ready for output. 

#### Interfaces

All interfaces were created in order to make the app more flexible for future expansion as well as for easier unit testing.

1. `IDiscountSchema`: plays a key part in this app as it can apply adaptable discount calculation strategies for different package sizes and providers. The method `Apply` within this interface encapsulates the logic for calculating discounts, enabling the system to easily introduce new providers or package sizes without disrupting the existing infrastructure. This adaptability is particulary ilustrated by `LpLargeSizeDiscountSchema` and `SmallSizeDiscountSchema` classes, which implement the IDiscountSchema:
`LpLargeSizeDiscountSchema` class is designed for the calculation of 'LP' large package discounts, applying a free shipment for every third large package monthly with resets at each month's start.
`SmallSizeDiscountSchema` class ensures that the smallest packages always benefit from the lowest available price among providers, adjusting for monthly discount limits.
2. `IFileReader` and `IFileWriter`: serves the same purpose as previous interfaces - scalability and also easy testability. Both interfaces allow addition of new reading or writing methods.
3. `IConfigurationProvider`: This interface allows the application to asynchronously load settings from a configuration file.
4. `ILogger`:  This interface allows a flexibility in adding different level of logging within the app. It plays a crucial role in documenting information and errors, ensuring the application's maintainability and easier debugging.

#### Testing

Unit tests have been conducted using MSTest and the Moq framework, ensuring the reliability and correctness of key components. `DiscountProcessorTests` check that discounts are correctly calculated and applied, reflecting the expected behavior under varied scenarios. Tests for `LpLargeSizeDiscountSchema` and `SmallSizeDiscountSchema` validate the discount application for different package sizes and providers, confirming adherence to business rules. These tests also ensure that edge cases are handled properly and monthly discount limits are respected. LoggerTests ensure that the logging functionality accurately records information and errors once per event. 

### Future Considerations

The handling of any exceptions is presented. Enhancements can be made by integrating specific exception types across various classes as needed for more precise error handling. Also, more test cases might be covered, e. g. verify if any exception might occur and be registered. The folder `InputFiles` was created for future expansion as the app could be modified to read more than one text file asynchroniously.


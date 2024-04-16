# RainfallApi

An OpenAPI implementation of
the [Environment Agency's Rainfall API](https://environment.data.gov.uk/flood-monitoring/doc/rainfall).
The API provides access to rainfall data from the Environment Agency's monitoring stations.

## Introduction

The Environment Agency has approximately 1000 real time rain gauges which telemetry connects.
Measurements of the amount of precipitation (mm) are captured in Tipping Bucket Raingauges (TBR).
The data reported here gives accumulated totals for each 15-min period.
The data is typically transferred once or twice per day.

The Rainfall API provides access to these rainfall measurements, and to information on the monitoring stations providing
those measurements.
It is compatible with (and integrated into) the API for water level/flow readings.

Note that for information protection reasons, the rainfall monitoring stations do not have names and their geographic
location has been reduced to a 100m grid.

## API Summary

The API provides access to the following resources:

| Description                                              | Endpoint                            | Parameters |
|----------------------------------------------------------|-------------------------------------|------------|
| Retrieve the latest readings for the specified stationId | `/rainfall/id/{stationId}/readings` | `count`    |

## How to use the API

### Requirements

- [Git](https://git-scm.com/) for cloning the repository
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download) for running the API

1. Clone the repository
    ```shell
    git clone https://github.com/sabihoshi/RainfallApi
    cd RainfallApi
    ```

2. Run the API on your local machine (HTTPS)
    ```shell
    dotnet run --project ./RainfallApi/RainfallApi.csproj --launch-profile "https"
    ```

This will start the API on `https://localhost:3000`.
You can access the API documentation
at `https://localhost:3000/swagger/index.html` which will be opened automatically in your default browser.

## Running the tests

The API has a suite of unit tests that can be run using the following command in the root directory of the repository:

```shell
dotnet test
```

## Acknowledgements

### License

- This project is licensed under the MIT License—see the [LICENSE](LICENSE.md) file for details.
- This project uses third-party libraries or other resources that may be
  distributed under [different licenses](THIRD-PARTY-NOTICES.md).

### Environment Agency Rainfall API

This project uses Environment Agency rainfall data from the real-time data API (Beta)
under the [Open Government Licence v3.0](https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/).
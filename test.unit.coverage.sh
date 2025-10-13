#!/bin/bash

# Run the domain unit tests with coverage
dotnet test tests/Booking.Tests.Unit/Booking.Tests.Unit.csproj \
  --collect:"XPlat Code Coverage" \
  /p:Include="[Booking.Domain]*"

# Generate HTML report from the coverage file
reportgenerator \
  -reports:tests/Booking.Tests.Unit/TestResults/*/coverage.cobertura.xml \
  -targetdir:tests/Booking.Tests.Unit/CoverageReport \
  -reporttypes:Html
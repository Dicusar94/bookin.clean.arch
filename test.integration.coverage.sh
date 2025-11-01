#!/bin/bash

# Run integration tests with coverage
dotnet test tests/Booking.Tests.Integration/Booking.Tests.Integration.csproj \
  --collect:"XPlat Code Coverage" \
  /p:Include="[Booking.Web]*[Booking.Application]*[Booking.Infrastructure]*"

# Generate HTML report from the coverage file
reportgenerator \
  -reports:tests/Booking.Tests.Integration/TestResults/*/coverage.cobertura.xml \
  -targetdir:tests/Booking.Tests.Integration/CoverageReport \
  -reporttypes:Html
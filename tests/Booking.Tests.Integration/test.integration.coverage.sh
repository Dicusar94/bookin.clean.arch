#!/bin/bash
set -e
set -o pipefail

PROJECT="Booking.Tests.Integration.csproj"
REPORT_DIR="CoverageReport"

echo "🧪 Running integration tests with coverage..."
dotnet test "$PROJECT" \
  --settings ../coverage.integration.runsettings \
  --collect:"XPlat Code Coverage" \
  --results-directory "$REPORT_DIR" \
  -p:ExcludeByFile="**/Migrations/*" \
  -v minimal

echo "🧹 Cleaning old reports..."
rm -rf "$REPORT_DIR/Html"

echo "📊 Merging and generating HTML report..."
reportgenerator \
  -reports:"$REPORT_DIR"/*/coverage.cobertura.xml \
  -targetdir:"$REPORT_DIR"/Html \
  -reporttypes:"Html;Cobertura" \
  "-assemblyfilters:-Booking.Domain;-Booking.Tests*;-Booking.Core"

# 🧮 Extract percentage from merged Cobertura XML
COVERAGE_FILE="$REPORT_DIR/Html/Cobertura.xml"
COVERAGE=$(grep 'line-rate=' "$COVERAGE_FILE" \
  | sed -E 's/.*line-rate="([0-9.]+)".*/\1/' \
  | head -1 \
  | awk '{printf "%.2f", $1*100}')

echo "✅ Integration coverage report generated at:"
echo "file://$(pwd)/$REPORT_DIR/Html/index.html"

# 🚦 Enforce minimum coverage threshold
MIN=75
echo "🔍 Integration coverage: ${COVERAGE}% (minimum ${MIN}%)"
if (( $(echo "$COVERAGE < $MIN" | bc -l) )); then
  echo "❌ Integration coverage $COVERAGE% is below threshold $MIN%"
  exit 1
else
  echo "✅ Integration coverage check passed."
fi
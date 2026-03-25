#!/bin/bash
# Fetch USAePay docs via Jina Reader API
#
# Usage:
#   JINA_API_KEY="your-key" ./scripts/fetch-usaepay-docs.sh
#
# This script fetches the USAePay REST API documentation and converts it to
# well-formatted markdown using Jina Reader.

set -e

# Primary documentation pages
PAGES=(
  # REST API
  "https://help.usaepay.info/api/rest/"
  "https://help.usaepay.info/developer/rest-api/"
  "https://help.usaepay.info/developer/rest-api/transactions/processing/sale/"
  "https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/"
  "https://help.usaepay.info/developer/rest-api/customers/paymethods/"
  "https://help.usaepay.info/developer/rest-api/more/commonobjects/"
  "https://help.usaepay.info/developer/rest-api/more/public-api-key/"
  "https://help.usaepay.info/developer/rest-api/changelog/"

  # Reference (complete)
  "https://help.usaepay.info/developer/reference/avs/"
  "https://help.usaepay.info/developer/reference/cardlevelcodes/"
  "https://help.usaepay.info/developer/reference/checkformat/"
  "https://help.usaepay.info/developer/reference/currencycodes/"
  "https://help.usaepay.info/developer/reference/cvv2/"
  "https://help.usaepay.info/developer/reference/e2e/"
  "https://help.usaepay.info/developer/reference/errorcodes/"
  "https://help.usaepay.info/developer/reference/existingdebt/"
  "https://help.usaepay.info/developer/reference/giftcards/"
  "https://help.usaepay.info/developer/reference/highavailability/"
  "https://help.usaepay.info/developer/reference/invoice_terms/"
  "https://help.usaepay.info/developer/reference/level3/"
  "https://help.usaepay.info/developer/reference/multicurrency"
  "https://help.usaepay.info/developer/reference/publickeys"
  "https://help.usaepay.info/developer/reference/sandbox/"
  "https://help.usaepay.info/developer/reference/statuscodes/"
  "https://help.usaepay.info/developer/reference/testcards/"
  "https://help.usaepay.info/developer/reference/testcheckdata/"
  "https://help.usaepay.info/developer/reference/timeouts/"
  "https://help.usaepay.info/developer/reference/tokenization/"
  "https://help.usaepay.info/developer/reference/transactioncodes/"
  "https://help.usaepay.info/developer/reference/transid/"
  "https://help.usaepay.info/developer/reference/umcodes/"
  "https://help.usaepay.info/developer/reference/vpas_msc/"

  # Webhooks
  "https://help.usaepay.info/merchant/guide/settings/webhooks/"
)

OUTPUT_DIR="docs"

# Change to project root
cd "$(dirname "$0")/.."

if [ -z "$JINA_API_KEY" ]; then
  echo "Error: JINA_API_KEY environment variable is required"
  echo "Usage: JINA_API_KEY=\"your-key\" ./scripts/fetch-usaepay-docs.sh"
  exit 1
fi

mkdir -p "$OUTPUT_DIR"

# Clear previous combined output
COMBINED="$OUTPUT_DIR/usaepay-api-reference.md"
> "$COMBINED"

echo "Fetching USAePay documentation (${#PAGES[@]} pages)..."

for URL in "${PAGES[@]}"; do
  # Derive a filename-safe slug from the URL
  SLUG=$(echo "$URL" | sed 's|https://help.usaepay.info/||' | sed 's|/$||' | tr '/' '-')
  OUTFILE="$OUTPUT_DIR/${SLUG}.md"

  echo "  Fetching: $URL"

  # api/rest/ uses Slate template (different HTML structure) — no remove selectors
  # help.usaepay.info/developer/ and /merchant/ use Bootstrap — strip nav/sidebar
  if [[ "$URL" == *"api/rest"* ]]; then
    curl -sS "https://r.jina.ai/$URL" \
      -H "Authorization: Bearer $JINA_API_KEY" \
      -H "X-Retain-Images: none" \
      -H "X-Return-Format: markdown" \
      -H "X-Md-Link-Style: referenced" \
      -H "X-Engine: browser" \
      -H "X-Timeout: 120" \
      -o "$OUTFILE"
  else
    curl -sS "https://r.jina.ai/$URL" \
      -H "Authorization: Bearer $JINA_API_KEY" \
      -H "X-Remove-Selector: .navbar, footer, .bs-sidebar, .col-md-3, .navbar-header, .navbar-collapse" \
      -H "X-Retain-Images: none" \
      -H "X-Return-Format: markdown" \
      -H "X-Md-Link-Style: referenced" \
      -H "X-Engine: browser" \
      -H "X-Timeout: 60" \
      -o "$OUTFILE"
  fi

  # Append to combined reference with separator
  echo -e "\n\n---\n" >> "$COMBINED"
  cat "$OUTFILE" >> "$COMBINED"

  echo "    Saved to: $OUTFILE ($(wc -c < "$OUTFILE" | tr -d ' ') bytes)"

  # Be polite to the API
  sleep 1
done

echo ""
echo "Combined reference saved to: $COMBINED"
echo "Total size: $(wc -c < "$COMBINED" | tr -d ' ') bytes"
echo "Total lines: $(wc -l < "$COMBINED" | tr -d ' ')"

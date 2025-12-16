#!/bin/bash

# Script to test the Product Processing API

echo "=== Product Processing API Test ==="
echo ""

# Use port 5000 to match docker-compose configuration
BASE_URL="http://localhost:5000"

echo "1. Testing GET /api/products (should be empty initially)"
curl -s $BASE_URL/api/products | python3 -m json.tool
echo ""

echo "2. Creating a new product"
PRODUCT1=$(curl -s -X POST $BASE_URL/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell Inspiron 15",
    "description": "Laptop with 16GB RAM and 512GB SSD",
    "price": 3500.00,
    "stock": 15
  }')
echo $PRODUCT1 | python3 -m json.tool
echo ""

echo "3. Creating another product"
curl -s -X POST $BASE_URL/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Mouse Logitech MX Master 3",
    "description": "Wireless mouse with precision scrolling",
    "price": 450.00,
    "stock": 30
  }' | python3 -m json.tool
echo ""

echo "4. Listing all products"
curl -s $BASE_URL/api/products | python3 -m json.tool
echo ""

echo "5. Getting product by ID (ID=1)"
curl -s $BASE_URL/api/products/1 | python3 -m json.tool
echo ""

echo "6. Calculating 20% discount for product ID=1"
curl -s "$BASE_URL/api/products/1/discount?percentage=20" | python3 -m json.tool
echo ""

echo "7. Updating product ID=1"
curl -s -X PUT $BASE_URL/api/products/1 \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Notebook Dell Inspiron 15 (Updated)",
    "description": "Laptop with 32GB RAM and 1TB SSD",
    "price": 4200.00,
    "stock": 12
  }' | python3 -m json.tool
echo ""

echo "8. Verifying the update"
curl -s $BASE_URL/api/products/1 | python3 -m json.tool
echo ""

echo "9. Deleting product ID=1"
curl -s -X DELETE $BASE_URL/api/products/1 -w "\nHTTP Status: %{http_code}\n"
echo ""

echo "10. Verifying deletion"
curl -s $BASE_URL/api/products | python3 -m json.tool
echo ""

echo "=== Test completed ==="

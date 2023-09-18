# cuddly-rotary-phone

## Products API Demo

Simple base template API Project to use for a Peer Programming Session

The API should allow users to:
  - Create products (/api/products)
  - List all products (/api/products)
  - Search for products based on their name (/api/products/search?name=<searchTerm>)
  - Update only the description by Id (/api/products/?Id=<searchTerm>)

Other non-functional requirements:
  - Implement API versioning
  - Implement Documentation for the API
  - Ensure proper error handling and validation for the API endpoints
  - Write an example unit and integration test to verify the functionality of your code
  - Product id while creating a product is mandatory
  - Description of the product should not exceed 50 characters
  - Use an in-memory data storage mechanism for simplicity (e.g., a list or dictionary)
  - Focus on writing clean, maintainable, and testable code

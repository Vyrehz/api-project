## Initial commit
- Restore nuget packages
- Correct Assembly name and Default namespace from refactor-me to refactor-this
- Update Newtonsoft.Json from 8.0.3 to Latest stable 13.0.3 due to high severity vulnerability alert
- Add api prefix to the RoutePrefix

## Second commit
- Refactoring SQL Connections and Commands to use Using Statements. This ensures that all SQL-related objects are properly disposed of, reducing the risk of memory leaks and ensures that database connections are closed appropriately
- Also changed ExecuteReader to ExecuteNonQuery for the Delete method.ExecuteReader is not suitable for deletion since it's intended use is to retrieve data. ExecuteNonQuery is used for INSERT, UPDATE and DELETE queries, I.E queries that are intended for modifying data, not retrieving it.
- Fixed mistake I had made including a / in front of the api/products prefix

## Third commit
- Used SQL Parameters to prevent SQL injection attacks. This ensures that the values are properly escaped and treated as values rather than as part of the SQL command text

## Fourth commit
- Added extra methods for LoadProductOption and LoadProduct to avoid passing in where clauses and reducing code complexity. This came at the cost of code duplication but resulted in better readabiltiy

## Fifth commit
- Added a service layer with dependency injection using Unity to achieve IoC
- For now starting with a basic implementation of the GetAll method from Products
- Used RegisterType rather than RegisterSingleton for now as the application is not yet thread safe
- Used the new ProductService as an intermediary layer between the controller and database
- For now omitting a ProductOptionService and Product/Options Repositories (coming in later commits)

## Sixth commit
- Added new ProductOptionService 
- Used this new service in the controller

## Seventh commit
- Separate Product and ProductOption into distinct classes. This aligns with best practices in software development (SOLID), leading to a codebase that is more maintainable, understandable, and scalable. It also enhances the ability to test, reuse, and evolve parts of the system independently
- By separating these classes, each class adheres more closely to the Single Responsibility Principle. This is one of the SOLID principles of object-oriented design. Essentially a class should only have one reason to change and a single job/responsibility

## Eighth commit
- Added a repository layer for both Products and ProductOptions
- Removed Products and ProductOptions classes and replaced with Enumerables
- Use Repositories in both Services

## Ninth commit
- Added error handling for both repositories
- Allows greater visibility for any SQL errors that occur (through the use of logging)
- Also added null argument checks for all functions with non-GUID arguments

## Tenth commit
- Simplified SQL query for getting all products and all productOptions by id
- Replaced SELECT id with SELECT * and then set values accordingly
- This removed multiple extra calls to getById
- Also ensured the option get queries reference the productId

## Eleventh commit
- Added ApiKeyAuthentication
- In future this could be extended for generated API keys but for now I have hardcoded a test key directly into the check function
- This can be tested using postman and adding a Bearer token with the value: test_api_key

## Twelfth commit
- Added Upsert function to both services for use in the "Update" endpoint
- I followed this pattern from the original implementation and made the assumption that Update should also Upsert

## Thirteenth commit
- Added Unit tests project
- Added tests for Product only endpoints (excluding options for now)
- Used async and await operations for Product service and repo to provide thread safe functions
- Allows for scalability by not blocking threads when waiting for I/O operations (database calls etc)

## Fourteenth commit
- Added tests for ProductOption endpoints
- Used async and await operations for ProductOption service and repo to provide thread safe functions

## Fifteenth commit
- Added tests for both services
- Also changed ConnectionHelper to use an interface
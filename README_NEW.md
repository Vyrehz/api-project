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
## Initial commit
- Restore nuget packages
- Correct Assembly name and Default namespace from refactor-me to refactor-this
- Update Newtonsoft.Json from 8.0.3 to Latest stable 13.0.3 due to high severity vulnerability alert
- Add api prefix to the RoutePrefix

## Second commit
- Refactoring SQL Connections and Commands to use Using Statements. This ensures that all SQL-related objects are properly disposed of, reducing the risk of memory leaks and ensures that database connections are closed appropriately
- Also changed ExecuteReader to ExecuteNonQuery for the Delete method.ExecuteReader is not suitable for deletion since it's intended use is to retrieve data. ExecuteNonQuery is used for INSERT, UPDATE and DELETE queries, I.E queries that are intended for modifying data, not retrieving it.
- Fixed mistake I had made including a / in front of the api/products prefix
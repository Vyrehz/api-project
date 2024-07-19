using refactor_this.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using refactor_this.Exceptions;

namespace refactor_this.Repositories
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        private readonly IConnectionHelper _connectionHelper;

        public ProductOptionRepository(IConnectionHelper connectionHelper)
        {
            _connectionHelper = connectionHelper;
        }

        public async Task<IEnumerable<ProductOption>> GetAllByProductIdAsync(Guid productId)
        {
            var items = new List<ProductOption>();

            try
            {
                var productIdAsString = productId.ToString();

                if (string.IsNullOrEmpty(productIdAsString)) throw new ArgumentNullException(nameof(productIdAsString));

                using (var conn = _connectionHelper.NewConnection())
                {
                    const string sql = "SELECT * FROM productoption WHERE productid = @ProductId";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productIdAsString);

                        await conn.OpenAsync();

                        using (var rdr = await cmd.ExecuteReaderAsync())
                        {
                            while (await rdr.ReadAsync())
                            {
                                var option = new ProductOption
                                {
                                    IsNew = false, // Assuming IsNew is a flag you use to track new vs existing entities
                                    Id = Guid.Parse(rdr["Id"].ToString()),
                                    ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                                    Name = rdr["Name"].ToString(),
                                    Description = rdr["Description"] != DBNull.Value ? rdr["Description"].ToString() : null
                                };

                                items.Add(option);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message} while getting all Product Options by Product ID: {productId}.");

                throw new RepositoryException($"An SQL error occurred while getting all Product Options by Product ID: {productId}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while getting all Product Options by Product ID: {productId}.", ex);
            }

            return items;
        }

        public async Task<ProductOption> GetByIdAsync(Guid productId, Guid id)
        {
            try
            {
                var option = new ProductOption();

                using (var conn = _connectionHelper.NewConnection())
                {
                    const string sql = "SELECT * FROM productoption WHERE id = @Id AND productid = @ProductId";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@ProductId", productId);

                        await conn.OpenAsync();

                        using (var rdr = await cmd.ExecuteReaderAsync())
                        {
                            if (!await rdr.ReadAsync()) return option;

                            option.IsNew = false;
                            option.Id = Guid.Parse(rdr["Id"].ToString());
                            option.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                            option.Name = rdr["Name"].ToString();
                            option.Description =
                                (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                        }
                    }
                }

                return option;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message} while getting Product Option by Id: {id}.");

                throw new RepositoryException($"An SQL error occurred while getting Product Option by Id: {id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while getting Product Option by Id: {id}.", ex);
            }
        }

        public async Task AddAsync(ProductOption option)
        {
            try
            {
                if (option == null) throw new ArgumentNullException(nameof(option));

                using (var conn = _connectionHelper.NewConnection())
                {
                    const string sql = "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", option.Id);
                        cmd.Parameters.AddWithValue("@ProductId", option.ProductId);
                        cmd.Parameters.AddWithValue("@Name", option.Name);
                        cmd.Parameters.AddWithValue("@Description", option.Description ?? (object)DBNull.Value);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message} while adding Product Option: {option?.Id}.");

                throw new RepositoryException($"An SQL error occurred while adding Product Option: {option?.Id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while adding Product Option: {option?.Id}.", ex);
            }
        }

        public async Task UpdateAsync(ProductOption option)
        {
            try
            {
                if (option == null) throw new ArgumentNullException(nameof(option));

                using (var conn = _connectionHelper.NewConnection())
                {
                    const string sql = "UPDATE productoption SET name = @Name, description = @Description WHERE id = @Id";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", option.Id);
                        cmd.Parameters.AddWithValue("@ProductId", option.ProductId);
                        cmd.Parameters.AddWithValue("@Name", option.Name);
                        cmd.Parameters.AddWithValue("@Description", option.Description ?? (object)DBNull.Value);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message} while updating Product Option: {option?.Id}.");

                throw new RepositoryException($"An SQL error occurred while updating Product Option: {option?.Id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while updating Product Option: {option?.Id}.", ex);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            try
            {
                using (var conn = _connectionHelper.NewConnection())
                {
                    const string sql = "DELETE FROM productoption WHERE id = @Id";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message} while deleting Product Option: {id}.");

                throw new RepositoryException($"An SQL error occurred while deleting Product Option: {id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while deleting Product Option: {id}.", ex);
            }
        }
    }
}

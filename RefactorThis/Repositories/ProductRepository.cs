using refactor_this.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using refactor_this.Exceptions;
using System.Xml.Linq;

namespace refactor_this.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAll()
        {
            try
            {
                var items = new List<Product>();

                using (var conn = Helpers.NewConnection())
                {
                    string sql = "select id from product";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        conn.Open();

                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                // ToDo: cleanup
                                var id = Guid.Parse(rdr["id"].ToString());
                                var product = GetById(id);

                                items.Add(product);
                            }
                        }

                        conn.Close();
                    }
                }

                return items;
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} while getting all Products.");

                throw new RepositoryException($"An SQL error occurred while getting all Products.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while getting all Products.", ex);
            }
        }

        public IEnumerable<Product> GetByName(string name)
        {
            try
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                var lowerCaseName = name?.ToLower();

                var items = new List<Product>();

                using (var conn = Helpers.NewConnection())
                {
                    string sql = "select id from product where lower(name) like @Name";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", lowerCaseName);

                        conn.Open();

                        using (var rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                var id = Guid.Parse(rdr["id"].ToString());
                                var product = GetById(id);

                                items.Add(product);
                            }
                        }
                    }
                }

                return items;
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} while getting Product by name: {name}.");

                throw new RepositoryException($"An SQL error occurred while getting Product by name: {name}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while getting Product by name: {name}.", ex);
            }
        }

        public Product GetById(Guid id)
        {
            try
            {
                var product = new Product();

                using (var conn = Helpers.NewConnection())
                using (var cmd = new SqlCommand("SELECT * FROM product WHERE id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        if (!rdr.Read()) return product;

                        product.IsNew = false;
                        product.Id = Guid.Parse(rdr["Id"].ToString());
                        product.Name = rdr["Name"].ToString();
                        product.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                        product.Price = decimal.Parse(rdr["Price"].ToString());
                        product.DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                    }
                }

                return product;
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} while getting Product: {id}.");

                throw new RepositoryException($"An SQL error occurred while getting Product: {id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while getting Product: {id}.", ex);
            }
        }

        public void Add(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            try
            {
                using (var conn = Helpers.NewConnection())
                {
                    string sql = "INSERT INTO product (id, name, description, price, deliveryprice) VALUES (@Id, @Name, @Description, @Price, @DeliveryPrice)";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", product.Id);
                        cmd.Parameters.AddWithValue("@Name", product.Name);
                        cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Price", product.Price);
                        cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} while adding Product: {product.Id}.");

                throw new RepositoryException($"An SQL error occurred while adding Product: {product.Id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while adding Product: {product.Id}.", ex);
            }
        }

        public void Update(Product product)
        {
            try
            {
                if (product == null) throw new ArgumentNullException(nameof(product));

                using (var conn = Helpers.NewConnection())
                {
                    string sql = "UPDATE product SET name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice WHERE id = @Id";

                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", product.Id);
                        cmd.Parameters.AddWithValue("@Name", product.Name);
                        cmd.Parameters.AddWithValue("@Description", product.Description ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Price", product.Price);
                        cmd.Parameters.AddWithValue("@DeliveryPrice", product.DeliveryPrice);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} while updating Product: {product.Id}.");

                throw new RepositoryException($"An SQL error occurred while updating Product: {product.Id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while updating Product: {product.Id}.", ex);
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                using (var conn = Helpers.NewConnection())
                using (var cmd = new SqlCommand("DELETE FROM product WHERE id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                // I would use a logger if this was an application deployed on AWS or GCP
                Console.WriteLine($"SQL Error: {ex.Message} when deleting Product: {id}.");

                throw new RepositoryException($"An SQL error occurred while deleting Product: {id}.", ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw new RepositoryException($"An unexpected error occurred while deleting Product: {id}.", ex);
            }

        }
    }
}

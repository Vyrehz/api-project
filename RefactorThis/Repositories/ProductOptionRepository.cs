using refactor_this.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace refactor_this.Repositories
{
    public class ProductOptionRepository : IProductOptionRepository
    {
        public IEnumerable<ProductOption> GetAll()
        {
            var items = new List<ProductOption>();

            using (var conn = Helpers.NewConnection())
            {
                string sql = "SELECT id FROM productoption";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = Guid.Parse(rdr["id"].ToString());
                            var option = GetById(id);

                            items.Add(option);
                        }
                    }
                }
            }

            return items;
        }

        public IEnumerable<ProductOption> GetAllByProductId(Guid productId)
        {
            var productIdAsString = productId.ToString();

            if (string.IsNullOrEmpty(productIdAsString)) throw new ArgumentNullException(nameof(productIdAsString));

            var items = new List<ProductOption>();

            using (var conn = Helpers.NewConnection())
            {
                string sql = "SELECT id FROM productoption";

                sql += " WHERE productid = @ProductId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productIdAsString);

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

        public ProductOption GetById(Guid id)
        {
            var option = new ProductOption();

            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("SELECT * FROM productoption WHERE id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return option;

                    option.IsNew = false;
                    option.Id = Guid.Parse(rdr["Id"].ToString());
                    option.ProductId = Guid.Parse(rdr["ProductId"].ToString());
                    option.Name = rdr["Name"].ToString();
                    option.Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                }
            }

            return option;
        }

        public void Add(ProductOption option)
        {
            using (var conn = Helpers.NewConnection())
            {
                string sql =
                    "INSERT INTO productoption (id, productid, name, description) VALUES (@Id, @ProductId, @Name, @Description)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", option.Id);
                    cmd.Parameters.AddWithValue("@ProductId", option.ProductId);
                    cmd.Parameters.AddWithValue("@Name", option.Name);
                    cmd.Parameters.AddWithValue("@Description", option.Description ?? (object)DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(ProductOption option)
        {
            using (var conn = Helpers.NewConnection())
            {
                string sql = 
                    "UPDATE productoption SET name = @Name, description = @Description WHERE id = @Id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", option.Id);
                    cmd.Parameters.AddWithValue("@ProductId", option.ProductId);
                    cmd.Parameters.AddWithValue("@Name", option.Name);
                    cmd.Parameters.AddWithValue("@Description", option.Description ?? (object)DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("DELETE FROM productoption WHERE id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

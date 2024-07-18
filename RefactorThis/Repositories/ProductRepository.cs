using refactor_this.Models;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace refactor_this.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAll()
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

        public IEnumerable<Product> GetByName(string name)
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

        public Product GetById(Guid id)
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

        public void Add(Product product)
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

        public void Update(Product product)
        {
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

        public void Delete(Guid id)
        {
            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("DELETE FROM product WHERE id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

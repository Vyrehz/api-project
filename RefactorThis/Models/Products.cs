using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace refactor_this.Models
{
    public class Products
    {
        public List<Product> Items { get; private set; }

        public Products()
        {
            LoadProducts();
        }

        public Products(string name)
        {
            LoadProductsByName(name);
        }

        private void LoadProducts()
        {

            Items = new List<Product>();

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
                            var id = Guid.Parse(rdr["id"].ToString());

                            Items.Add(new Product(id));
                        }
                    }
                }
            }
        }

        private void LoadProductsByName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var lowerCaseName = name?.ToLower();

            Items = new List<Product>();

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

                            Items.Add(new Product(id));
                        }
                    }
                }
            }
        }
    }

    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }
        
        [JsonIgnore]
        public bool IsNew { get; }

        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Product(Guid id)
        {
            IsNew = true;

            using (var conn = Helpers.NewConnection())        
            using (var cmd = new SqlCommand("select * from product where id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return;

                    IsNew = false;
                    Id = Guid.Parse(rdr["Id"].ToString());
                    Name = rdr["Name"].ToString();
                    Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                    Price = decimal.Parse(rdr["Price"].ToString());
                    DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                }
            }
        }

        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                string sql = IsNew ?
                    "insert into product (id, name, description, price, deliveryprice) values (@Id, @Name, @Description, @Price, @DeliveryPrice)" :
                    "update product set name = @Name, description = @Description, price = @Price, deliveryprice = @DeliveryPrice where id = @Id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Description", Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Price", Price);
                    cmd.Parameters.AddWithValue("@DeliveryPrice", DeliveryPrice);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete()
        {
            foreach (var option in new ProductOptions(Id).Items)
                option.Delete();

            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("delete from product where id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    public class ProductOptions
    {
        public List<ProductOption> Items { get; private set; }

        public ProductOptions()
        {
            LoadProductOptions();
        }

        public ProductOptions(Guid productId)
        {
            LoadProductOption(productId);
        }

        private void LoadProductOptions()
        {
            Items = new List<ProductOption>();

            using (var conn = Helpers.NewConnection())
            {
                string sql = "select id from productoption";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = Guid.Parse(rdr["id"].ToString());

                            Items.Add(new ProductOption(id));
                        }
                    }
                }
            }
        }

        private void LoadProductOption(Guid productId)
        {
            var productIdAsString = productId.ToString();

            if (string.IsNullOrEmpty(productIdAsString)) throw new ArgumentNullException(nameof(productIdAsString));
                
            Items = new List<ProductOption>();

            using (var conn = Helpers.NewConnection())
            {
                string sql = "select id from productoption";

                sql += " where productid = @ProductId";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productIdAsString);

                    conn.Open();

                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var id = Guid.Parse(rdr["id"].ToString());

                            Items.Add(new ProductOption(id));
                        }
                    }
                }
            }
        }
    }

    public class ProductOption
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }

        public ProductOption()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public ProductOption(Guid id)
        {
            IsNew = true;
            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("select * from productoption where id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return;

                    IsNew = false;
                    Id = Guid.Parse(rdr["Id"].ToString());
                    ProductId = Guid.Parse(rdr["ProductId"].ToString());
                    Name = rdr["Name"].ToString();
                    Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                }
            }
        }

        public void Save()
        {
            using (var conn = Helpers.NewConnection())
            {
                string sql = IsNew ?
                    "insert into productoption (id, productid, name, description) values (@Id, @ProductId, @Name, @Description)" :
                    "update productoption set name = @Name, description = @Description where id = @Id";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.Parameters.AddWithValue("@ProductId", ProductId);
                    cmd.Parameters.AddWithValue("@Name", Name);
                    cmd.Parameters.AddWithValue("@Description", Description ?? (object)DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete()
        {
            using (var conn = Helpers.NewConnection())
            using (var cmd = new SqlCommand("delete from productoption where id = @Id", conn))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
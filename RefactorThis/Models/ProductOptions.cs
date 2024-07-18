using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace refactor_this.Models
{
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
}
﻿using System;
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
}
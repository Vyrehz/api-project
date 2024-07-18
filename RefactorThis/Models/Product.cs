using Newtonsoft.Json;
using System.Data.SqlClient;
using System;

namespace refactor_this.Models
{
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
}
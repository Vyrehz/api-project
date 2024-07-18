using Newtonsoft.Json;
using System.Data.SqlClient;
using System;

namespace refactor_this.Models
{
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
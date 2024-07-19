using System.Data.SqlClient;

namespace refactor_this.Models
{
    public interface IConnectionHelper
    {
        SqlConnection NewConnection();
    }
}

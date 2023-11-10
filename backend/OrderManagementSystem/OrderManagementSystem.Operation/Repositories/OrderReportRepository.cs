using Dapper;
using OrderManagementSystem.Schema;
using System.Data;

namespace OrderManagementSystem.Repositories
{
    public class OrderReportRepository
    {
        private readonly IDbConnection _dbConnection;

        public OrderReportRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IEnumerable<OrderReportDto>> GetOrderReportByUserId(int userId)
        {
            const string query = "SELECT * FROM OrderReport WHERE UserId = @UserId";
            return await _dbConnection.QueryAsync<OrderReportDto>(query, new { UserId = userId });
        }
    }
}

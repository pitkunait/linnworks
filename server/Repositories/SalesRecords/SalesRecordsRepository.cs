using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace LinnworksTechTest.Repositories.SalesRecords
{
    public class SalesRecordsRepository : SqlConnectionProvider
    {
        public SalesRecordsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<IEnumerable<SalesRecord>> GetAllAsync()
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * from SalesRecords";
                return await conn.QueryAsync<SalesRecord>(sql);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "DELETE FROM SalesRecords WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                await conn.QueryFirstOrDefaultAsync<SalesRecord>(sql, parameters);
            }
        }

        public async Task<SalesRecord> FindAsync(int id)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE Id = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.Int32);
                return await conn.QueryFirstOrDefaultAsync<SalesRecord>(sql, parameters);
            }
        }

        public async Task InsertAsync(List<SalesRecord> salesRecords)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = @"INSERT INTO SalesRecords (
                          Region, 
                          Country, 
                          ItemType, 
                          SalesChannel, 
                          OrderPriority, 
                          OrderDate, 
                          OrderId, 
                          ShipDate, 
                          UnitsSold, 
                          UnitPrice, 
                          UnitCost, 
                          TotalRevenue, 
                          TotalCost, 
                          TotalProfit) 
                          VALUES (@Region, 
                                  @Country, 
                                  @ItemType, 
                                  @SalesChannel, 
                                  @OrderPriority, 
                                  @OrderDate, 
                                  @OrderId, 
                                  @ShipDate, 
                                  @UnitsSold, 
                                  @UnitPrice, 
                                  @UnitCost, 
                                  @TotalRevenue, 
                                  @TotalCost, 
                                  @TotalProfit)
                ";
                await conn.ExecuteAsync(sql, salesRecords);
            }
        }

        public async Task<IEnumerable<SalesRecord>> FilterByYearAsync(int year)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE YEAR(OrderDate) = @Year";
                var parameters = new DynamicParameters();
                parameters.Add("@Year", year, DbType.Int32);
                return await conn.QueryAsync<SalesRecord>(sql, parameters);
            }
        }
        
        public async Task<IEnumerable<SalesRecord>> FilterByCountry(string country)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE Country = @Country";
                var parameters = new DynamicParameters();
                parameters.Add("@Country", country, DbType.String);
                return await conn.QueryAsync<SalesRecord>(sql, parameters);
            }
        }
        
        public async Task<IEnumerable<SalesRecord>> FilterByYearAndCountry(string country, int year)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE YEAR(OrderDate) = @Year AND Country = @Country";
                var parameters = new DynamicParameters();
                parameters.Add("@Year", year, DbType.Int32);
                parameters.Add("@Country", country, DbType.String);
                return await conn.QueryAsync<SalesRecord>(sql, parameters);
            }
        }

        public async Task UpdateAsync(SalesRecord entityToUpdate)
        {
            using (var conn = GetOpenConnection())
            {
                var existingEntity = await FindAsync(entityToUpdate.Id);
                var sql = "UPDATE SalesRecords SET ";
                var parameters = new DynamicParameters();

                sql += "Name=@Name";
                parameters.Add("@Name", entityToUpdate.Country, DbType.String);

                sql = sql.TrimEnd(',');
                sql += " WHERE Id=@Id";
                parameters.Add("@Id", entityToUpdate.Id, DbType.Int32);
                await conn.QueryAsync(sql, parameters);
            }
        }
    }
}
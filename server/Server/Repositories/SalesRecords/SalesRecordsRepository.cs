using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using FastMember;

namespace Server.Repositories.SalesRecords
{
    public class SalesRecordsRepository : SqlConnectionProvider
    {
        public SalesRecordsRepository(string connectionString) : base(connectionString)
        {
        }

        public async Task<PagedResult<SalesRecord>> GetAsync(
            int page = 1,
            int pageSize = 100,
            string sortColumn = "id",
            string sortDirection = "asc",
            string country = null,
            int? year = null
        )
        {
            var results = new PagedResult<SalesRecord>();
            using (var conn = GetOpenConnection())
            {
                var sql = @"SELECT *
                            FROM SalesRecords
                            WHERE (@Country IS NULL OR Country = @Country) AND (@Year IS NULL OR YEAR(OrderDate) = @Year)
                            ORDER BY

                                --          Int
                                CASE WHEN @SortDirection = 'asc' THEN
                                         CASE @SortColumn
                                             WHEN 'id'          THEN Id
                                             WHEN 'country'     THEN Country
                                             END
                                    END,
                                CASE WHEN @SortDirection = 'desc' THEN
                                         CASE @SortColumn
                                             WHEN 'id'          THEN Id
                                             WHEN 'country'     THEN Country
                                             END
                                    END DESC,

                                --          Date
                                CASE WHEN @SortDirection = 'asc' THEN
                                         CASE @SortColumn
                                             WHEN 'orderDate'   THEN OrderDate
                                             END
                                    END,
                                CASE WHEN @SortDirection = 'desc' THEN
                                         CASE @SortColumn
                                             WHEN 'orderDate'   THEN OrderDate
                                             END
                                    END DESC
                            OFFSET @Offset ROWS
                            FETCH NEXT @PageSize ROWS ONLY;
                           
                            SELECT COUNT(*)
                            FROM SalesRecords
                            WHERE (@Country IS NULL OR Country = @Country) AND (@Year IS NULL OR YEAR(OrderDate) = @Year);
                            
                            SELECT SUM(TotalProfit)
                            FROM SalesRecords
                            WHERE (@Country IS NULL OR Country = @Country) AND (@Year IS NULL OR YEAR(OrderDate) = @Year)";

                var multi = await conn.QueryMultipleAsync(sql,
                    new
                    {
                        Offset = (page - 1) * pageSize,
                        PageSize = pageSize,
                        SortColumn = sortColumn,
                        SortDirection = sortDirection,
                        Country = country,
                        Year = year
                    });

                results.Items = multi.Read<SalesRecord>().ToList();
                results.TotalCount = multi.ReadFirst<int>();
                results.TotalProfit = multi.ReadFirst<float>();
                results.Page = page;
                results.PageSize = pageSize;
                results.HasNext = results.TotalCount > page * pageSize;

                return results;
            }
        }

        public async Task<IEnumerable<string>> GetAllCountries()
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT DISTINCT Country FROM SalesRecords";
                return await conn.QueryAsync<string>(sql);
            }
        }
        
        public async Task DeleteAsync(List<int> idList)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "DELETE FROM SalesRecords WHERE Id IN @idList";
                await conn.ExecuteAsync(sql, new {idList});
            }
        }

        public async Task InsertBulkAsync(IEnumerable<SalesRecord> salesRecords)
        {
            using (var conn = GetOpenBulkInsertConnection())
            using (var reader = ObjectReader.Create(
                salesRecords, "Id",
                "Region", "Country", "ItemType", "SalesChannel", "OrderPriority", "OrderDate", "OrderId",
                "ShipDate", "UnitsSold", "UnitPrice", "UnitCost", "TotalRevenue", "TotalCost", "TotalProfit"
            ))
            {
                conn.DestinationTableName = "SalesRecords";
                conn.EnableStreaming = true;
                conn.BatchSize = 10000;
                conn.BulkCopyTimeout = 0;
                conn.NotifyAfter = 100;
                await conn.WriteToServerAsync(reader);
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
                return await conn.QueryAsync<SalesRecord>(sql, new {Year = year});
            }
        }

        public async Task<IEnumerable<SalesRecord>> FilterByCountry(string country)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE Country = @Country";
                return await conn.QueryAsync<SalesRecord>(sql, new {Country = country});
            }
        }

        public async Task<IEnumerable<SalesRecord>> FilterByYearAndCountry(string country, int year)
        {
            using (var conn = GetOpenConnection())
            {
                var sql = "SELECT * FROM SalesRecords WHERE YEAR(OrderDate) = @Year AND Country = @Country";
                return await conn.QueryAsync<SalesRecord>(sql, new {Year = year, Country = country});
            }
        }

        public async Task UpdateAsync(SalesRecord entityToUpdate)
        {
            using (var conn = GetOpenConnection())
            {
                await conn.UpdateAsync(entityToUpdate);
            }
        }
    }
}
using Hotel.Application.Interfaces.Repositories;
using Hotel.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Infrastructure.Repositories
{
    public class RoomAmenitiesImagesRepository : IRoomAmenitiesImagesRepository
    {
        private readonly HotelDbContext _context;

        public RoomAmenitiesImagesRepository(HotelDbContext context)
            => _context = context;


        public async Task<int> CreateAsync(
            string name, 
            decimal pricePerNight, 
            string createdBy, 
            int capacity, 
            string description, 
            bool isWorking,
            int[]? amenities, 
            string[]? images)
        {
            var amenitiesParam = amenities != null && amenities.Any() 
                ? "{" + string.Join(",", amenities) + "}" 
                : null;
                
            var imagesParam = images != null && images.Any()
                ? "{" + string.Join(",", images.Select(i => $"\"{i}\"")) + "}"
                : null;

            var result = await _context.Database.SqlQueryRaw<int>(
                "SELECT fn_hotel_c_room_amenities_images({0}, {1}, {2}, {3}, {4}, {5}, {6}::integer[], {7}::text[]) AS \"Value\"",
                name,
                pricePerNight,
                createdBy,
                capacity,
                description,
                isWorking,
                amenitiesParam,
                imagesParam
            ).FirstOrDefaultAsync();

            return result;
        }

        public async Task UpdateAsync(
            int roomId, 
            string name, 
            decimal pricePerNight, 
            int capacity, 
            string description, 
            bool isWorking,
            string amenitiesJson, 
            string imagesJson)
        {
            var connection = _context.Database.GetDbConnection();
            
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT fn_hotel_u_room_amenities_images(@roomId, @name, @pricePerNight, @capacity, @description, @isWorking, @amenitiesJson::json, @imagesJson::json)";
                
                void AddParam(string paramName, object? value)
                {
                    var p = command.CreateParameter();
                    p.ParameterName = paramName;
                    p.Value = value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                AddParam("@roomId", roomId);
                AddParam("@name", name);
                AddParam("@pricePerNight", pricePerNight);
                AddParam("@capacity", capacity);
                AddParam("@description", description);
                AddParam("@isWorking", isWorking);
                AddParam("@amenitiesJson", amenitiesJson);
                AddParam("@imagesJson", imagesJson);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task DeleteAsync(int roomId)
        {
            var connection = _context.Database.GetDbConnection();
            
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT fn_hotel_d_room(@roomId)";
                
                var p = command.CreateParameter();
                p.ParameterName = "@roomId";
                p.Value = roomId;
                command.Parameters.Add(p);

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
        }

        public async Task<RoomFullQueryResult?> GetByIdAsync(int roomId)
        {
            RoomFullQueryResult? result = null;
            var connection = _context.Database.GetDbConnection();
            
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM fn_hotel_s_room_full(@roomId)";
                
                var param = command.CreateParameter();
                param.ParameterName = "@roomId";
                param.Value = roomId;
                command.Parameters.Add(param);

                using var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    result = new RoomFullQueryResult
                    {
                        room_id = reader.GetInt32(reader.GetOrdinal("room_id")),
                        name = reader.GetString(reader.GetOrdinal("name")),
                        price_per_night = reader.GetDecimal(reader.GetOrdinal("price_per_night")),
                        capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
                        description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                        is_active = reader.GetBoolean(reader.GetOrdinal("is_active")),
                        is_working = reader.GetBoolean(reader.GetOrdinal("is_working")),
                        created_date = reader.GetDateTime(reader.GetOrdinal("created_date")),
                        created_by = reader.IsDBNull(reader.GetOrdinal("created_by")) ? string.Empty : reader.GetString(reader.GetOrdinal("created_by")),
                        amenities = reader.IsDBNull(reader.GetOrdinal("amenities")) ? null : reader.GetValue(reader.GetOrdinal("amenities")).ToString(),
                        images = reader.IsDBNull(reader.GetOrdinal("images")) ? null : reader.GetValue(reader.GetOrdinal("images")).ToString()
                    };
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }
            
            return result;
        }

        public async Task<(IEnumerable<RoomFullListQueryResult> 
            Data, int TotalRecords)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? name, 
            int? capacity, 
            decimal? minPrice, 
            decimal? maxPrice, 
            string sort)
        {
            var results = new List<RoomFullListQueryResult>();
            int totalRecords = 0;
            
            var connection = _context.Database.GetDbConnection();
            
            try
            {
                if (connection.State != ConnectionState.Open)
                    await connection.OpenAsync();

                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM fn_hotel_s_rooms_full($1, $2, $3, $4, $5, $6, $7)";
                
                void AddParam(object? value)
                {
                    var p = command.CreateParameter();
                    p.Value = value ?? DBNull.Value;
                    command.Parameters.Add(p);
                }

                AddParam(page);
                AddParam(pageSize);
                AddParam(name);
                AddParam(capacity);
                AddParam(minPrice);
                AddParam(maxPrice);
                AddParam(sort);

                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var row = new RoomFullListQueryResult
                    {
                        room_id = reader.GetInt32(reader.GetOrdinal("room_id")),
                        name = reader.GetString(reader.GetOrdinal("name")),
                        price_per_night = reader.GetDecimal(reader.GetOrdinal("price_per_night")),
                        capacity = reader.GetInt32(reader.GetOrdinal("capacity")),
                        description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                        is_available = reader.GetBoolean(reader.GetOrdinal("is_available")),
                        is_working = reader.GetBoolean(reader.GetOrdinal("is_working")),
                        amenities = reader.IsDBNull(reader.GetOrdinal("amenities")) ? null : reader.GetValue(reader.GetOrdinal("amenities")).ToString(),
                        images = reader.IsDBNull(reader.GetOrdinal("images")) ? null : reader.GetValue(reader.GetOrdinal("images")).ToString(),
                        total_records = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("total_records")))
                    };
                    results.Add(row);
                }
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    await connection.CloseAsync();
            }

            totalRecords = results.FirstOrDefault()?.total_records ?? 0;
            return (results, totalRecords);
        }
    }
}

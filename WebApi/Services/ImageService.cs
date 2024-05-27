using System.Data.SQLite;
using Microsoft.Extensions.Configuration;

namespace WebApi.Services
{
    public class ImageService
    {
        
       public string _connectionString = "";
        //string _connectionString = @"Data Source= |DataDirectory|\data.db; Version=3;";

        public ImageService()
        {
            string appDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string dbPath = Path.Combine(appDirectory, "data.db");
            _connectionString = $"Data Source={dbPath}; Version=3;";
        }

        public string GetImageUrl(string userId)
        {
            string imageUrl = null;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                // Extract last digit of user ID
                int lastDigit = userId.EndsWith("0") ? 0 : int.Parse(userId.Last().ToString());

                // Write SQL query
                string sql = "SELECT url FROM images WHERE id = @lastDigit";

                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@lastDigit", lastDigit);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            imageUrl = reader.GetString(0); // Assuming "url" is the first column (index 0)
                        }
                    }
                }
            }

            return imageUrl;
        }
    }
}

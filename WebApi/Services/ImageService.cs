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

                string sql2 = @"
                    CREATE TABLE IF NOT EXISTS images (
                        id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                        url TEXT NOT NULL
                    );";

                string sql3 = @"INSERT INTO images (url) VALUES (@url)";

                var imageData = new[] {
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=0&size=150" },
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=1&size=150" },
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=2&size=150" },
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=3&size=150" },
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=4&size=150" },
                    new { url = "https://api.dicebear.com/8.x/pixel-art/png?seed=5&size=150" },

                };

                using (var command = new SQLiteCommand(sql2, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(sql3, connection))
                {
                    foreach (var data in imageData)
                    {
                        command.Parameters.AddWithValue("@url", data.url);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear(); // Clear parameters for next iteration
                    }
                }

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

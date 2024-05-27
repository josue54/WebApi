using Microsoft.AspNetCore.Mvc;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class AvatarController : Controller
    {

        [HttpGet]
        [Route("avatar")]
        public IActionResult GetAvatar([FromQuery] string userIdentifier)
        {
            // Validate user input
            if (string.IsNullOrEmpty(userIdentifier))
            {
                return BadRequest(new { message = "User identifier is required." });
            }

            var imageUrl = GetImageUrl(userIdentifier);

            if (imageUrl == null)
            {
                return NotFound();
            }

            return Ok(new { imageUrl });
        }

        private string GetImageUrl(string userId)
        {
            var lastChar = userId.LastOrDefault();

            // Check for digits (6-9)
            if (char.IsDigit(lastChar))
            {


                int digit;
                if (int.TryParse(lastChar.ToString(), out digit) && digit >= 6)
                {
                    switch (digit)
                    {
                        case 6:
                            return "https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/6"; 
                        case 7:
                            return "https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/7"; 
                        case 8:
                            return "https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/8"; 
                        case 9:
                            return "https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/9"; 
                        default:
                            break;
                    }
                }else if (int.TryParse(lastChar.ToString(), out digit) && digit <= 5)
                {
                    var imageService = new ImageService();
                    string imageUrl = imageService.GetImageUrl(userId);
                }
            }

            if (userId.IndexOfAny("aeiou".ToCharArray()) != -1)
            {
                return $"https://api.dicebear.com/8.x/pixel-art/png?seed=vowel&size=150";
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(userId, "^[a-zA-Z0-9]+$"))
            {
                var randomNumber = new Random().Next(1, 6);
                return $"https://api.dicebear.com/8.x/pixel-art/png?seed={randomNumber}&size=150";
            }

            // Default
            return "https://api.dicebear.com/8.x/pixel-art/png?seed=default&size=150";
        }

    }
}

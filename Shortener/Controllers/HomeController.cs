using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shortener.Data.Entities;
using Shortener.Interfaces;
using Shortener.Models;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shortener.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    public class HomeController : Controller
    {
        private readonly string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private readonly IDbContext _dbContext;

        public HomeController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, Route("/")]
        public async Task<IActionResult> ShortenUrlAsync([FromBody] string urlToShorten)
        {
            var shortUrl = new ShortUrl
            {
                OriginalUrl = urlToShorten,
                Route = GenerateRoute(urlToShorten)
            };

            _dbContext.ShortUrls.Add(shortUrl);
            await _dbContext.SaveChangesAsync();

            var dto = new ShortUrlViewModel
            {
                ShortUrl = $"{Request.Scheme}://{Request.Host}/{shortUrl.Route}",
                OriginalUrl = shortUrl.OriginalUrl
            };
            return Json(dto);
        }

        [HttpGet, Route("/{route}")]
        public async Task<IActionResult> RedirectToAsync([FromRoute] string route)
        {
            var shortUrl = await _dbContext.ShortUrls.FirstOrDefaultAsync(u => u.Route == route);
            if (shortUrl is null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            return Redirect(shortUrl.OriginalUrl);
        }

        private string GenerateRoute(string url)
        {
            var hash = MD5HashConvert(url);
            var base62 = Base62Convert(hash);

            var builder = new StringBuilder();
            for (var i = 0; i < 6; i++)
            {
                builder.Append(Alphabet[base62[i]]);
            }

            return builder.ToString();
        }

        private byte[] MD5HashConvert(string text)
        {
            var md5 = new MD5CryptoServiceProvider();

            var bytes = Encoding.ASCII.GetBytes(text);
            var hash = md5.ComputeHash(bytes);

            return hash;
        }

        private byte[] Base62Convert(byte[] source)
        {
            var result = new List<int>();
            var count = 0;
            while ((count = source.Length) > 0)
            {
                var quotient = new List<byte>();
                var remainder = 0;
                for (var i = 0; i != count; i++)
                {
                    int accumulator = source[i] + remainder * 256;
                    byte digit = System.Convert.ToByte((accumulator - (accumulator % 62)) / 62);
                    remainder = accumulator % 62;
                    if (quotient.Count > 0 || digit != 0)
                    {
                        quotient.Add(digit);
                    }
                }

                result.Insert(0, remainder);
                source = quotient.ToArray();
            }

            var output = new byte[result.Count];
            for (var i = 0; i < result.Count; i++)
            {
                output[i] = (byte)result[i];
            }

            return output;
        }
    }
}

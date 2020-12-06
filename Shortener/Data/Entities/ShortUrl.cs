using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shortener.Data.Entities
{
    [Table("short_urls")]
    public class ShortUrl
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("route")]
        public string Route { get; set; }

        [Column("original_url")]
        public string OriginalUrl { get; set; }
    }
}

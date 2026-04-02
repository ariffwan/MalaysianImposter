using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JanganKantoi.Models
{
	[Table("categories")]
	public class Category
	{
		[Key]
		public Guid categories_id { get; set; }

		public string categories_name { get; set; }

		public int categories_index { get; set; }

		public ICollection<Word> Words { get; set; }
	}
}
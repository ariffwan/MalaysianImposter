using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JanganKantoi.Models
{
	[Table("words")]
	public class Word
	{
		[Key]
		public Guid word_id { get; set; }

		public string word_name { get; set; }

		public Guid word_category { get; set; }
		public string word_hint { get; set; }

		[ForeignKey("word_category")]
		public Category Category { get; set; }
	}
}
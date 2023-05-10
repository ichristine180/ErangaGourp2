using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace E_ranga.Models
{
    public class Documents
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Owner Name")]
        [Column("owner_names")]
        public string OwnerNames { get; set; }
        [Required]
        [Display(Name = "Description (like found place)")]
        [Column("description")]
        public string Description { get; set; }
        [Column("status")]
        public int Status { get; set; }

        [Required]
        [Display(Name = "Type of Document")]
        [Column("doc_type")]
        public string DocType { get; set; }
        [Display(Name = "Document")]
        [Column("doc_data")]
        public byte[] ImageData { get; set; }
        [NotMapped]
        public string ImageDataUrl { get; set; }

        [Column("poster_id")]
        public int PosterId { get; set; }

        [ForeignKey(nameof(PosterId))]
        [InverseProperty("Document")]
        public virtual UserRegister UserRegister { get; set; }

    }
}


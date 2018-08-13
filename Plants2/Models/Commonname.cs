using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Plants2.Models
{
    public class Commonname
    {
        [Key]
        public int CommID { get; set; }
        [Required]
        [StringLength(50)]
        //[RegularExpression("^[a-zA-Z ]+$")]  // matches alphabets and space
        //[RegularExpression("^[a-zA-Z]+[ ?][a-zA-Z]+$")]  // matches alphabets and space
        [RegularExpression("^[a-zA-Z]+ ?-?[a-zA-Z]+$")]  // matches alphabets and space
        [Display(Name = "Common Name")]
        [Index("IX_CommName", 1, IsUnique = true)]
        public string CommName { get; set; }
        public string Language { get; set; }
        //[InverseProperty("Plant")]
        public virtual ICollection<Plant> Plants { get; set; }
    }

    public partial class Plant2DBContext : DbContext
    {
        public DbSet<Commonname> Commonnames { get; set; }
    }
}
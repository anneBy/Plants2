using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace Plants2.Models
{
    public class Plant
    {
        [Key]
        public int PlantID { get; set; } 
        [Required]
        [StringLength(50)]
        [Index("IX_Name", 1, IsUnique = true)]
        [Remote("IsPlantInDB", "Plants", ErrorMessage = "This Plant is already in the Database.")]
        public string Name { get; set; }
        public int? ParentID { get; set; }
        [StringLength(50)]
        [Display(Name = "Level")]
        public string HLevelName { get; set; }
        [ForeignKey("ParentID")]
        public virtual Plant Parent { get; set; }
        public virtual ICollection<Plant> Childs { get; set; }
    }

    public class Plant2DBContext : DbContext
    {
        public DbSet<Plant> Plants { get; set; }
    }
}
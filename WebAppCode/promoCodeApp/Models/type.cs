namespace promoCodeApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("type")]
    public partial class type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public type()
        {
            promotions = new HashSet<promotion>();
        }

        [Display(Name = "Type Id")]
        public int typeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Type Name")]
        public string name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<promotion> promotions { get; set; }


        [Display(Name = "Is Deteled")]
        public bool isDeleted { get; set; }
    }
}

namespace promoCodeApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("gamePackage")]
    public partial class gamePackage
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public gamePackage()
        {
            promotions = new HashSet<promotion>();
        }

        [Display(Name = "Game Package Id")]
        public int gamePackageId { get; set; }

        [Display(Name = "Game Id")]
        public int gameId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Package Name")]
        public string packageName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Secret Key")]
        public string secretKey { get; set; }

        [Display(Name = "Is Active")]
        public bool isActive { get; set; }

        [Display(Name = "Date Time")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime datetime { get; set; }

        public virtual game game { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<promotion> promotions { get; set; }


        [Display(Name = "Is Deteled")]
        public bool isDeleted { get; set; }
    }
}

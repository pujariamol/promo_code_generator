namespace promoCodeApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("game")]
    public partial class game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public game()
        {
            gamePackages = new HashSet<gamePackage>();
        }

        [Display(Name = "Game Id")]
        public int gameId { get; set; }

        [Display(Name = "Registration Id")]
        public int registrationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Game Name")]
        public string gameName { get; set; }

        [Display(Name = "Is Active")]
        public bool isActive { get; set; }

        [Display(Name = "Date Time")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime dateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<gamePackage> gamePackages { get; set; }

        public virtual registration registration { get; set; }


        [Display(Name = "Is Deteled")]
        public bool isDeleted { get; set; }
    }
}

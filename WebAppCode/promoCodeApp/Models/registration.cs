namespace promoCodeApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("registration")]
    public partial class registration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public registration()
        {
            games = new HashSet<game>();
        }

        [Display(Name = "Environment Id")]
        public int registrationId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Environment Name")]
        public string registrationName { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [StringLength(100)]
        [Display(Name = "Client Secret Key")]
        public string clientServerKey { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Date Time")]
        public DateTime dateTime { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<game> games { get; set; }


        [Display(Name = "Is Deteled")]
        public bool isDeleted { get; set; }
    }
}

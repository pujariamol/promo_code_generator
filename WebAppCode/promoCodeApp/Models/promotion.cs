namespace promoCodeApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("promotion")]
    public partial class promotion
    {
        [Display(Name = "Promotion Id")]
        public int promotionId { get; set; }

        [Display(Name = "Game Package Id")]
        public int gamePackageId { get; set; }

        [Display(Name = "Type Id")]
        public int typeId { get; set; }

        [Display(Name = "Start Date")]
        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }

        [Display(Name = "End Date")]
        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }


        [StringLength(100)]
        [Display(Name = "Promotion Code")]
        public string promotionCode { get; set; }

        [Display(Name = "Is Redeemed")]
        public bool isRedeemed { get; set; }

        [Display(Name = "Is Active")]
        public bool isActive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Display(Name = "Date Time")]
        public DateTime dateTime { get; set; }

        public virtual gamePackage gamePackage { get; set; }

        public virtual type type { get; set; }


        [Display(Name = "Is Deteled")]
        public bool isDeleted { get; set; }

        [NotMapped]
        [Range(1, 5000, ErrorMessage = "The value must be greater than 0 and less than 5000")]
        [Display(Name = "Number Of Code")]
        public int numberOfCodesToBeGenerated { get; set; }

        [NotMapped]
        [Display(Name = "Custom Code Value")]
        public string customCodeValue { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (endDate < startDate)
            {
                yield return new ValidationResult("EndDate must be greater than StartDate");
            }
        }
    }
}

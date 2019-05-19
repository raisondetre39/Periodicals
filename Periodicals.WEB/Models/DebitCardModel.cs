using System.ComponentModel.DataAnnotations;

namespace Periodicals.Models
{
    /// <summary>
    /// Class creates model to add money to use wallet
    /// </summary>
    public class DebitCardModel
    {
        [Display(Name = "Card number")]
        [Required(ErrorMessage = "Card number couldn't be empty")]
        public int CardNumber { get; set; }

        [Display(Name = "EM")]
        [RegularExpression(@"[0-9]{2}", ErrorMessage = "Invalid card")]
        public int ExpiryMounth { get; set; }

        [Display(Name = "EY")]
        [RegularExpression(@"[0-9]{2}", ErrorMessage = "Invalid card")]
        public int ExpiryYear { get; set; }

        [RegularExpression(@"[0-9]{3}", ErrorMessage = "Invalid card")]
        public int CCV { get; set; }

        [DataType(DataType.Currency)]
        public int Sum { get; set; }
    }
}
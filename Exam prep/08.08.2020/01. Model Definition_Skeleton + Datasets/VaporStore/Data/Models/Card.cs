using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using VaporStore.Data.Models.Enums;

namespace VaporStore.Data.Models
{
    public class Card
    {
        public Card()
        {
            Purchases = new List<Purchase>();
        }

        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Number { get; set; }

        [Required]
        [MaxLength(3)]
        public string Cvc { get; set; }

        [Required]
        [Range(0, 1)]
        public CardType Type { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}

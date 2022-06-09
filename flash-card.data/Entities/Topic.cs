using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data.Entities
{
    public class Topic : BaseEntity
    {
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public virtual ICollection<FlashCard> FlashCards { get; set; }
    }
}

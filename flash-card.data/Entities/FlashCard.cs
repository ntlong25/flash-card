using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data.Entities
{
    public class FlashCard : BaseEntity
    {
        public string Question { get; set; }
        public string ImgQuestion { get; set; }
        public string Answer { get; set; }
        public string ImgAnswer { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}

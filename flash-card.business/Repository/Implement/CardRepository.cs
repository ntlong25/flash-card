using flash_card.data;
using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.business.Repository.Implement
{
    public class CardRepository : Repository<FlashCard>, ICardRepository
    {
        private readonly DataContext _dataContext;
        public CardRepository(DataContext dataContext) : base(dataContext)
        {
            _dataContext = dataContext;
        }
    }
}

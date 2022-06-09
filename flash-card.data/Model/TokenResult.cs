using flash_card.data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data.Model
{
    public class TokenResult
    {
        public AccountModel User { get; set; }
        public string Token { get; set; }
    }
}

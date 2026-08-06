using System;
using System.Collections.Generic;
using System.Text;
using TIntegration.Models.Enums;

namespace TIntegration.Models.DTO
{
    public class PriceRequestDto
    {
        public string Code { get; set; }
        public StockTypeEnum Type { get; set; }

    }
}

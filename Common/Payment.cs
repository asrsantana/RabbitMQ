using System;

namespace Common
{
    [Serializable]
    public class Payment
    {
        public decimal AmountToPay { get; set; }
        public string CardNumber { get; set; }
        public string Name { get; set; }
    }
}

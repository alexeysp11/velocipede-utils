namespace VelocipedeUtils.Shared.Models.Business.Monetary
{
    /// <summary>
    /// Price
    /// </summary>
    public class Price
    {
        /// <summary>
        /// 
        /// </summary>
        public Price() : this(0)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public Price(decimal netPrice)
        {
            NetPrice = netPrice;
            Taxes = [];
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal? ListPrice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Tax> Taxes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal? Discount { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public decimal? NetPrice { get; }
    }
}
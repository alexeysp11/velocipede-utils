namespace VelocipedeUtils.Shared.Models.Business.Monetary
{
    /// <summary>
    /// Money.
    /// </summary>
    public class Money
    {
        /// <summary>
        /// Integer.
        /// </summary>
        public int Integer { get; private set; }

        /// <summary>
        /// Fraction.
        /// </summary>
        public int Fraction { get; private set; }

        /// <summary>
        /// Currency.
        /// </summary>
        public Currency Currency { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Money(string amount, Currency currency)
        {
            int integer = 0, fraction = 0;

            // Convert amount 
            string[] subs = amount.Split('.');
            if (subs.Length == 0 || subs.Length > 2) throw new System.Exception("Incorrect format of the parameter amount: " + amount);
            try
            {
                integer = System.Int32.Parse(subs[0]);
                fraction = subs.Length == 2 ? System.Int32.Parse(subs[1]) : 0;
            }
            catch (System.FormatException ex)
            {
                throw new System.Exception("Unable to convert amount string into money object: " + ex.Message);
            }
            CheckFractionFormat(fraction);

            Integer = integer;
            Fraction = fraction;
            Currency = currency;
        }

        /// <summary>
        /// 
        /// </summary>
        public Money(int integer, int fraction, Currency currency)
        {
            CheckFractionFormat(fraction);

            Integer = integer;
            Fraction = fraction;
            Currency = currency;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Add(int integer, int fraction)
        {
            Integer += integer;
            Fraction += fraction;
            if (Fraction > 99) { Fraction -= 100; Integer += 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Substract(int integer, int fraction)
        {
            Integer -= integer;
            Fraction -= fraction;
            if (Fraction < 0) { Fraction += 100; Integer -= 1; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetString()
        {
            CheckFractionFormat(Fraction);
            return Integer.ToString() + "." + Fraction.ToString() + " " + Currency.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetAmount()
        {
            CheckFractionFormat(Fraction);
            return Integer.ToString() + "." + Fraction.ToString();
        }

        /// <summary>
        /// Get currency.
        /// </summary>
        public string GetCurrency()
        {
            return Currency.ToString();
        }

        /// <summary>
        /// Get fraction format.
        /// </summary>
        private static void CheckFractionFormat(int fraction)
        {
            if (fraction < 0 || fraction > 99)
                throw new System.Exception("Fraction could not be less than 0 and begger than 99");
        }
    }
}
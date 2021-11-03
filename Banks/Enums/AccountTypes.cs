namespace Banks.Enums
{
    public enum AccountTypes
    {
        /// <summary>
        /// An ordinary account with a fixed interest on the balance.
        /// Money can be withdrawn at any time, you cannot leave a minus.
        /// There are no commissions.
        /// </summary>
        Debit = 1,

        /// <summary>
        /// An account from which you cannot withdraw and transfer money until its term expires (you can replenish).
        /// The percentage on the balance depends on the original amount.
        /// There are no commissions.
        /// Interest should be set for each bank.
        /// </summary>
        Deposit = 2,

        /// <summary>
        /// Has a credit limit, within which you can go into a minus (you can also go into plus).
        /// There is no interest on the balance.
        /// There is a flat usage fee if the customer is in the minus.
        /// </summary>
        Credit = 3
    }
}
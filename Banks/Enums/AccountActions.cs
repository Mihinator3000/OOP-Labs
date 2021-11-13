namespace Banks.Enums
{
    public enum AccountActions
    {
        /// <summary>
        /// Replenishment of the account
        /// </summary>
        Replenish = 1,

        /// <summary>
        /// Transaction from account to account
        /// </summary>
        Transact = 2,

        /// <summary>
        /// Withdrawal from account
        /// </summary>
        Withdraw = 3,
    }
}
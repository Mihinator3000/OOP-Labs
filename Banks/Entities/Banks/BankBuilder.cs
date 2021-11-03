namespace Banks.Entities.Banks
{
    public class BankBuilder
    {
        private readonly Bank _bank;

        public BankBuilder()
        {
            _bank = new Bank();
        }

        public BankBuilder SetMaxSummForDubiousClients(decimal maxSumm)
        {
            _bank.MaxSummForDubiousClients = maxSumm;
            return this;
        }

        public BankBuilder SetCreditComission(decimal comission)
        {
            _bank.CreditComission = comission;
            return this;
        }

        public BankBuilder SetCreditLimit(decimal limit)
        {
            _bank.CreditLimit = limit;
            return this;
        }

        public BankBuilder SetDebitInterest(decimal interest)
        {
            _bank.DebitInterest = interest;
            return this;
        }

        public Bank Build()
        {
            return _bank;
        }
    }
}
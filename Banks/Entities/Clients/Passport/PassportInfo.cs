using System;
using System.Text.RegularExpressions;
using Banks.Tools;

namespace Banks.Entities.Clients.Passport
{
    public class PassportInfo : IEquatable<PassportInfo>
    {
        public PassportInfo(string passportInfo)
        {
            Match match = new Regex(@"^M3(\d{4})(\d{8})$")
                .Match(passportInfo.Replace(" ", string.Empty));

            if (!match.Success)
            {
                throw new BanksException();
            }

            Batch = Convert.ToInt32(match.Groups[1].Value);
            Code = Convert.ToInt32(match.Groups[2].Value);
        }

        public int Batch { get; }

        public int Code { get; }

        public static implicit operator PassportInfo(string passportInfo)
        {
            return new PassportInfo(passportInfo);
        }

        public bool Equals(PassportInfo other)
        {
            if (other is null)
                throw new BanksException("Passport information is null");

            return Batch == other.Batch &&
                   Code == other.Code;
        }
    }
}
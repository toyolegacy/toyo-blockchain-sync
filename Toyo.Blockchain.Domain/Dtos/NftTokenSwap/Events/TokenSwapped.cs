using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace Toyo.Blockchain.Domain.Dtos
{
    [Event("TokenSwapped")]
    public class TokenSwappedEventDto : IEventDTO
    {
        [Parameter("address", "sender", 1, true)]
        public string Sender { get; set; }

        [Parameter("uint256", "fromTypeId", 2, true)]
        public BigInteger FromTypeId { get; set; }

        [Parameter("uint256", "toTypeId", 3, true)]
        public BigInteger ToTypeId { get; set; }

        [Parameter("uint256", "fromTokenId", 4, false)]
        public BigInteger FromTokenId { get; set; }

        [Parameter("uint256", "toTokenId", 5, false)]
        public BigInteger ToTokenId { get; set; }
    }
}
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Numerics;

namespace Toyo.Blockchain.Domain.Dtos
{
    [Event("Transfer")]
    public class TransferEventDto : IEventDTO
    {
        [Parameter("address", "from", 1, true)]
        public string From { get; set; }

        [Parameter("address", "to", 2, true)]
        public string To { get; set; }

        [Parameter("uint256", "tokenId", 3, true)]
        public BigInteger TokenId { get; set; }
    }
}
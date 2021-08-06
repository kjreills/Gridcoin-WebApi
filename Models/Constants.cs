using System.Collections.Generic;

namespace Gridcoin.WebApi.Constants
{
    public class Permissions
    {
        public const string ReadInfo = "read:info";
        public const string CreateAddress = "create:address";
        public const string CreateTransaction = "create:transaction";

        public static IEnumerable<string> All { get; } = new List<string> { ReadInfo, CreateAddress, CreateTransaction };
    }
}

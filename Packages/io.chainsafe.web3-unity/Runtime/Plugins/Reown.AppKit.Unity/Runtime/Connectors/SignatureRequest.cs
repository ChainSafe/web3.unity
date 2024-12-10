using System;
using System.Threading.Tasks;

namespace Reown.AppKit.Unity
{
    /// <summary>
    ///     Request to approve/reject EIP-4361 (SIWE) or CAIP-122 signature
    /// </summary>
    public class SignatureRequest
    {
        public Connector Connector { get; set; }
        public Func<Task> ApproveAsync { get; set; }
        public Func<Task> RejectAsync { get; set; }
    }
}
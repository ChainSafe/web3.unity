using System;
using evm.net.Models;

namespace evm.net
{
    public class EvmParameterInfoAttribute : Attribute
    {
        public string Type { get; set; }

        public string Name { get; set; }
    }
}
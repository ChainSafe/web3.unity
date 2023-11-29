using System;
using evm.net.Models;

namespace evm.net
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EvmMethodInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public bool View { get; set; } = false;

        public string[] Returns { get; set; } = null;

        public EvmMethodInfoAttribute()
        {
        }
    }
}
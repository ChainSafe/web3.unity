using System;

namespace evm.net
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EvmConstructorMethodAttribute : Attribute
    {
        public string Bytecode { get; set; }
        
        public EvmConstructorMethodAttribute()
        {
        }
    }
}
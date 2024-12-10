using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.CountlySDK
{
    public interface ISafeIDGenerator
    {
        string GenerateValue();
    }

    public class SafeIDGenerator : ISafeIDGenerator
    {
        public string GenerateValue()
        {
            return CountlyUtils.SafeRandomVal();
        }
    }
}


using Nethereum.Hex.HexTypes;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class HexBigIntUtil
{
    public static HexBigInteger ParseHexBigInt(string str) =>
        str == null ? null : new HexBigInteger(BigInteger.Parse(str));
}

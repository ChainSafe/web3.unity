using System;
using System.Text;
using Nethereum.Hex.HexConvertors.Extensions;

namespace ChainSafe.Gaming.Evm.RLP
{
    /// <summary>
    /// Provides methods to format and convert RLPElements to string.
    /// </summary>
    public class RLPStringFormatter
    {
        /// <summary>
        /// Converts an IRLPElement object to string in hexadecimal format.
        /// </summary>
        /// <param name="element">The IRLPElement object being converted.</param>
        /// <returns>The string format of the IRLPElement object in hexadecimal form.</returns>
        /// <exception cref="System.Exception">Thrown when the RLPElement object is null.</exception>
        public static string Format(IRLPElement element)
        {
            var output = new StringBuilder();
            if (element == null)
            {
                throw new Exception("RLPElement object can't be null");
            }

            if (element is RLPCollection rlpCollection)
            {
                output.Append('[');

                foreach (var innerElement in rlpCollection)
                {
                    output.Append(Format(innerElement));
                    output.Append(", ");
                }

                output.Append(']');
            }
            else
            {
                output.Append(element.RLPData.ToHex() + ", ");
            }

            return output.ToString();
        }
    }
}
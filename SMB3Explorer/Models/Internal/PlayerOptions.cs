using System.Collections.Generic;

namespace SMB3Explorer.Models.Internal
{
    public class PlayerOptions
    {
        // Option Key 57
        public static Dictionary<long, string> Positions = new Dictionary<long, string>
        {
            {1, "SP" },
            {2, "SP/RP" },
            {3, "RP" },
            {4, "CP" },
            {5, "C" },
            {6, "1B" },
            {7, "2B" },
            {8, "3B" },
            {9, "SS" },
            {10, "LF" },
            {11, "CF" },
            {12, "RF" }
        };

        // Option Key 5
        public static Dictionary<long, string> BattingHand = new Dictionary<long, string>
        {
            { 0, "L" },
            { 1, "R" },
            { 2, "S" }
        };

        // Option Key 4
        public static Dictionary<long, string> ThrowingHand = new Dictionary<long, string>
        {
            { 0, "L" },
            { 1, "R" }
        };
    }
}

using System.Collections.Generic;

namespace SMB3Explorer.Models.Internal
{
    public class PlayerOptions
    {
        // Option Key 57
        public static Dictionary<long, string> PitchPositions = new Dictionary<long, string>
        {
            { 1, "SP" },
            { 2, "SP/RP" },
            { 3, "RP" },
            { 4, "CP" }
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

        // Option Key 107
        public static Dictionary<long, string> Chemistry = new Dictionary<long, string> {
            { 0, "Competitive" },
            { 1, "Spirited" },
            { 2, "Disciplined" },
            { 3, "Scholarly" },
            { 4, "Crafty" }
        };

        // Option Key 49
        public static Dictionary<long, string> ArmAngle = new Dictionary<long, string> {
            { 0, "Sub" },
            { 1, "Low" },
            { 2, "Mid" },
            { 3, "High" }
        };
    }
}

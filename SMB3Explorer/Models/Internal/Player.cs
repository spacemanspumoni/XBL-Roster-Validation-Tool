using SMB3Explorer.Enums;
using SMB3Explorer.Utils;
using System;
using System.Linq;

namespace SMB3Explorer.Models.Internal
{
    public class Player
    {
        private Trait[] _traits = Array.Empty<Trait>();
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long Power { get; set; }
        public long Contact { get; set; }
        public long Speed { get; set; }
        public long Fielding { get; set; }
        public long? Arm {  get; set; }
        public long? Velocity { get; set; }
        public long? Junk { get; set; }
        public long? Accuracy { get; set; }
        public long? PrimaryPosition { get; set; }
        public long? SecondaryPosition { get; set; }
        public long? PitchPosition { get; set; }
        public long Batting { get; set; }
        public long Throwing { get; set; }
        public long Chemistry { get; set; }
        public bool FourSeam { get; set; }
        public bool TwoSeam { get; set; }
        public bool Screwball { get; set; }
        public bool ChangeUp { get; set; }
        public bool Fork { get; set; }
        public bool Curve { get; set; }
        public bool Slider { get; set; }
        public bool Cutter { get; set; }
        public long? ArmAngle { get; set; }
        public Trait[] Traits
        {
            set
            {
                _traits = value;
                if (!_traits.Any()) return;

                Trait1 = _traits[0].GetEnumDescription();

                if (_traits.Length > 1)
                    Trait2 = _traits[1].GetEnumDescription();
            }
        }

        public string? DisplayName => $"{FirstName} {LastName}";
        public string DisplayPrimaryPosition => PrimaryPosition.HasValue ? ((BaseballPlayerPosition)PrimaryPosition).GetEnumDescription() : "N/A";
        public string DisplaySecondaryPosition => SecondaryPosition.HasValue ? ((BaseballPlayerPosition)SecondaryPosition).GetEnumDescription() : "";
        public string DisplayPitchPosition => PitchPosition.HasValue ? PlayerOptions.PitchPositions[PitchPosition.Value] : "N/A";
        public string DisplayBatting => PlayerOptions.BattingHand[Batting];
        public string DisplayThrowing => PlayerOptions.ThrowingHand[Throwing];
        public string DisplayChemistry => PlayerOptions.Chemistry[Chemistry];
        public string DisplayArmAngle => ArmAngle.HasValue ? PlayerOptions.ArmAngle[ArmAngle.Value] : "N/A";
        public string? Trait1 { get; set; }
        public string? Trait2 { get; set; }
    }
}

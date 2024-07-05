namespace SMB3Explorer.Models.Internal
{
    public class SheetPlayer
    {
        public string? Name { get; set; }
        public int Power { get; set; }
        public int Contact { get; set; }
        public int Speed { get; set; }
        public int Fielding { get; set; }
        public int? Arm {  get; set; }
        public int? Velocity { get; set; }
        public int? Junk { get; set; }
        public int? Accuracy { get; set; }
        public string? PrimaryPosition { get; set; }
        public string? SecondaryPosition { get; set; }
        public string? PitchPosition { get; set; }
        public string? Batting { get; set; }
        public string? Throwing { get; set; }
        public string? Chemistry { get; set; }
        public bool FourSeam { get; set; }
        public bool TwoSeam { get; set; }
        public bool Screwball { get; set; }
        public bool ChangeUp { get; set; }
        public bool Fork { get; set; }
        public bool Curve { get; set; }
        public bool Slider { get; set; }
        public bool Cutter { get; set; }
        public string? ArmAngle { get; set; }
        public string? Trait1 { get; set; }
        public string? Trait2 { get; set; }
    }
}

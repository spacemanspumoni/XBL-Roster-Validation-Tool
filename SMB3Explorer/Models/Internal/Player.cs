using System;

namespace SMB3Explorer.Models.Internal
{
    public class Player
    {
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
    }
}

using System;
using System.IO;
using System.Linq;

namespace SMB3Explorer.Models.Internal;

public record TeamSelection
{
    private readonly char[] _invalidChars = new[] {' '}
        .Concat(Path.GetInvalidFileNameChars())
        .ToArray();
    
    public Guid TeamId { get; init; }
    public string TeamName { get; init; } = string.Empty;
}
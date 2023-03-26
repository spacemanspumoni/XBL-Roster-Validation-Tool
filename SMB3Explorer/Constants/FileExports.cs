﻿using System;
using System.IO;

namespace SMB3Explorer.Constants;

public static class FileExports
{
    public static readonly string BaseGameDirectoryPath = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData), "Metalhead", "Super Mega Baseball 3");
    
    public static readonly string BaseExportsDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer");
    
    public static readonly string LogDirectory = Path.Combine(BaseExportsDirectory, "Logs");

    public static readonly string TempDirectory = Path.GetTempFileName();
}
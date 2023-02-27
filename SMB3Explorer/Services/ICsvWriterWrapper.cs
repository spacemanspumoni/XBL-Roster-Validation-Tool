﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace SMB3Explorer.Services;

public interface ICsvWriterWrapper : IDisposable, IAsyncDisposable
{
    void Initialize(StreamWriter writer);
    Task WriteHeaderAsync<T>() where T : notnull;
    Task NextRecordAsync();
    Task WriteRecordAsync<T>(T record) where T : notnull;
}
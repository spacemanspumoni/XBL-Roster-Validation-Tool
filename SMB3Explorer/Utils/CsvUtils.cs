﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SMB3Explorer.Services;

namespace SMB3Explorer.Utils;

public static class CsvUtils
{
    public static readonly string DefaultDirectory =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer");

    private static string GetDefaultFilePath(string fileName) => Path.Combine(DefaultDirectory, fileName);

    public static async Task<(string, int)> ExportCsv<T>(ISystemInteropWrapper systemInteropWrapper,
        IAsyncEnumerable<T> records, string fileName, int limit = int.MaxValue) where T : notnull
    {
        if (!systemInteropWrapper.DirectoryExists(DefaultDirectory))
        {
            systemInteropWrapper.DirectoryCreate(DefaultDirectory);
        }
        
        var filePath = GetDefaultFilePath(fileName);
        
        if (systemInteropWrapper.FileExists(filePath)) systemInteropWrapper.FileDelete(filePath);
        await systemInteropWrapper.FileCreate(filePath);
        
        await using var writer = new StreamWriter(filePath);
        await using var csv = systemInteropWrapper.CreateCsvWriter();
        
        await csv.WriteHeaderAsync<T>();

        var rowCount = 1;
        var enumerator = records.GetAsyncEnumerator();
        while (await enumerator.MoveNextAsync())
        {
            await csv.WriteRecordAsync(enumerator.Current);

            if (rowCount >= limit)
            {
                break;
            }
            rowCount++;
        }

        await writer.FlushAsync();

        return (filePath, rowCount);
    }
}
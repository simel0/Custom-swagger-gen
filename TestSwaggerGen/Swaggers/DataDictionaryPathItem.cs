﻿using Microsoft.OpenApi.Models;

namespace TestSwaggerGen.Swaggers;

internal class DataDictionaryPathItem
{
    internal DataDictionaryPathItem(string key)
    {
        Key = key;
        PathItem = new OpenApiPathItem();
    }

    internal string Key { get; }
    internal OpenApiPathItem PathItem { get; }

    internal void AddOperation(OperationType type, OpenApiOperation operation)
    {
        PathItem.AddOperation(type, operation);
    }
}

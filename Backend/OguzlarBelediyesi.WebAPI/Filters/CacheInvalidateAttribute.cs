using System;

namespace OguzlarBelediyesi.WebAPI.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class CacheInvalidateAttribute : Attribute
{
    public string[] Tags { get; }

    public CacheInvalidateAttribute(params string[] tags)
    {
        Tags = tags;
    }
}

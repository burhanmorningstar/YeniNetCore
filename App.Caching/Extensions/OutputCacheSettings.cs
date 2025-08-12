namespace App.Caching.Extensions;

public class OutputCacheSettings
{
    public bool Enabled { get; init; } = true;
    public int ExpireSeconds { get; init; } = 600;
    public bool VaryByAllQuery { get; init; } = true;
    public string[]? VaryByHeaders { get; init; }
    public bool UseRedis { get; init; } = false;
}
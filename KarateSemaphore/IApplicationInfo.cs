using System;

namespace KarateSemaphore
{
    public interface IApplicationInfo
    {
        Version Version { get; }
        Version LatestVersion { get; }
        string Directory { get; }
    }
}

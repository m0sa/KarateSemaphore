using System;
using System.Reflection;

namespace KarateSemaphore
{
    public class AssemblyBasedApplicationInfo : IApplicationInfo
    {
        public AssemblyBasedApplicationInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            LatestVersion = Version = assembly.GetName().Version;
            Directory = assembly.Location;
        }

        public Version Version { get; private set; }
        public Version LatestVersion { get; private set; }
        public string Directory { get; private set; }
    }
}
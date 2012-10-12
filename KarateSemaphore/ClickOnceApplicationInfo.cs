using System;
using System.Deployment.Application;

namespace KarateSemaphore
{
    public class ClickOnceApplicationInfo :IApplicationInfo
    {
        public ClickOnceApplicationInfo()
        {
            Version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            LatestVersion = ApplicationDeployment.CurrentDeployment.UpdatedVersion;
            Directory = ApplicationDeployment.CurrentDeployment.DataDirectory;
        }

        public Version Version { get; private set; }
        public Version LatestVersion { get; private set; }
        public string Directory { get; private set; }
    }
}
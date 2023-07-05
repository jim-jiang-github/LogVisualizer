using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using LogVisualizer.Commons;
using GithubReleaseUpgrader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.I18N;
using MessageBox.Avalonia.DTO;
using Avalonia.Controls;
using MessageBox.Avalonia.Models;
using System.Runtime.InteropServices;

namespace LogVisualizer.Services
{
    public class UpgradeService
    {
        private UpgradeHandler _upgradeHandler;

        public UpgradeService(UpgradeHandler upgradeHandler)
        {
            _upgradeHandler = upgradeHandler;
        }

        public bool CheckForUpgrade(bool forceCheck = false)
        {
#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                return false;
            }
#endif
            var isNeedUpgrade = Upgrader.CheckForUpgrade(_upgradeHandler, forceCheck);
            return isNeedUpgrade;
        }

        public void PerformUpgradeIfNeeded()
        {
#if DEBUG
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) == true)
            {
                return;
            }
#endif
            Upgrader.PerformUpgradeIfNeeded();
        }
    }
}

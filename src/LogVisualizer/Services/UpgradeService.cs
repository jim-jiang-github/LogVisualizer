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

namespace LogVisualizer.Services
{
    public class UpgradeService
    {
        private UpgradeProgress _upgradeProgress;

        public UpgradeService(UpgradeProgress upgradeProgress)
        {
            _upgradeProgress = upgradeProgress;
        }

        public bool CheckForUpgrade(bool forceCheck = false)
        {
            var isNeedUpgrade = Upgrader.CheckForUpgrade(_upgradeProgress, forceCheck);
            return isNeedUpgrade;
        }

        public void PerformUpgradeIfNeeded()
        {
            Upgrader.PerformUpgradeIfNeeded();
        }
    }
}

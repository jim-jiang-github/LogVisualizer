﻿using System;
using Avalonia.Threading;
using GithubReleaseUpgrader;
using LogVisualizer.Commons;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.IO;
using System.Reflection;

namespace LogVisualizer.Platforms.Windows
{
    public class UpgradeHandlerOSX : UpgradeHandlerPlatform
    {
        public override string UpgradeResourceName { get; } = "osx-x64.zip";

        public override string ExecutableName { get; } = $"{Global.APP_NAME}";

        public override string UpgradeScriptName => "upgrader.sh";

        protected override string? GetUpgradeScriptContent()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using Stream? stream = assembly.GetManifestResourceStream($"LogVisualizer.Platforms.MacOS.{UpgradeScriptName}");
            if (stream == null)
            {
                Log.Warning("Can not found upgrader");
                return null;
            }
            using StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            return content;
        }
    }
}

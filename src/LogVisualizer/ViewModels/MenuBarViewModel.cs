﻿using Avalonia.Controls;
using Avalonia.Platform.Storage;
using LogVisualizer.Commons;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LogVisualizer.I18N;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LogVisualizer.ViewModels.LogDisplayViewModel;
using LogVisualizer.Decompress;

namespace LogVisualizer.ViewModels
{
    public partial class MenuBarViewModel : ViewModelBase
    {
        private readonly UpgradeService _upgradeService;
        private readonly ScenarioService _scenarioService;

        private IReadOnlyList<FilePickerFileType>? SupportedFileType
        {
            get
            {
                return new FilePickerFileType[]
                {
                    new(I18NKeys.OpenFileDialog_SupportedLogs.GetLocalizationRawValue())
                    {
                        Patterns = _scenarioService.SupportedLogExtension
                    }
                };
            }
        }

        public MenuBarViewModel(UpgradeService upgradeService, ScenarioService scenarioService)
        {
            _upgradeService = upgradeService;
            _scenarioService = scenarioService;
        }

        [RelayCommand]
        public async Task Open()
        {
            await Task.Run(async () =>
            {
                Loading.SetMessage(I18NKeys.Loading_OpenFileStart.GetLocalizationRawValue());
                var storageFiles = await GlobalStorageProvider.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                {
                    Title = I18NKeys.OpenFileDialog_PickLog.GetLocalizationRawValue(),
                    FileTypeFilter = SupportedFileType,
                    AllowMultiple = true
                });
                var paths = storageFiles
                    .Where(x => x.CanBookmark).Select(async x => await x.SaveBookmarkAsync())
                    .Select(x => x.Result)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Cast<string>()
                    .SelectMany(x =>
                    {
                        Loading.SetMessage(I18NKeys.Loading_LoadingFile.GetLocalizationString(x));
                        if (CompressedPackageLoader.IsSupportedCompressedPackage(x))
                        {
                            return CompressedPackageLoader.GetEntryPaths(x);
                        }
                        else
                        {
                            return new[] { x };
                        }
                    })
                    .ToArray();
                if (paths.Length == 0)
                {
                    return;
                }
                await _scenarioService.OpenLogSource(paths);
                Loading.SetProgress(1);
            }).WithLoadingMask();
        }

        [RelayCommand]
        public async Task FromUrl()
        {
        }
        private bool flag;
        [RelayCommand]
        public void Exit()
        {
            if (flag)
            {
                I18NManager.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            }
            else
            {
                I18NManager.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("zh");
            }
            flag = !flag;
        }

        [RelayCommand]
        public void ShowOnlyFiltered()
        {
        }

        [RelayCommand]
        public void AddNewFilter()
        {
        }

        [RelayCommand]
        public void OpenAppDataFolder()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Global.AppDataDirectory,
                UseShellExecute = true
            };
            process.Start();
        }

        [RelayCommand]
        public void CheckForUpgrade()
        {
            _upgradeService?.CheckForUpgrade(true);
        }

        [RelayCommand]
        public void About()
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = Global.GITHUB_URL,
                UseShellExecute = true
            };
            process.Start();
        }
    }
}

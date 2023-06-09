using Avalonia.Controls;
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
                    new(I18NKeys.Menu_Open_Pick_Log_Dialog_Supported_Logs.GetLocalizationRawValue())
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
            var storageFiles = await GlobalStorageProvider.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = I18NKeys.Menu_Open_Pick_Log_Dialog.GetLocalizationRawValue(),
                FileTypeFilter = SupportedFileType,
                AllowMultiple = true
            });
            await Task.Run(async () =>
            {
                var paths = storageFiles
                    .Where(x => x.CanBookmark).Select(async x => await x.SaveBookmarkAsync())
                    .Select(x => x.Result)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Cast<string>()
                    .SelectMany(x =>
                    {
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
                I18NManager.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            }
            else
            {
                I18NManager.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("zh-CN");
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

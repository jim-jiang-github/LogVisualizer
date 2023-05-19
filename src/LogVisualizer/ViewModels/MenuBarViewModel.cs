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

namespace LogVisualizer.ViewModels
{
    public partial class MenuBarViewModel : ViewModelBase
    {
        private UpgradeService _upgradeService;

        public MenuBarViewModel(UpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
        }

        [RelayCommand]
        public async Task Open()
        {

            List<FilePickerFileType>? GetFileTypes()
            {
                return new List<FilePickerFileType>
                            {
                                FilePickerFileTypes.All,
                                FilePickerFileTypes.TextPlain,
                                new("Binary Log")
                                {
                                    Patterns = new[] { "*.binlog", "*.buildlog" },
                                    MimeTypes = new[] { "application/binlog", "application/buildlog" },
                                    AppleUniformTypeIdentifiers = new []{ "public.data" }
                                }
                            };
            }
            var r = await GlobalStorageProvider.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open file",
                FileTypeFilter = GetFileTypes(),
                AllowMultiple = true
            });
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

using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogVisualizer.I18N;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using LogVisualizer.Commons;
using Avalonia.Controls.Shapes;
using System.IO;
using CommunityToolkit.Mvvm.Messaging;
using Avalonia.Controls;
using System.Collections;
using LogVisualizer.Models;
using Avalonia.Threading;

namespace LogVisualizer.ViewModels
{
    public partial class ScenarioConfigViewModel : ViewModelBase
    {
        public partial class ScenarioCreator : ObservableValidator
        {
            public class ScenarioConfigsFolderExistValidation : ValidationAttribute
            {
                public override bool IsValid(object? value)
                {
                    var gitService = DependencyInjectionProvider.GetService<GitService>();
                    if (value is string folderName)
                    {
                        string folder = System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, folderName);
                        if (!Directory.Exists(folder))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            private readonly ScenarioConfigViewModel _owner;
            private readonly GitService _gitService;
            private readonly DebounceDispatcher _debounceDispatcher;
            private CancellationTokenSource? _checkRepoCancellationTokenSource;

            [RegularExpression(@"^[^<>:""/\\|?*\x00-\x1F\x7F]+(\.[^<>:""/\\|?*\x00-\x1F\x7F]+)*$", ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Scenario_Creator_ScenarioNameValidNameError)]
            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            [ScenarioConfigsFolderExistValidation(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Scenario_Creator_ScenarioNameValidExist)]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Scenario_Creator_ScenarioNameValidNull)]
            [ObservableProperty]
            private string? _scenarioName = string.Empty;

            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Scenario_Creator_ScenarioRepoValidNull)]
            [ObservableProperty]
            private string? _scenarioRepo = string.Empty;

            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Scenario_Creator_ScenarioBranchValidNull)]
            [ObservableProperty]
            private string? _scenarioBranch = string.Empty;

            [ObservableProperty]
            private bool _isLoading = false;

            [ObservableProperty]
            private ObservableCollection<string> _allBranches;

            public ScenarioCreator(ScenarioConfigViewModel owner, GitService gitService)
            {
                _owner = owner;
                _gitService = gitService;
                _allBranches = new ObservableCollection<string>();
                _debounceDispatcher = new DebounceDispatcher();
            }

            [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanCreateScenario))]
            private async Task CreateScenario(CancellationToken token = default)
            {
                if (!HasErrors)
                {
                    _owner.Enabled = true;
                    string folder = System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, ScenarioName);
                    var result = await _gitService.Clone(ScenarioRepo, ScenarioBranch, folder, token);
                    if (!result)
                    {
                        FileOperationsHelper.SafeDeleteDirectory(folder);
                    }
                    _owner.Enabled = false;
                }
            }
            private bool CanCreateScenario()
            {
                ValidateAllProperties();
                return !HasErrors;
            }

            [RelayCommand]
            private void FetchBranches()
            {
                if (string.IsNullOrWhiteSpace(ScenarioRepo))
                {
                    return;
                }
                _checkRepoCancellationTokenSource?.Cancel();
                _checkRepoCancellationTokenSource = new CancellationTokenSource();

                _debounceDispatcher.Debounce(800, async (x) =>
                {
                    IsLoading = true;
                    var allBranches = await _gitService.GetAllOriginBranches(ScenarioRepo, _checkRepoCancellationTokenSource.Token);
                    AllBranches = new ObservableCollection<string>(allBranches);
                    IsLoading = false;
                });
            }
        }

        private GitService _gitService;
        private CancellationTokenSource? _filterBranchesCancellationTokenSource;
        private readonly DebounceDispatcher _debounceDispatcher;

        [ObservableProperty]
        private bool _enabled = true;
        [ObservableProperty]
        private string? _currentName;
        [ObservableProperty]
        private bool _hasSomeError = false;
        [ObservableProperty]
        private ObservableCollection<string> _filterBranches;
        [ObservableProperty]
        private ObservableCollection<ScenarioConfig> _scenarioConfigs;
        [ObservableProperty]
        private ScenarioConfig? _selectedScenarioConfig;
        [ObservableProperty]
        private ScenarioCreator _creator;

        public ScenarioConfigViewModel(GitService gitService)
        {
            Creator = new ScenarioCreator(this, gitService);
            _gitService = gitService;
            _filterBranches = new ObservableCollection<string>();
            _scenarioConfigs = new ObservableCollection<ScenarioConfig>();
            _debounceDispatcher = new DebounceDispatcher();

            LoadScenarioConfigs();
        }

        private void StartCheckForScenarioUpdate()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer(TimeSpan.FromSeconds(10), DispatcherPriority.ApplicationIdle, async (s, e) =>
            {
                foreach (var scenarioConfig in ScenarioConfigs)
                {
                    scenarioConfig.HasUpdate = await _gitService.HasUpdate(scenarioConfig.ScenarioConfigFolder);
                }
            });
            dispatcherTimer.Start();
        }

        private void LoadScenarioConfigs()
        {
            if (!Directory.Exists(Global.ScenarioConfigFolderRoot))
            {
                return;
            }
            var scenarioConfigFolders = Directory.GetDirectories(Global.ScenarioConfigFolderRoot);
            foreach (var scenarioConfigFolder in scenarioConfigFolders)
            {
                ScenarioConfig scenarioConfig = new()
                {
                    ScenarioName = System.IO.Path.GetFileNameWithoutExtension(scenarioConfigFolder)
                };
                ScenarioConfigs.Add(scenarioConfig);
            }
            Task.Run(async () =>
            {
                foreach (var scenarioConfig in ScenarioConfigs)
                {
                    scenarioConfig.ScenarioBranch = await _gitService.GetLocalBranchName(scenarioConfig.ScenarioConfigFolder, TimeSpan.FromSeconds(5));
                    scenarioConfig.ScenarioRepo = await _gitService.GetFolderGitRepo(scenarioConfig.ScenarioConfigFolder, TimeSpan.FromSeconds(5));
                    if (scenarioConfig.ScenarioRepo == null)
                    {
                        return;
                    }
                    var branches = await _gitService.GetAllOriginBranches(scenarioConfig.ScenarioRepo, TimeSpan.FromSeconds(60));
                    scenarioConfig.FilterBranches = new ObservableCollection<string>(branches);
                }
                StartCheckForScenarioUpdate();
            });
        }

        [RelayCommand]
        private async Task DeleteScenarioConfig(ScenarioConfig scenarioConfig)
        {
            var content = I18NKeys.Common_ConfirmDelete.GetLocalizationString(scenarioConfig.ScenarioName);
            if (await Notify.ShowComfirmMessageBox(content))
            {
                string folder = System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, scenarioConfig.ScenarioName);
                if (FileOperationsHelper.SafeDeleteDirectory(folder))
                {
                    ScenarioConfigs.Remove(scenarioConfig);
                }
            }
        }
    }
}

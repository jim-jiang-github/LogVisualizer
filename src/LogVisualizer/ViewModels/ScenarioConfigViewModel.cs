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
using LogVisualizer.Commons.Attributes;
using FluentValidation;
using System.Text.RegularExpressions;

namespace LogVisualizer.ViewModels
{
    public partial class ScenarioConfigViewModel : ViewModelBase
    {
        public partial class ScenarioCreatorViewModel : ViewModelBase, INotifyDataErrorInfo
        {
            public class ScenarioCreatorValidator : AbstractValidator<ScenarioCreatorViewModel>
            {
                public ScenarioCreatorValidator()
                {
                    RuleFor(x => x.ScenarioName)
                        .NotEmpty()
                        .When(x => x.ScenarioName != null)
                        .WithMessage(x =>
                        {
                            return I18NKeys.Scenario_Creator_ScenarioNameValidNull.GetLocalizationRawValue();
                        });
                    RuleFor(x => x.ScenarioName)
                        .Must(IsScenarioConfigsFolderExist)
                        //.When(x => x.ScenarioName != null)
                        .WithMessage(x =>
                        {
                            return I18NKeys.Scenario_Creator_ScenarioNameValidExist.GetLocalizationRawValue();
                        });
                    RuleFor(x => x.ScenarioName)
                        .Must(IsValidFileName)
                        .WithMessage(x =>
                        {
                            return I18NKeys.Scenario_Creator_ScenarioNameValidNameError.GetLocalizationRawValue();
                        });

                    RuleFor(x => x.ScenarioRepo)
                        .NotEmpty()
                        .When(x => x.ScenarioRepo != null)
                        .WithMessage(x =>
                        {
                            return I18NKeys.Scenario_Creator_ScenarioRepoValidNull.GetLocalizationRawValue();
                        });

                    RuleFor(x => x.ScenarioBranch)
                        .NotEmpty()
                        .When(x => x.ScenarioBranch != null)
                        .WithMessage(x =>
                        {
                            return I18NKeys.Scenario_Creator_ScenarioBranchValidNull.GetLocalizationRawValue();
                        });
                }

                public bool IsScenarioConfigsFolderExist(string? value)
                {
                    if (value == null)
                    {
                        return true;
                    }
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

                public bool IsValidFileName(string? value)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                    var isValid = Regex.IsMatch(value, @"^[^<>:""/\\|?*\x00-\x1F\x7F]+(\.[^<>:""/\\|?*\x00-\x1F\x7F]+)*$");
                    return isValid;
                }
            }

            private readonly ScenarioCreatorValidator _validator;
            private readonly ScenarioConfigViewModel _owner;
            private readonly GitService _gitService;
            private readonly DebounceDispatcher _debounceDispatcher;
            private CancellationTokenSource? _checkRepoCancellationTokenSource;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            private string? _scenarioName = null;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            private string? _scenarioRepo = null;

            [ObservableProperty]
            [NotifyCanExecuteChangedFor(nameof(CreateScenarioCommand))]
            private string? _scenarioBranch = null;

            [ObservableProperty]
            private bool _isFetchingBranches = false;

            [ObservableProperty]
            private ObservableCollection<string> _allBranches;

            public ScenarioCreatorViewModel(ScenarioConfigViewModel owner, GitService gitService)
            {
                _owner = owner;
                _gitService = gitService;
                _validator = new ScenarioCreatorValidator();
                _allBranches = new ObservableCollection<string>();
                _debounceDispatcher = new DebounceDispatcher();
            }

            private void Reset()
            {
                ScenarioName = null;
                ScenarioRepo = null;
                ScenarioBranch = null;
                IsFetchingBranches = false;
                AllBranches = new ObservableCollection<string>();
            }

            [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanCreateScenario))]
            [LogInfo]
            private async Task CreateScenario(CancellationToken cancellationToken = default)
            {
                if (ScenarioName == null || ScenarioRepo == null || ScenarioBranch == null)
                {
                    return;
                }
                if (!HasErrors)
                {
                    _owner.Enabled = false;
                    string folder = System.IO.Path.Combine(Global.ScenarioConfigFolderRoot, ScenarioName);
                    await _gitService.Clone(folder, ScenarioRepo, ScenarioBranch, cancellationToken);
                    _owner.Enabled = true;
                    _owner.LoadScenarioConfigs();
                    Reset();
                }
            }
            private bool CanCreateScenario()
            {
                return !string.IsNullOrEmpty(ScenarioName) &&
                    !string.IsNullOrEmpty(ScenarioRepo) &&
                    !string.IsNullOrEmpty(ScenarioBranch);
            }

            [RelayCommand]
            [LogInfo]
            private void FetchBranches()
            {
                if (string.IsNullOrWhiteSpace(ScenarioRepo))
                {
                    return;
                }
                _checkRepoCancellationTokenSource?.Cancel();
                _checkRepoCancellationTokenSource = new CancellationTokenSource();

                _debounceDispatcher.Debounce(400, async (x) =>
                {
                    IsFetchingBranches = true;
                    var allBranches = await _gitService.GetAllOriginBranches(ScenarioRepo, _checkRepoCancellationTokenSource.Token);
                    AllBranches = new ObservableCollection<string>(allBranches);
                    IsFetchingBranches = false;
                });
            }

            #region INotifyDataErrorInfo

            public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

            public bool HasErrors => !_validator.Validate(this).IsValid;

            public IEnumerable GetErrors(string? propertyName)
            {
                return _validator.Validate(this).Errors.Where(x => x.PropertyName == propertyName);
            }

            #endregion
        }

        private readonly INotify _notify;
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
        private ScenarioCreatorViewModel _creator;

        public ScenarioConfigViewModel(INotify notify, GitService gitService)
        {
            _notify = notify;
            Creator = new ScenarioCreatorViewModel(this, gitService);
            _gitService = gitService;
            _filterBranches = new ObservableCollection<string>();
            _scenarioConfigs = new ObservableCollection<ScenarioConfig>();
            _debounceDispatcher = new DebounceDispatcher();

            LoadScenarioConfigs();
        }

        [LogInfo]
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

        [LogInfo]
        private void LoadScenarioConfigs()
        {
            if (!Directory.Exists(Global.ScenarioConfigFolderRoot))
            {
                return;
            }
            ScenarioConfigs.Clear();
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
            if (await _notify.ShowComfirmMessageBox(content))
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

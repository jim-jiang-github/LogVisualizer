using CommunityToolkit.Mvvm.ComponentModel;
using LogVisualizer.Services;
using LogVisualizer.Extensions;
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

namespace LogVisualizer.ViewModels
{
    public partial class SchemaConfigViewModel : ViewModelBase
    {
        public partial class SchemaCreator : ObservableValidator
        {
            public class SchemaConfigsFolderExistValidation : ValidationAttribute
            {
                public override bool IsValid(object? value)
                {
                    var gitService = DependencyInjectionProvider.GetService<GitService>();
                    if (value is string folderName)
                    {
                        string folder = System.IO.Path.Combine(Global.SchemaConfigFolderRoot, folderName);
                        if (!Directory.Exists(folder))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            private readonly SchemaConfigViewModel _owner;
            private readonly GitService _gitService;
            private readonly DebounceDispatcher _debounceDispatcher;
            private CancellationTokenSource? _checkRepoCancellationTokenSource;

            [RegularExpression(@"^[a-zA-Z0-9_\-\.]+$", ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Schema_Creator_Schema_Name_Error)]
            [NotifyCanExecuteChangedFor(nameof(CreateSchemaCommand))]
            [SchemaConfigsFolderExistValidation(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Schema_Creator_Schema_Name_Exist)]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Schema_Creator_Schema_Name_Valid)]
            [ObservableProperty]
            private string? _schemaName = string.Empty;

            [NotifyCanExecuteChangedFor(nameof(CreateSchemaCommand))]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Schema_Creator_Schema_Repo_Valid)]
            [ObservableProperty]
            private string? _schemaRepo = string.Empty;

            [NotifyCanExecuteChangedFor(nameof(CreateSchemaCommand))]
            [Required(ErrorMessageResourceType = typeof(I18NResource), ErrorMessageResourceName = I18NResource.Schema_Creator_Schema_Branch_Valid)]
            [ObservableProperty]
            private string? _schemaBranch = string.Empty;

            [ObservableProperty]
            private bool _isLoading = false;

            [ObservableProperty]
            private ObservableCollection<string> _allBranches;

            public SchemaCreator(SchemaConfigViewModel owner, GitService gitService)
            {
                _owner = owner;
                _gitService = gitService;
                _allBranches = new ObservableCollection<string>();
                _debounceDispatcher = new DebounceDispatcher();
            }

            [RelayCommand(IncludeCancelCommand = true, CanExecute = nameof(CanCreateSchema))]
            private async Task CreateSchema(CancellationToken token = default)
            {
                if (!HasErrors)
                {
                    _owner.Enabled = true;
                    string folder = System.IO.Path.Combine(Global.SchemaConfigFolderRoot, SchemaName);
                    var result = await _gitService.CloneTo(SchemaRepo, SchemaBranch, folder, token);
                    if (!result)
                    {
                        FileOperationsHelper.SafeDeleteDirectory(folder);
                    }
                    _owner.Enabled = false;
                }
            }
            private bool CanCreateSchema()
            {
                ValidateAllProperties();
                return !HasErrors;
            }

            [RelayCommand]
            private void FetchBranches()
            {
                if (string.IsNullOrWhiteSpace(SchemaRepo))
                {
                    return;
                }
                _checkRepoCancellationTokenSource?.Cancel();
                _checkRepoCancellationTokenSource = new CancellationTokenSource();

                _debounceDispatcher.Debounce(800, async (x) =>
                {
                    IsLoading = true;
                    var allBranches = await _gitService.GetAllOriginBranches(SchemaRepo, true, _checkRepoCancellationTokenSource.Token);
                    AllBranches = new ObservableCollection<string>(allBranches);
                    IsLoading = false;
                });
            }
        }

        private GitService _gitService;

        [ObservableProperty]
        private bool _enabled = true;
        [ObservableProperty]
        private string? _currentName;
        [ObservableProperty]
        private bool _hasSomeError = false;
        [ObservableProperty]
        private ObservableCollection<string> _branches;
        [ObservableProperty]
        private ObservableCollection<SchemaConfig> _schemaConfigs;
        [ObservableProperty]
        private SchemaConfig? _selectedSchemaConfig;
        [ObservableProperty]
        private SchemaCreator _creator;

        public SchemaConfigViewModel(GitService gitService)
        {
            Creator = new SchemaCreator(this, gitService);
            _gitService = gitService;
            _branches = new ObservableCollection<string>();
            _schemaConfigs = new ObservableCollection<SchemaConfig>();

            LoadSchemaConfigs();
        }

        private void LoadSchemaConfigs()
        {
            var schemaConfigFolders = Directory.GetDirectories(Global.SchemaConfigFolderRoot);
            foreach (var schemaConfigFolder in schemaConfigFolders)
            {
                SchemaConfig schemaConfig = new()
                {
                    SchemaName = System.IO.Path.GetFileNameWithoutExtension(schemaConfigFolder)
                };
                SchemaConfigs.Add(schemaConfig);
            }
            foreach (var schemaConfig in SchemaConfigs)
            {
                var schemaConfigFolder = System.IO.Path.Combine(Global.SchemaConfigFolderRoot, schemaConfig.SchemaName);
                _gitService.GetLocalBranchName(schemaConfigFolder, TimeSpan.FromSeconds(10)).ContinueWith(c =>
                {
                    schemaConfig.SchemaBranch = c.Result;
                });
            }
        }
    }
}

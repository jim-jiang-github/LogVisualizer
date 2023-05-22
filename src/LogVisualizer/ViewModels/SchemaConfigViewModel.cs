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

namespace LogVisualizer.ViewModels
{
    public partial class SchemaConfigViewModel : ViewModelBase
    {
        public partial class SchemaCreate : ViewModelValidator
        {
            public class ValidationAttributeImpl : ValidationAttribute 
            {
                protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
                {
                    return base.IsValid(value, validationContext);
                }
            }
            private SchemaConfigViewModel _owner;

            [RegularExpression(@"^[a-zA-Z0-9_\-\.]+$")]
            [NotifyCanExecuteChangedFor(nameof(CreateSchemaCommand))]
            [ValidationAttributeImpl]
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

            public SchemaCreate(SchemaConfigViewModel owner)
            {
                _owner = owner;
            }

            [RelayCommand(CanExecute = nameof(CanCreateSchema))]
            private async Task CreateSchema(object schemaNameAndRepo)
            {
                if (!HasErrors)
                {
                    _owner.IsUpdating = true;
                    await Task.Delay(5555);
                    _owner.IsUpdating = false;
                }
                //ValidateAllProperties();
                //if (HasErrors)
                //{
                //    return;
                //}
                //int a = 1;
                //return;
                //try
                //{
                //    if (schemaNameAndRepo is not ReadOnlyCollection<object> x)
                //    {
                //        return;
                //    }
                //    string schemaName = (string)x[0];
                //    string schemaRepo = (string)x[1];
                //    string schemaBranch = (string)x[2];
                //    var result = await _gitService.CloneTo(schemaRepo, schemaBranch, schemaName);
                //}
                //catch (OperationCanceledException)
                //{
                //}
            }
            private bool CanCreateSchema(object schemaNameAndRepo)
            {
                ValidateAllProperties();
                return !HasErrors;
                return false;
                if (schemaNameAndRepo is not ReadOnlyCollection<object> x)
                {
                    return false;
                }
                if (x.Count != 3)
                {
                    return false;
                }
                if (x[0] is not string schemaName || x[1] is not string schemaRepo || x[2] is not string schemaBranch)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(schemaName) || string.IsNullOrEmpty(schemaRepo) || string.IsNullOrEmpty(schemaBranch))
                {
                    return false;
                }
                return true;
            }
        }

        private GitService _gitService;

        [ObservableProperty]
        private bool _isUpdating = false;
        [ObservableProperty]
        private string? _currentName;
        [ObservableProperty]
        private bool _needsAttention = false;
        [ObservableProperty]
        private ObservableCollection<string> _branches;
        [ObservableProperty]
        private SchemaCreate _create;

        public SchemaConfigViewModel(GitService gitService)
        {
            Create = new SchemaCreate(this);
            _gitService = gitService;
            _branches = new ObservableCollection<string>();
            NeedsAttention = true;
            I18NKeys.Schema_No_Source.BindingExpression(this, x => x.CurrentName);

            IsUpdating = true;
            _ = Task.Run(async () =>
            {
                IsUpdating = false;
            });
        }
    }
}

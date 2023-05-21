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

namespace LogVisualizer.ViewModels
{
    public partial class SchemaConfigViewModel : ViewModelBase
    {
        private GitService _gitService;

        [ObservableProperty]
        private string? _currentName;
        [ObservableProperty]
        private bool _needsAttention = false;
        [ObservableProperty]
        private ObservableCollection<string> _branches;

        public SchemaConfigViewModel(GitService gitService)
        {
            _gitService = gitService;
            _branches = new ObservableCollection<string>();
            NeedsAttention = true;
            I18NKeys.Schema_No_Source.BindingExpression(this, x => x.CurrentName);

            //_ = Task.Run(async () =>
            //{
            //    var branches = await gitService.GetAllOriginBranches(Configuration.Instance.Schema?.SchemaRepo).WithLoadingMask();
            //    Branches = new ObservableCollection<string>(branches);
            //});
        }

        [RelayCommand(CanExecute = nameof(CanCreateSchema))]
        private async Task CreateSchema(object schemaNameAndRepo)
        {
            try
            {
                if (schemaNameAndRepo is not ReadOnlyCollection<object> x)
                {
                    return;
                }
                string schemaName = (string)x[0];
                string schemaRepo = (string)x[1];
                string schemaBranch = (string)x[2];
                var result = await _gitService.CloneTo(schemaRepo, schemaBranch, schemaName);
            }
            catch (OperationCanceledException)
            {
            }
        }
        private bool CanCreateSchema(object schemaNameAndRepo)
        {
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
}

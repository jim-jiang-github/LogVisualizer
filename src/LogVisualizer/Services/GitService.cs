using Avalonia.Controls;
using CliWrap;
using LogVisualizer.Commons;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace LogVisualizer.Services
{
    public class GitService
    {
        private const string BRANCH_NAME_HEAD = "refs/heads/";

        public async Task<bool> CloneTo(string gitRepo, string branch, string folder, CancellationToken cancellationToken = default)
        {
            if (!FileOperationsHelper.SafeResetDirectory(folder))
            {
                FileOperationsHelper.SafeDeleteDirectory(folder);
                return false;
            }
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var cmd = await Cli.Wrap("git")
                     .WithWorkingDirectory(folder)
                     .WithArguments(args => args
                     .Add("clone")
                     .Add(gitRepo)
                     .Add("--depth=1")
                     .Add("-b")
                     .Add(branch)
                     .Add(folder)
                     )
                     .WithStandardErrorPipe(PipeTarget.ToDelegate((msg) =>
                     {
                         stringBuilder.AppendLine(msg);
                     }))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(cancellationToken);
                Log.Information("Execute result: {StartTime} {RunTime} {ExitTime} {ExitCode}",
                    cmd.StartTime,
                    cmd.RunTime,
                    cmd.ExitTime,
                    cmd.ExitCode);
                if (cmd.ExitCode != 0)
                {
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                    FileOperationsHelper.SafeDeleteDirectory(folder);
                }
                else
                {
                    Log.Information("Branch: {branch} cloned!", branch);
                }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Information("User canceled!");
                }
                else
                {
                    stringBuilder.AppendLine(ex.Message);
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
                FileOperationsHelper.SafeDeleteDirectory(folder);
            }
            return true;
        }

        public async Task<IEnumerable<string>> GetAllOriginBranches(string gitRepo, bool isSimplify = true, CancellationToken cancellationToken = default)
        {
            List<string> branches = new List<string>();
            if (gitRepo == null)
            {
                Log.Information("gitRepo: {StartTime} is null.");
                return branches;
            }
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var cmd = await Cli.Wrap("git")
                    .WithArguments(args => args
                    .Add("ls-remote")
                    .Add("--head")
                    .Add(gitRepo))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate((branch) =>
                    {
                        if (isSimplify)
                        {
                            var match = Regex.Match(branch, "(.*)\\t(.*)");
                            if (match.Success && match.Groups.Count == 3)
                            {
                                var id = match.Groups[1];
                                var name = match.Groups[2];
                                var branchName = name.Value.Replace(BRANCH_NAME_HEAD, "");
                                branches.Add(branchName);
                            }
                        }
                        else
                        {
                            branches.Add(branch);
                        }
                    }))
                    .WithStandardErrorPipe(PipeTarget.ToDelegate((err) =>
                    {
                        stringBuilder.AppendLine(err);
                    }))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(cancellationToken);
                Log.Information("Execute result: {StartTime} {RunTime} {ExitTime} {ExitCode}",
                    cmd.StartTime,
                    cmd.RunTime,
                    cmd.ExitTime,
                    cmd.ExitCode);
                if (cmd.ExitCode != 0)
                {
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
                else
                {
                    Log.Information(string.Join("\r\n", branches));
                }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Information("User canceled!");
                }
                else
                {
                    stringBuilder.AppendLine(ex.Message);
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
            }
            return branches;
        }

        public Task<string?> GetLocalBranchName(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return GetLocalBranchName(folder, cancellationTokenSource.Token);
        }

        public async Task<string?> GetLocalBranchName(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Information("gitRepo: {StartTime} is null.");
                return null;
            }
            string? branchName = null;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var cmd = await Cli.Wrap("git")
                    .WithWorkingDirectory(folder)
                    .WithArguments(args => args
                    .Add("branch")
                    .Add("--show-current"))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate((branch) =>
                    {
                        branchName = branch;
                    }))
                    .WithStandardErrorPipe(PipeTarget.ToDelegate((err) =>
                    {
                        stringBuilder.AppendLine(err);
                    }))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(cancellationToken);
                Log.Information("Execute result: {StartTime} {RunTime} {ExitTime} {ExitCode}",
                    cmd.StartTime,
                    cmd.RunTime,
                    cmd.ExitTime,
                    cmd.ExitCode);
                if (cmd.ExitCode != 0)
                {
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
                else
                {
                    Log.Information(string.Join("\r\n", branchName));
                }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Information("User canceled!");
                }
                else
                {
                    stringBuilder.AppendLine(ex.Message);
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
            }
            return branchName;

        }

        public async Task<bool> HasGitRepo(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Information("gitRepo: {StartTime} is null.");
                return false;
            }
            bool hasGitRepo = false;
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                var cmd = await Cli.Wrap("git")
                    .WithWorkingDirectory(folder)
                    .WithArguments(args => args
                    .Add("rev-parse")
                    .Add("--is-inside-work-tree"))
                    .WithStandardOutputPipe(PipeTarget.ToDelegate((branch) =>
                    {
                        if (bool.TryParse(branch, out bool result))
                        {
                            hasGitRepo = result;
                        }
                        else
                        {
                            hasGitRepo = false;
                        }
                    }))
                    .WithStandardErrorPipe(PipeTarget.ToDelegate((err) =>
                    {
                        stringBuilder.AppendLine(err);
                    }))
                    .WithValidation(CommandResultValidation.None)
                    .ExecuteAsync(cancellationToken);
                Log.Information("Execute result: {StartTime} {RunTime} {ExitTime} {ExitCode}",
                    cmd.StartTime,
                    cmd.RunTime,
                    cmd.ExitTime,
                    cmd.ExitCode);
                if (cmd.ExitCode != 0)
                {
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
                else
                {
                    Log.Information("result:{result}", hasGitRepo);
                }
            }
            catch (Exception ex)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Log.Information("User canceled!");
                }
                else
                {
                    stringBuilder.AppendLine(ex.Message);
                    var errorMsg = stringBuilder.ToString();
                    Log.Warning(errorMsg);
                }
            }
            return hasGitRepo;

        }
    }
}

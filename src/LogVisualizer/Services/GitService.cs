using Avalonia.Controls;
using CliWrap;
using LogVisualizer.Commons;
using ReverseMarkdown.Converters;
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

        public Task<bool> Clone(string folder, string gitRepo, string branch, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return Clone(folder, gitRepo, branch, cancellationTokenSource.Token);
        }

        public Task<bool> Clone(string folder, string gitRepo, string branch, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(folder) && !FileOperationsHelper.IsValidFileName(Path.GetFileName(folder)))
            {
                Log.Error("folder:{folder} is not a vaild path.", folder);
                return Task.FromResult(false);
            }
            if (!string.IsNullOrEmpty(folder))
            {
                if (!FileOperationsHelper.SafeResetDirectory(folder))
                {
                    FileOperationsHelper.SafeDeleteDirectory(folder);
                    return Task.FromResult(false);
                }
            }
            var result = ExecuteGitCommand(null, null, (str) =>
            {
                return str.StartsWith("Cloning into");
            }, cancellationToken, "clone", gitRepo, "--depth=1", "-b", branch, folder);
            return result;
        }

        public Task<IEnumerable<string>> GetAllOriginBranches(string gitRepo, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return GetAllOriginBranches(gitRepo, cancellationTokenSource.Token);
        }

        public async Task<IEnumerable<string>> GetAllOriginBranches(string gitRepo, CancellationToken cancellationToken = default)
        {
            List<string> branches = new List<string>();
            if (gitRepo == null)
            {
                Log.Information("gitRepo: {gitRepo} is null.");
                return branches;
            }
            var result = await ExecuteGitCommand(null, (allBranchs) =>
            {
                string[] branches = allBranchs.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                var result = branches.Select(branch =>
                {
                    var match = Regex.Match(branch, "(.*)\\t(.*)");
                    if (match.Success && match.Groups.Count == 3)
                    {
                        var id = match.Groups[1];
                        var name = match.Groups[2];
                        var branchName = name.Value.Replace(BRANCH_NAME_HEAD, "");
                        return branchName;
                    }
                    return string.Empty;
                })
                .Where(x => x != string.Empty);
                return result;
            }, null, cancellationToken, "ls-remote", "--head", gitRepo);
            return result ?? branches;
        }

        public Task<string?> GetLocalBranchName(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return GetLocalBranchName(folder, cancellationTokenSource.Token);
        }

        public Task<string?> GetLocalBranchName(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return Task.FromResult<string?>(null);
            }
            var result = ExecuteGitCommand(folder, (str) => str, null, cancellationToken, "branch", "--show-current");
            return result;
        }

        public Task<bool> HasGitRepo(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return HasGitRepo(folder, cancellationTokenSource.Token);
        }

        public async Task<bool> HasGitRepo(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return false;
            }
            var result = await ExecuteGitCommand(folder, (branch) =>
            {
                if (bool.TryParse(branch, out bool result))
                {
                    return result;
                }
                else
                {
                    return false;
                }
            }, null, cancellationToken, "rev-parse", "--is-inside-work-tree");
            return result;
        }

        public Task<string?> GetFolderGitRepo(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return GetFolderGitRepo(folder, cancellationTokenSource.Token);
        }

        public async Task<string?> GetFolderGitRepo(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return null;
            }

            var result = await ExecuteGitCommand(folder, (repo) => repo, null, cancellationToken, "remote", "get-url", "origin");
            return result;
        }

        public Task<bool> FetchRemote(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return FetchRemote(folder, cancellationTokenSource.Token);
        }

        public async Task<bool> FetchRemote(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return false;
            }
            var result = await ExecuteGitCommand(folder, null, (msg) =>
            {
                var result = !msg.Contains("fatal");
                return result;
            }, cancellationToken, "fetch", "origin");
            return result;
        }

        public Task<bool> HasUpdate(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return HasUpdate(folder, cancellationTokenSource.Token);
        }

        public async Task<bool> HasUpdate(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return false;
            }
            var success = await FetchRemote(folder);
            if (!success)
            {
                return false;
            }
            var result = await ExecuteGitCommand(folder, (msg) =>
            {
                if (msg.Contains("Your branch is up to date"))
                {
                    return false;
                }
                return true;
            }, null, cancellationToken, "status");
            return result;
        }

        public Task<bool> Pull(string folder, TimeSpan timeout)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter((int)timeout.TotalMilliseconds);
            return Pull(folder, cancellationTokenSource.Token);
        }

        public async Task<bool> Pull(string folder, CancellationToken cancellationToken = default)
        {
            if (folder == null)
            {
                Log.Warning("folder: {folder} is null.", folder);
                return false;
            }
            var result = await ExecuteGitCommand(folder, (msg) =>
            {
                if (msg.Contains("Already up to date"))
                {
                    return false;
                }
                return true;
            }, null, cancellationToken, "pull");
            return result;
        }

        public async Task<T?> ExecuteGitCommand<T>(string? folder, Func<string, T>? convertorStandard, Func<string, T>? convertorError, CancellationToken cancellationToken = default, params string[] gitParams)
        {
            T? result = default;
            StringBuilder stringBuilderStandard = new();
            StringBuilder stringBuilderError = new();
            try
            {
                var cmd = await Cli.Wrap("git")
                    .WithWorkingDirectory(folder)
                    .WithArguments(args =>
                    {
                        foreach (var gitParam in gitParams)
                        {
                            args.Add(gitParam);
                        }
                    })
                    .WithStandardOutputPipe(PipeTarget.ToDelegate((output) =>
                    {
                        stringBuilderStandard.AppendLine(output);
                    }))
                    .WithStandardErrorPipe(PipeTarget.ToDelegate((err) =>
                    {
                        stringBuilderError.AppendLine(err);
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
                    var errorMsg = stringBuilderError.ToString();
                    Log.Warning(errorMsg);
                }
                else
                {
                    Log.Information("result:{result}", result);
                }
                if (convertorStandard != null)
                {
                    var standardOutput = stringBuilderStandard.ToString().TrimEnd('\r', '\n');
                    result = convertorStandard.Invoke(standardOutput);
                }
                if (convertorError != null)
                {
                    var errorMsg = stringBuilderError.ToString().TrimEnd('\r', '\n');
                    result = convertorError.Invoke(errorMsg);
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
                    stringBuilderError.AppendLine(ex.Message);
                    var errorMsg = stringBuilderError.ToString();
                    Log.Warning(errorMsg);
                }
            }
            return result;
        }
    }
}

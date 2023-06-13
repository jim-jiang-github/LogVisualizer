using LogVisualizer.Commons;
using LogVisualizer.Services;

namespace LogVisualizer.Test
{
    public class GitServiceTest
    {
        private GitService _gitService;

        public GitServiceTest(GitService gitService)
        {
            _gitService = gitService;
        }

        [Theory]
        [InlineData("TestFolder", Global.GITHUB_URL, "dev", true)]
        [InlineData("TestFolder", Global.GITHUB_URL, "xxxxx", false)]
        //[InlineData("~!@#$%^&*().,/?;'", Global.GITHUB_URL, "dev", false)]
        //[InlineData("TestFolder", Global.GITHUB_URL, "xxxx", false)]
        public async Task Clone(string folder, string gitRepo, string branch, bool expected)
        {
            var dir = Path.Combine(Global.AppTempDirectory, folder);
            var result = await _gitService.Clone(dir, gitRepo, branch);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, new string[0])]
        [InlineData("", new string[0])]
        [InlineData("xxxx", new string[0])]
        public async Task GetAllOriginBranches(string githubUrl, string[]? expected)
        {
            var branchs = await _gitService.GetAllOriginBranches(githubUrl);
            Assert.Equal(expected, branchs);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "dev")]
        [InlineData("xxxqwex", null)]
        public async Task GetLocalBranchName(string folder, string? expected)
        {
            var branchName = await _gitService.GetLocalBranchName(folder);
            Assert.Equal(expected, branchName);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", true)]
        [InlineData("xxxx", false)]
        public async Task HasGitRepo(string folder, bool expected)
        {
            var hasGitRepo = await _gitService.HasGitRepo(folder);
            Assert.Equal(expected, hasGitRepo);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("xxxx", null)]
        public async Task GetFolderGitRepo(string folder, string expected)
        {
            var repo = await _gitService.GetFolderGitRepo(folder);
            Assert.Equal(expected, repo);
        }

        [Theory]
        [InlineData("TestFolder", true)]
        [InlineData("xxxx", false)]
        public async Task FetchRemote(string folder, bool expected)
        {
            var dir = Path.Combine(Global.AppTempDirectory, folder);
            var result = await _gitService.FetchRemote(dir);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("xxxx", false)]
        public async Task HasUpdate(string folder, bool expected)
        {
            var dir = Path.Combine(Global.AppTempDirectory, folder);
            var result = await _gitService.HasUpdate(dir);
            Assert.Equal(expected, result);
        }
    }
}
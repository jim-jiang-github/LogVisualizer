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
        [InlineData(null, new string[0])]
        [InlineData("", new string[0])]
        [InlineData("xxxx", new string[0])]
        [InlineData(Global.GITHUB_URL, new string[] { "dev" })]
        public async Task GetAllOriginBranches(string githubUrl, string[]? expected)
        {
            var branchs = await _gitService.GetAllOriginBranches(githubUrl);
            Assert.Equal(expected, branchs);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "dev")]
        [InlineData("xxxx", null)]
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
    }
}
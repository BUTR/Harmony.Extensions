using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NuGet;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.NuGet.NuGetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.Pack);

    string Version => $"2.0.0.{(Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER") is { } env && !string.IsNullOrEmpty(env) ? env : "0")}";

    [Solution] readonly Solution Solution;

    Project IsExternalInitProject => Solution.GetProject("Harmony.Extensions");

    IEnumerable<FileInfo> AttributeFiles => Directory
        .GetFiles(IsExternalInitProject.Directory, "*.cs", SearchOption.AllDirectories)
        .Select(path => new FileInfo(path));

    FileInfo NuspecFile => new(RootDirectory / "src" / "Harmony.Extensions.nuspec");

    DirectoryInfo ArtifactsDirectory => new(RootDirectory / "artifacts");

    DirectoryInfo OutDirectory => new(TemporaryDirectory / "out");

    DirectoryInfo ExcludeFromCodeCoverageOutDirectory => new(TemporaryDirectory / "out" / "ExcludeFromCodeCoverage");

    DirectoryInfo NoExcludeFromCodeCoverageOutDirectory => new(TemporaryDirectory / "out" / "NoExcludeFromCodeCoverage");

    Target Clean => _ => _
        .Executes(() =>
        {
            DeleteDirectory(ArtifactsDirectory.FullName);
            DeleteDirectory(ExcludeFromCodeCoverageOutDirectory.FullName);
            DeleteDirectory(NoExcludeFromCodeCoverageOutDirectory.FullName);
        });

    Target Compile => _ => _
        .Executes(() =>
        {
            // We build once to verify that the sources don't contain errors.
            // Has no function apart from that since the build output is not used.
            DotNetBuild(s => s
                .SetProjectFile(IsExternalInitProject.Path)
                .SetIgnoreFailedSources(true));
        });

    Target CreateOutFiles => _ => _
        .DependsOn(Clean)
        .DependsOn(Compile)
        .Executes(() =>
        {
            // IsExternalInit uses the [ExcludeFromCodeCoverage] attribute.
            // This is not available in certain target frameworks (e.g. .NET 2.0 and .NET Standard 1.0).
            // For this reason, we remove that attribute from the source code.
            // This results in two different file versions. These are temporarily stored in the output directories
            // and then used by the nuspec accordingly.
            ExcludeFromCodeCoverageOutDirectory.Create();
            NoExcludeFromCodeCoverageOutDirectory.Create();

            foreach (var attributeFile in AttributeFiles)
            {
                attributeFile.CopyTo(Path.Combine(ExcludeFromCodeCoverageOutDirectory.FullName, attributeFile.Name));
                attributeFile.CopyTo(Path.Combine(NoExcludeFromCodeCoverageOutDirectory.FullName, attributeFile.Name));
            }

            foreach (var noExcludeFromCodeCoverageFile in NoExcludeFromCodeCoverageOutDirectory.GetFiles())
            {
                var content = File.ReadAllText(noExcludeFromCodeCoverageFile.FullName);
                content = content.Replace("ExcludeFromCodeCoverage, ", "");
                File.WriteAllText(noExcludeFromCodeCoverageFile.FullName, content);
            }

            // Renaming the .cs files to .cs.pp ensures that the files aren't listed in VS's solution explorer
            // when the package is consumed via NuGet. With .cs files, that happens.
            foreach (var csFile in OutDirectory.GetFiles("*.cs", SearchOption.AllDirectories))
            {
                csFile.MoveTo($"{csFile.FullName}.pp");
            }
        });

    Target Pack => _ => _
        .DependsOn(CreateOutFiles)
        .Executes(() =>
        {
            NuGetPack(s => s
                .SetTargetPath(NuspecFile.FullName)
                .SetBasePath(OutDirectory.FullName)
                .SetOutputDirectory(ArtifactsDirectory.FullName)
                .SetVersion(Version));
        });
}

#l "nuget:?package=Cake.Storm.Fluent"
#l "nuget:?package=Cake.Storm.Fluent.DotNetCore"
#l "nuget:?package=Cake.Storm.Fluent.NuGet"

Configure()
	.UseRootDirectory("..")
	.UseBuildDirectory("build")
	.UseArtifactsDirectory("artifacts")
	.AddConfiguration(configuration => configuration
		.WithSolution("Sources/AutoFindViews.sln")
        .WithTargetFrameworks("net46")
		.WithBuildParameter("Configuration", "Release")
		.WithBuildParameter("Platform", "Any CPU")
		.UseDefaultTooling()
		.UseDotNetCoreTooling()
        .WithDotNetCoreOutputType(OutputType.Copy)
	)
	//platforms configuration
	.AddPlatform("dotnet")
	//targets configuration
	.AddTarget("pack", configuration => configuration
        .UseNugetPack(nugetConfiguration => nugetConfiguration.WithAuthor("Valentin Jubert, Julien Mialon"))
	)
    .AddTarget("push", configuration => configuration
        .UseNugetPack(nugetConfiguration => nugetConfiguration.WithAuthor("Valentin Jubert, Julien Mialon"))
        .UseNugetPush(nugetConfiguration => nugetConfiguration.WithApiKeyFromEnvironment())
    )
    //applications configuration
	.AddApplication("android-colors", configuration => configuration
        .WithProject("Sources/AutoFindViews.Build/AutoFindViews.Build.csproj")
        .WithVersion("0.8.0")
        .UseNugetPack(nugetConfiguration => nugetConfiguration
            .WithNuspec("NuGet/Package.nuspec")
            .WithPackageId("Ideine.AutoFindViews")
            .WithReleaseNotesFile("NuGet/notes.md")
            .AddFileFromArtifacts("net46/AutoFindViews.Build.dll", "colors")
            //dependencies
            /* 
            .AddFile("src/Storm.BuildTasks.AndroidColors/bin/Release/net46/Storm.BuildTasks.Common.dll", "colors")
            .AddFile("src/Storm.BuildTasks.AndroidColors/bin/Release/net46/Microsoft.CodeAnalysis.CSharp.dll", "colors")
            .AddFile("src/Storm.BuildTasks.AndroidColors/bin/Release/net46/Microsoft.CodeAnalysis.dll", "colors")
            */
            //props & target file
            .AddFile("Sources/AutoFindViews.Build/AutoFindViews.targets", "build/monoandroid")
            //.AddFile("src/Storm.BuildTasks.AndroidColors/Storm.BuildTasks.AndroidColors.targets", "build/monoandroid")
        )
    )
	.Build();

RunTarget(Argument("target", "help"));
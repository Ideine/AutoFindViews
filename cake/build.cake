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
            .AddFileFromArtifacts("net46/AutoFindViews.Build.dll", "autofindviews")
            //dependencies
            .AddFile("Sources/AutoFindViews.Build/bin/Release/net46/Newtonsoft.Json.dll", "autofindviews")
            //props & target file
            .AddFile("Sources/AutoFindViews.Build/AutoFindViews.targets", "build/monoandroid")
        )
    )
	.Build();

RunTarget(Argument("target", "help"));
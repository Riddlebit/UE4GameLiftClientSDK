using UnrealBuildTool;

public class AWSCoreSDK : ModuleRules
{
	public AWSCoreSDK(ReadOnlyTargetRules Target ): base(Target)
	{

		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
    
		PublicDependencyModuleNames.AddRange(
			new []
			{
				"Core",
				"Projects"
			}
		);
		
		PublicIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Public")});
		PrivateIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Private")});

		string BaseDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(ModuleDirectory, "..", ".."));
        string SDKDirectory = System.IO.Path.Combine(BaseDirectory, "ThirdParty", "AWSCoreSDK", Target.Platform.ToString());
        
        bool bHasAWSCoreSDK = System.IO.Directory.Exists(SDKDirectory);

        if (!bHasAWSCoreSDK || Target.Type != TargetRules.TargetType.Client) return;
		
		string AWSCoreLibFile = System.IO.Path.Combine(SDKDirectory, "aws-cpp-sdk-core.lib");
		if (System.IO.File.Exists(AWSCoreLibFile))
		{
			PublicAdditionalLibraries.Add(AWSCoreLibFile);
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-core.lib not found. Expected in this location: " + AWSCoreLibFile);
		}

		string AWSCoreDLLFile = System.IO.Path.Combine(SDKDirectory, "aws-cpp-sdk-core.dll");
		if (System.IO.File.Exists(AWSCoreDLLFile))
		{
            PublicDelayLoadDLLs.Add("aws-cpp-sdk-core.dll");
            RuntimeDependencies.Add(AWSCoreDLLFile);
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-core.dll not found. Expected in this location: " + AWSCoreDLLFile);
		}
	}
}

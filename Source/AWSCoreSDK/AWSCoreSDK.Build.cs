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

        if (!bHasAWSCoreSDK || Target.Type != TargetRules.TargetType.Client)
        {
	        PublicDefinitions.Add("WITH_AWS_CORE=0");
	        return;
        }
        
        PublicDefinitions.AddRange( new []
        {
	        "WITH_AWS_CORE=1",
	        "USE_IMPORT_EXPORT=1",
	        "USE_WINDOWS_DLL_SEMANTICS=1"
        });
		
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

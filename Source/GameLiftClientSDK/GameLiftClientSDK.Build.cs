using UnrealBuildTool;

public class GameLiftClientSDK : ModuleRules
{
	public GameLiftClientSDK(ReadOnlyTargetRules Target) : base(Target)
	{
		
		PublicDependencyModuleNames.AddRange(
			new []
			{
				"Engine", 
				"Core", 
				"CoreUObject",
				"InputCore",
				"Projects"
			}
		);

		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		PublicIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Public")});
		PrivateIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Private")});

		string BaseDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(ModuleDirectory, "..", ".."));
        string SDKDirectory = System.IO.Path.Combine(BaseDirectory, "ThirdParty", "GameLiftClientSDK", "Win64");

        if (Target.Type != TargetType.Client)
        {
	        PublicDefinitions.Add("WITH_GAMELIFT_CLIENT=0");
	        return;   
        }

        if (!System.IO.Directory.Exists(SDKDirectory))
        {
	        throw new BuildException("Could not find the CognitoIdentitySDK. Expected at: " + SDKDirectory);
        }
        
        PublicDefinitions.AddRange( new []
        {
	        "WITH_GAMELIFT_CLIENT=1",
	        "USE_IMPORT_EXPORT=1",
	        "USE_WINDOWS_DLL_SEMANTICS=1"
        });
        
		string CognitoLibFile = System.IO.Path.Combine(SDKDirectory, "aws-cpp-sdk-cognito-identity.lib");
		if (System.IO.File.Exists(CognitoLibFile))
		{
			PublicAdditionalLibraries.Add(CognitoLibFile);
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-cognito-identity.lib not found. Expected in this location: " + CognitoLibFile);
		}

		string CognitoDLLFile = System.IO.Path.Combine(SDKDirectory, "aws-cpp-sdk-cognito-identity.dll");
		if (System.IO.File.Exists(CognitoDLLFile))
		{
			PublicDelayLoadDLLs.Add("aws-cpp-sdk-cognito-identity.dll");
			RuntimeDependencies.Add(CognitoDLLFile);
			RuntimeDependencies.Add(System.IO.Path.Combine(BaseDirectory, "..", "..", "Binaries", "Win64"),
				System.IO.Path.Combine(SDKDirectory, "Dependencies", "*.dll"));
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-cognito-identity.dll not found. Expected in this location: " + CognitoDLLFile);
		}
		
		string AWSCoreLibFile = System.IO.Path.Combine(SDKDirectory, "aws-cpp-sdk-core.lib");
		if (System.IO.File.Exists(AWSCoreLibFile))
		{
			PublicAdditionalLibraries.Add(AWSCoreLibFile);
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-core.lib not found. Expected in this location: " + AWSCoreLibFile);
		}
	}
}

using UnrealBuildTool;

public class CognitoIdentitySDK : ModuleRules
{
	public CognitoIdentitySDK(ReadOnlyTargetRules Target) : base(Target)
	{
		
		PublicIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Public")});
		PrivateIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Private")});
		
		PublicDependencyModuleNames.AddRange(
			new []
			{
				"Core",
				"Projects"
			}
		);

		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;

		string BaseDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(ModuleDirectory, "..", ".."));
        string SDKDirectory = System.IO.Path.Combine(BaseDirectory, "ThirdParty", "CognitoIdentitySDK", Target.Platform.ToString());
        
        bool bHasCognitoIdentity = System.IO.Directory.Exists(SDKDirectory);

        if (!bHasCognitoIdentity || Target.Type != TargetRules.TargetType.Client)
        {
	        PublicDefinitions.Add("WITH_COGNITO_IDENTITY=0");
	        return;   
        }
        
        PublicDefinitions.Add("WITH_COGNITO_IDENTITY=1");
        
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
		}
		else
		{
			throw new BuildException("aws-cpp-sdk-cognito-identity.dll not found. Expected in this location: " + CognitoDLLFile);
		}
	}
}

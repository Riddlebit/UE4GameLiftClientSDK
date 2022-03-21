using UnrealBuildTool;

public class GameLiftClientSDK : ModuleRules
{
	public GameLiftClientSDK(ReadOnlyTargetRules Target ): base(Target)
	{

		PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
    
		PublicDependencyModuleNames.AddRange(
			new []
			{
				"Core",
				"Projects",
				"AWSCoreSDK",
				"CognitoIdentitySDK"
			}
		);
		
		PublicIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Public")});
		PrivateIncludePaths.AddRange(new [] {System.IO.Path.Combine(ModuleDirectory, "Private")});

	}
}
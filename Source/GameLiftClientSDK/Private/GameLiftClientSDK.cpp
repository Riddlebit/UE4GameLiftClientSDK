#include "GameLiftClientSDK.h"
#include "Core.h"

#if PLATFORM_WINDOWS && PLATFORM_64BITS && WITH_GAMELIFT_CLIENT
#include "Interfaces/IPluginManager.h"
#include "Misc/MessageDialog.h"
#endif

#define LOCTEXT_NAMESPACE "FCognitoIdentityModule"

void* FGameLiftClientSDKModule::CognitoIdentitySKDLibraryHandle = nullptr;

void FGameLiftClientSDKModule::StartupModule()
{
#if PLATFORM_WINDOWS && PLATFORM_64BITS && WITH_GAMELIFT_CLIENT

	const FString BaseDir = IPluginManager::Get().FindPlugin("GameLiftClientSDK")->GetBaseDir();

	const FString ThirdPartyDir = FPaths::Combine(*BaseDir, TEXT("ThirdParty"), TEXT("GameLiftClientSDK"), TEXT("Win64"));
	static const FString CognitoIdentityDLLName = "aws-cpp-sdk-cognito-identity";
	const bool bDependencyLoaded = LoadDependency(ThirdPartyDir, CognitoIdentityDLLName, CognitoIdentitySKDLibraryHandle);
	if (bDependencyLoaded == false)
	{
		FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT("LoadDependencyError", "Failed to load aws-cpp-sdk-cognito-identity. Plugin will not be functional"));
		FreeDependency(CognitoIdentitySKDLibraryHandle);
	}
#endif
}

void FGameLiftClientSDKModule::ShutdownModule()
{
	FreeDependency(CognitoIdentitySKDLibraryHandle);
}

void FGameLiftClientSDKModule::InitSDK()
{
#if WITH_GAMELIFT_CLIENT
	Aws::InitAPI(options);
#endif
}

bool FGameLiftClientSDKModule::LoadDependency(const FString& Dir, const FString& Name, void*& Handle)
{
	FString Lib = Name + TEXT(".") + FPlatformProcess::GetModuleExtension();
	FString Path = Dir.IsEmpty() ? *Lib : FPaths::Combine(*Dir, *Lib);

	Handle = FPlatformProcess::GetDllHandle(*Path);

	if (Handle == nullptr)
	{
		return false;
	}

	return true;
}

void FGameLiftClientSDKModule::FreeDependency(void*& Handle)
{
#if !PLATFORM_LINUX
	if (Handle != nullptr)
	{
		FPlatformProcess::FreeDllHandle(Handle);
		Handle = nullptr;
	}
#endif
}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FGameLiftClientSDKModule, GameLiftClientSDK);

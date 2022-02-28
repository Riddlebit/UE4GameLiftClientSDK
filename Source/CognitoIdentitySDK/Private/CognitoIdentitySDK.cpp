#include "CognitoIdentitySDK.h"
#include "Interfaces/IPluginManager.h"
#include "Misc/MessageDialog.h"
#include "Misc/Paths.h"
#include "Windows/WindowsPlatformProcess.h"

#define LOCTEXT_NAMESPACE "FCognitoIdentityModule"

void* FCognitoIdentitySDKModule::CognitoIdentitySKDLibraryHandle = nullptr;

void FCognitoIdentitySDKModule::StartupModule()
{
#if PLATFORM_WINDOWS && PLATFORM_64BITS

	const FString BaseDir = IPluginManager::Get().FindPlugin("GameLiftClientSDK")->GetBaseDir();

	const FString ThirdPartyDir = FPaths::Combine(*BaseDir, TEXT("ThirdParty"), TEXT("CognitoIdentitySDK"), TEXT("Win64"));
	static const FString CognitoIdentityDLLName = "aws-cpp-sdk-cognito-identity";
	const bool bDependencyLoaded = LoadDependency(ThirdPartyDir, CognitoIdentityDLLName, CognitoIdentitySKDLibraryHandle);
	if (bDependencyLoaded == false)
	{
		FFormatNamedArguments Arguments;
		Arguments.Add(TEXT("Name"), FText::FromString(CognitoIdentityDLLName));
		FMessageDialog::Open(EAppMsgType::Ok, FText::Format(LOCTEXT("LoadDependencyError", "Failed to load {Name}. Plugin will not be functional"), Arguments));
		FreeDependency(CognitoIdentitySKDLibraryHandle);
	}
#endif
}

void FCognitoIdentitySDKModule::ShutdownModule()
{
	FreeDependency(CognitoIdentitySKDLibraryHandle);
}

bool FCognitoIdentitySDKModule::LoadDependency(const FString& Dir, const FString& Name, void*& Handle)
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

void FCognitoIdentitySDKModule::FreeDependency(void*& Handle)
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

IMPLEMENT_MODULE(FCognitoIdentitySDKModule, CognitoIdentity);

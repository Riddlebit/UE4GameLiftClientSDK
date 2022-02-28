#include "AWSCoreSDK.h"
#include "Core.h"

#define LOCTEXT_NAMESPACE "FAWSCoreSDKModule"

void* FAWSCoreSDKModule::AWSCoreSKDLibraryHandle = nullptr;

void FAWSCoreSDKModule::StartupModule()
{
#if PLATFORM_WINDOWS && PLATFORM_64BITS && WITH_AWS_CORE
	const FString BaseDir = IPluginManager::Get().FindPlugin("GameLiftClientSDK")->GetBaseDir();

	const FString ThirdPartyDir = FPaths::Combine(*BaseDir, TEXT("ThirdParty"), TEXT("AWSCoreSDK"), TEXT("Win64"));

	static const FString CoreDLLName = "aws-cpp-sdk-core";
	const bool bDependencyLoaded = LoadDependency(ThirdPartyDir, CoreDLLName, AWSCoreSKDLibraryHandle);
	if (bDependencyLoaded == false)
	{
		FMessageDialog::Open(EAppMsgType::Ok, LOCTEXT(LOCTEXT_NAMESPACE, "Failed to load aws-cpp-sdk-core library. Plug-in will not be functional."));
		FreeDependency(AWSCoreSKDLibraryHandle);
	}
#endif
}

void FAWSCoreSDKModule::ShutdownModule()
{
	#if WITH_AWS_CORE
		Aws::ShutdownAPI(options);
	#endif
	FreeDependency(AWSCoreSKDLibraryHandle);
}

void FAWSCoreSDKModule::InitSDK()
{
	#if WITH_AWS_CORE
		Aws::InitAPI(options);
	#endif
}

bool FAWSCoreSDKModule::LoadDependency(const FString& Dir, const FString& Name, void*& Handle)
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

void FAWSCoreSDKModule::FreeDependency(void*& Handle)
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

IMPLEMENT_MODULE(FAWSCoreSDKModule, AWSCore);

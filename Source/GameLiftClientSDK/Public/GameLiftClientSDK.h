#pragma once

#include "Modules/ModuleManager.h"
#include "aws/core/Aws.h"

class FGameLiftClientSDKModule : public IModuleInterface
{
public:
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
	void InitSDK();

private:
	Aws::SDKOptions options;
	static void* CognitoIdentitySKDLibraryHandle;
	static bool LoadDependency(const FString& Dir, const FString& Name, void*& Handle);
	static void FreeDependency(void*& Handle);
};
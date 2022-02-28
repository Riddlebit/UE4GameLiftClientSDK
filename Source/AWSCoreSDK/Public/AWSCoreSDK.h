#pragma once

#include "Modules/ModuleManager.h"
#include "aws/core/Aws.h"

class FAWSCoreSDKModule : public IModuleInterface
{
public:
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
	virtual void InitSDK();

private:
	Aws::SDKOptions options;
	static void* AWSCoreSKDLibraryHandle;
	static bool LoadDependency(const FString& Dir, const FString& Name, void*& Handle);
	static void FreeDependency(void*& Handle);
};

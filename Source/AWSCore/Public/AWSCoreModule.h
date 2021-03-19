#pragma once

#include "Modules/ModuleManager.h"
#include "aws/core/Aws.h"

class FAWSCoreModule : public IModuleInterface
{
public:
	void StartupModule();
	void ShutdownModule();

private:
	Aws::SDKOptions options;
	static void* AWSCoreLibraryHandle;
	static bool LoadDependency(const FString& Dir, const FString& Name, void*& Handle);
	static void FreeDependency(void*& Handle);
};

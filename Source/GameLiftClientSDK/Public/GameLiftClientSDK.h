﻿#pragma once

#include "Modules/ModuleManager.h"

class FGameLiftClientSDKModule : public IModuleInterface
{
public:
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
private:
};
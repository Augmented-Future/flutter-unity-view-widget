#include "pch-c.h"
#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include "codegen/il2cpp-codegen-metadata.h"





// 0x00000001 System.Boolean UnityEngine.XR.XRSettings::get_enabled()
extern void XRSettings_get_enabled_m5ECD63DA0B306F7A3FBFD6B67A5B0702F8AEFB54 (void);
// 0x00000002 System.Void UnityEngine.XR.XRSettings::set_enabled(System.Boolean)
extern void XRSettings_set_enabled_mB93EAB3F282547FD575857B028DDD296C5B21D5F (void);
// 0x00000003 System.String UnityEngine.XR.XRSettings::get_loadedDeviceName()
extern void XRSettings_get_loadedDeviceName_m0EC0BC3BFBF1483DBC766D882A54677F2DBFC4BA (void);
// 0x00000004 System.Boolean UnityEngine.XR.XRDevice::get_isPresent()
extern void XRDevice_get_isPresent_m0287D6E7938755574C5F80C81529006B0D56D524 (void);
// 0x00000005 System.Void UnityEngine.XR.XRDevice::InvokeDeviceLoaded(System.String)
extern void XRDevice_InvokeDeviceLoaded_m07DEE6645B38728C7B8615DAAD6BE592C1DC59F9 (void);
static Il2CppMethodPointer s_methodPointers[5] = 
{
	XRSettings_get_enabled_m5ECD63DA0B306F7A3FBFD6B67A5B0702F8AEFB54,
	XRSettings_set_enabled_mB93EAB3F282547FD575857B028DDD296C5B21D5F,
	XRSettings_get_loadedDeviceName_m0EC0BC3BFBF1483DBC766D882A54677F2DBFC4BA,
	XRDevice_get_isPresent_m0287D6E7938755574C5F80C81529006B0D56D524,
	XRDevice_InvokeDeviceLoaded_m07DEE6645B38728C7B8615DAAD6BE592C1DC59F9,
};
static const int32_t s_InvokerIndices[5] = 
{
	7616,
	7520,
	7637,
	7616,
	7529,
};
IL2CPP_EXTERN_C const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule;
const Il2CppCodeGenModule g_UnityEngine_VRModule_CodeGenModule = 
{
	"UnityEngine.VRModule.dll",
	5,
	s_methodPointers,
	0,
	NULL,
	s_InvokerIndices,
	0,
	NULL,
	0,
	NULL,
	0,
	NULL,
	NULL,
	NULL, // module initializer,
	NULL,
	NULL,
	NULL,
};

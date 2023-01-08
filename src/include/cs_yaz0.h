#pragma once
#define CS_YAZ0

#include <oead/yaz0.h>

#ifdef __cplusplus
extern "C" {
#endif

#ifdef SHARED_DLL
#define CS_YAZ0 __declspec(dllexport)
#else
#define CS_YAZ0 __declspec(dllimport)
#endif

std::vector<u8> CS_YAZ0 Compress(tcb::span<const u8> data, u32 dataAlignment = 16, int level = 7);
void CS_YAZ0 Decompress(tcb::span<const u8> src, tcb::span<u8> dst);

#ifdef __cplusplus
}
#endif
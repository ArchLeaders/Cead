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

u8* CS_YAZ0 Compress(const u8* src, int src_len, int& dst_len, u32 data_alignment, int level);
void CS_YAZ0 Decompress(const u8* src, int src_len, u8* dst, int dst_len);

#ifdef __cplusplus
}
#endif
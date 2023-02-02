#pragma once
#define CS_YAZ0 __declspec(dllexport)

#include <oead/yaz0.h>

#ifdef __cplusplus
extern "C" {
#endif

void CS_YAZ0 Compress(const u8* src, u32 src_len, void** dst_handle, u8** dst, u32* dst_len, u32 data_alignment, int level);
void CS_YAZ0 Decompress(const u8* src, u32 src_len, u8* dst, u32 dst_len);

#ifdef __cplusplus
}
#endif
#pragma once
#define CS_BYML __declspec(dllexport)

#include <oead/byml.h>

#ifdef __cplusplus
extern "C" {
#endif

using namespace oead;

void CS_BYML FromBinary(const u8* src, u32 src_len, void** dst);
void CS_BYML FromText(const char* src, u32 src_len, void** dst);
void CS_BYML ToBinary(Byml* byml, void** dst_handle, u8** dst, u32* dst_len, bool big_endian, int version);
void CS_BYML ToText(Byml* byml, void** dst_handle, const char** dst);

#ifdef __cplusplus
}
#endif
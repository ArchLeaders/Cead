#pragma once
#define CS_BYML __declspec(dllexport)

#include <oead/byml.h>

#ifdef __cplusplus
extern "C" {
#endif

using namespace oead;

void CS_BYML FromBinary(const u8* src, u32 src_len, void** dst);
void CS_BYML FromText(const char* src, void** dst);
void CS_BYML ToBinary(Byml* byml, void** vector_handle, u8** dst, u32* dst_len, bool big_endian, int version);
void CS_BYML ToText(Byml* byml, void** string_handle, const char** dst);

Byml::Type CS_BYML GetType(Byml* byml);

void CS_BYML GetHash(Byml* byml, void** dst);
void CS_BYML GetArray(Byml* byml, void** vector_handle, Byml** dst, u32* dst_size);
void CS_BYML GetString(Byml* byml, void** string_handle, const char** dst);
void CS_BYML GetBinary(Byml* byml, void** vector_handle, u8** dst, u32* dst_size);
bool CS_BYML GetBool(Byml* byml);
s32 CS_BYML GetInt(Byml* byml);
u32 CS_BYML GetUInt(Byml* byml);
f32 CS_BYML GetFloat(Byml* byml);
s64 CS_BYML GetInt64(Byml* byml);
u64 CS_BYML GetUInt64(Byml* byml);
f64 CS_BYML GetDouble(Byml* byml);

#ifdef __cplusplus
}
#endif
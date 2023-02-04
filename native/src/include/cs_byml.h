#pragma once
#define CEAD __declspec(dllexport)

#include <oead/byml.h>

extern "C" {

using namespace oead;

CEAD Byml* FromBinary(const u8* src, u32 src_len);
CEAD Byml* FromText(const char* src);
CEAD void* ToBinary(Byml* byml, u8** dst, u32* dst_len, bool big_endian, int version);
CEAD const char* ToText(Byml* byml, std::string** handle);
CEAD Byml::Type GetType(Byml* byml);
CEAD Byml::Hash* GetHash(Byml* byml);
CEAD Byml::Array* GetArray(Byml* byml, Byml** dst, u32* dst_size);
CEAD const char* GetString(Byml* byml);
CEAD void* GetBinary(Byml* byml, u8** dst, u32* dst_size);
CEAD bool GetBool(Byml* byml);
CEAD s32 GetInt(Byml* byml);
CEAD u32 GetUInt(Byml* byml);
CEAD f32 GetFloat(Byml* byml);
CEAD s64 GetInt64(Byml* byml);
CEAD u64 GetUInt64(Byml* byml);
CEAD f64 GetDouble(Byml* byml);
}
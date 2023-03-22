#pragma once

#include <cead.h>
#include <oead/byml.h>

extern "C" {

using namespace oead;

CEAD Byml* BymlFromBinary(const u8* src, u32 src_len);
CEAD Byml* FromText(const char* src);
CEAD void* BymlToBinary(Byml* byml, u8** dst, u32* dst_len, bool big_endian, int version);
CEAD std::string* ToText(Byml* byml, const char** dst, int* dst_len);
CEAD Byml::Type GetType(Byml* byml);
CEAD Byml::Hash* GetHash(Byml* byml);
CEAD Byml::Array* GetArray(Byml* byml);
CEAD const char* GetString(Byml* byml);
CEAD void* GetBinary(Byml* byml, u8** dst, u32* dst_size);
CEAD bool GetBool(Byml* byml);
CEAD s32 GetInt(Byml* byml);
CEAD u32 GetUInt(Byml* byml);
CEAD f32 GetFloat(Byml* byml);
CEAD s64 GetInt64(Byml* byml);
CEAD u64 GetUInt64(Byml* byml);
CEAD f64 GetDouble(Byml* byml);

// Constructor wrapper functions
CEAD Byml* Hash(Byml::Hash* value);
CEAD Byml* Array(Byml::Array* value);
CEAD Byml* String(char* value);
CEAD Byml* Binary(u8* value, int value_len);
CEAD Byml* Int(S32 value);
CEAD Byml* UInt(U32 value);
CEAD Byml* Float(F32 value);
CEAD Byml* Int64(S64 value);
CEAD Byml* UInt64(U64 value);
CEAD Byml* Double(F64 value);

CEAD bool FreeByml(Byml* byml);
}
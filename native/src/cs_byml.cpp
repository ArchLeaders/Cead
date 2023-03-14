#include "include/cs_byml.h"

Byml* BymlFromBinary(const u8* src, u32 src_len) {
    auto* result = new auto{Byml::FromBinary({src, src_len})};
    return result;
}

Byml* FromText(const char* src) {
    auto* result = new auto{Byml::FromText(src)};
    return result;
}

void* BymlToBinary(Byml* byml, u8** dst, u32* dst_len, bool big_endian, int version) {
    auto* result = new auto{byml->ToBinary(big_endian, version)};
    *dst = result->data();
    *dst_len = result->size();
    return result;
}

std::string* ToText(Byml* byml, const char** dst, int* dst_len) {
    std::string* str = new auto{byml->ToText()};
    *dst = str->c_str();
    *dst_len = str->length();
    return str;
}

Byml::Type GetType(Byml* byml) {
    return byml->GetType();
}

Byml::Hash* GetHash(Byml* byml) {
    return &byml->GetHash();
}

Byml::Array* GetArray(Byml* byml) {
    return &byml->GetArray();
}

const char* GetString(Byml* byml) {
    return byml->GetString().c_str();
}

void* GetBinary(Byml* byml, u8** dst, u32* dst_size) {
    auto* result = &byml->GetBinary();
    *dst = result->data();
    *dst_size = result->size();
    return result;
}

bool GetBool(Byml* byml) {
    return byml->GetBool();
}

s32 GetInt(Byml* byml) {
    return byml->GetInt();
}

u32 GetUInt(Byml* byml) {
    return byml->GetUInt();
}

f32 GetFloat(Byml* byml) {
    return byml->GetFloat();
}

s64 GetInt64(Byml* byml) {
    return byml->GetInt64();
}

u64 GetUInt64(Byml* byml) {
    return byml->GetUInt64();
}

f64 GetDouble(Byml* byml) {
    return byml->GetDouble();
}

Byml* Hash(Byml::Hash* value) {
    return new auto{Byml(*value)};
}

Byml* Array(Byml::Array* value) {
    return new auto{Byml(*value)};
}

Byml* String(char* value) {
    return new auto{Byml(std::string(value))};
}

Byml* Binary(u8* value, int value_len) {
    return new auto{Byml(std::vector<u8>(value_len, *value))};
}

Byml* Int(S32 value) {
    return new auto{Byml(value)};
}

Byml* UInt(U32 value) {
    return new auto{Byml(value)};
}

Byml* Float(F32 value) {
    return new auto{Byml(value)};
}

Byml* Int64(S64 value) {
    return new auto{Byml(value)};
}

Byml* UInt64(U64 value) {
    return new auto{Byml(value)};
}

Byml* Double(F64 value) {
    return new auto{Byml(value)};
}
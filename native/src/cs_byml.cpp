#include "include/cs_byml.h"

Byml* FromBinary(const u8* src, u32 src_len) {
    auto* result = &Byml::FromBinary({src, src_len});
    return result;
}

Byml* FromText(const char* src) {
    auto* result = &Byml::FromText(src);
    return result;
}

void* ToBinary(Byml* byml, u8** dst, u32* dst_len, bool big_endian, int version) {
    auto* result = &byml->ToBinary(big_endian, version);
    *dst = result->data();
    *dst_len = result->size();
    return result;
}

const char* ToText(Byml* byml, std::string** handle) {
    return byml->ToText().c_str();
}

Byml::Type GetType(Byml* byml) {
    return byml->GetType();
}

Byml::Hash* GetHash(Byml* byml) {
    return &byml->GetHash();
}

Byml::Array* GetArray(Byml* byml, Byml** dst, u32* dst_size) {
    auto* result = &byml->GetArray();
    *dst = result->data();
    *dst_size = result->size();
    return result;
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
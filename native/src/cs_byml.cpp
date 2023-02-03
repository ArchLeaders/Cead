#include "include/cs_byml.h"

void FromBinary(const u8* src, u32 src_len, void** dst) {
    Byml byml = Byml::FromBinary({src, src_len});
    *dst = new auto{byml};
}

void FromText(const char* src, void** dst) {
    Byml byml = Byml::FromText(src);
    *dst = new auto{byml};
}

void ToBinary(Byml* byml, void** vector_handle, u8** dst, u32* dst_len, bool big_endian, int version) {
    auto* result = new auto{byml->ToBinary(big_endian, version)};
    *vector_handle = result;
    *dst = result->data();
    *dst_len = result->size();
}

void ToText(Byml* byml, void** string_handle, const char** dst) {
    auto str = new auto{byml->ToText()};
    *string_handle = str;
    *dst = str->c_str();
}

Byml::Type GetType(Byml* byml) {
    return byml->GetType();
}

void GetHash(Byml* byml, void** dst) {
    *dst = new auto{byml->GetHash()};
}

void GetArray(Byml* byml, void** vector_handle, Byml** dst, u32* dst_size) {
    auto* result = new auto{byml->GetArray()};
    *dst = result->data();
    *dst_size = result->size();
}

void GetString(Byml* byml, void** string_handle, const char** dst) {
    auto str = new auto{byml->ToText()};
    *string_handle = str;
    *dst = str->c_str();
}

void GetBinary(Byml* byml, void** vector_handle, u8** dst, u32* dst_size) {
    auto* result = new auto{byml->GetBinary()};
    *dst = result->data();
    *dst_size = result->size();
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
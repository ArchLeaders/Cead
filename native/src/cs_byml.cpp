#include "include/cs_byml.h"

void FromBinary(const u8* src, u32 src_len, void** dst) {
    Byml byml = Byml::FromBinary({src, src_len});
    *dst = new auto{byml};
}

void FromText(const char* src, void** dst) {
    Byml byml = Byml::FromText(src);
    *dst = new auto{byml};
}

void ToBinary(Byml* byml, void** dst_handle, u8** dst, u32* dst_len, bool big_endian, int version) {
    auto* result = new auto{byml->ToBinary(big_endian, version)};
    *dst_handle = result;
    *dst = result->data();
    *dst_len = result->size();
}

void ToText(Byml* byml, void** dst_handle, const char** dst) {
    auto str = new auto{byml->ToText()};
    *dst_handle = str;
    *dst = str->c_str();
}
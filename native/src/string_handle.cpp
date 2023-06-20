#include "include/string_handle.h"

bool FreeString(std::string* str) {
    delete str;
    return true;
}

void FillString(std::string* str, const char** dst, size_t* dst_len) {
    *dst = str->c_str();
    *dst_len = str->length();
}
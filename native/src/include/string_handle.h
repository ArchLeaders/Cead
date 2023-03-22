#pragma once

#include <cead.h>
#include <oead/types.h>

#include <string>

extern "C" {

CEAD bool FreeString(std::string* str);
CEAD void FillString(std::string* str, const char** dst, u32* dst_len);
}
#pragma once

#include <cead.h>
#include <oead/aamp.h>

extern "C" {

using namespace oead::aamp;

CEAD ParameterIO* AampFromBinary(u8* src, u32 src_len);
CEAD std::vector<u8>* AampToBinary(ParameterIO* src);
CEAD ParameterIO* AampFromText(const char* src);
CEAD std::string* AampToText(ParameterIO* src);
CEAD bool FreeAamp(ParameterIO* pio);
}
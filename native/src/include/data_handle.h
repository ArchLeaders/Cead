#pragma once

#include <cead.h>
#include <oead/types.h>

#include <vector>

extern "C" {

CEAD bool FreeVector(std::vector<u8>* vector);
CEAD void FillData(std::vector<u8>* vector, u8** dst, u32* dst_len);
}
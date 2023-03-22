#pragma once

#include <cead.h>
#include <oead/yaz0.h>

extern "C" {

using namespace oead;

CEAD void* Compress(const u8* src, u32 src_len, u32 data_alignment, int level);
CEAD void Decompress(const u8* src, u32 src_len, u8* dst, u32 dst_len);
}
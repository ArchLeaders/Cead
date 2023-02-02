#include "include/cs_yaz0.h"

using namespace oead;

void Compress(const u8* src, u32 src_len, void** dst_handle, u8** dst, u32* dst_len, u32 data_alignment, int level) {
    auto* result = new auto{yaz0::Compress({src, src_len}, data_alignment, level)};
    *dst_handle = result;
    *dst = result->data();
    *dst_len = result->size();
}

void Decompress(const u8* src, u32 src_len, u8* dst, u32 dst_len) {
    yaz0::Decompress({src, src_len}, {dst, dst_len});
}
#include "include/cs_yaz0.h"

void* Compress(const u8* src, u32 src_len, u32 data_alignment, int level) {
    return new auto(yaz0::Compress({src, src_len}, data_alignment, level));
}

void Decompress(const u8* src, u32 src_len, u8* dst, u32 dst_len) {
    yaz0::Decompress({src, src_len}, {dst, dst_len});
}
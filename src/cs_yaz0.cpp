#include "include/cs_yaz0.h"

using namespace oead;

u8* Compress(const u8* src, int src_len, int& dst_len, u32 data_alignment, int level) {
    auto data = tcb::span<const u8>(src, src_len);
    auto dst_data = yaz0::Compress(data, data_alignment, level);
    dst_len = dst_data.size();
    return dst_data.data();
}

void Decompress(const u8* src, int src_len, u8* dst, int dst_len) {
    auto src_span = tcb::span<const u8>(src, src_len);
    auto dst_span = tcb::span<u8>(dst, dst_len);
    yaz0::Decompress(src_span, dst_span);
}
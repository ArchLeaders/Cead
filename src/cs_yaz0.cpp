#include "include/cs_yaz0.h"

using namespace oead;

void Compress(const u8* src, u32 src_len, void** dst_handle, u8** dst, u32* dst_len, u32 data_alignment, int level) {
    auto* result = new auto{yaz0::Compress({src, src_len}, data_alignment, level)};
    *dst_handle = result;
    *dst = result->data();
    *dst_len = result->size();
}

void Decompress(const u8* src, int src_len, u8* dst, int dst_len) {
    auto src_span = tcb::span<const u8>(src, src_len);
    auto dst_span = tcb::span<u8>(dst, dst_len);
    yaz0::Decompress(src_span, dst_span);
}

bool FreeResource(void* vector_ptr) {
    delete static_cast<std::vector<u8>*>(vector_ptr);
    return true;
}
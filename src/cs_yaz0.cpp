#include "include/cs_yaz0.h"

using namespace oead;

std::vector<u8> Compress(tcb::span<const u8> data, u32 dataAlignment, int level) {
    return yaz0::Compress(data, dataAlignment, level);
}

void Decompress(tcb::span<const u8> src, tcb::span<u8> dst) {
    yaz0::Decompress(src, dst);
}
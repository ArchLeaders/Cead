#include "include/cs_aamp.h"

ParameterIO* AampFromBinary(u8* src, u32 src_len) {
    return new auto(ParameterIO::FromBinary({src, src_len}));
}

std::vector<u8>* AampToBinary(ParameterIO* aamp) {
    return new auto(aamp->ToBinary());
}

ParameterIO* AampFromText(const char* src) {
    return new auto(ParameterIO::FromText(src));
}

std::string* AampToText(ParameterIO* aamp) {
    return new auto(aamp->ToText());
}

bool FreeAamp(ParameterIO* pio) {
    delete pio;
    return true;
}
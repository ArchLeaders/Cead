#include "include/cs_sarc_file.h"

const char* GetSarcFileName(Sarc::File* sarc_file) {
    return sarc_file->name.data();
}

void SetSarcFileName(Sarc::File* sarc_file, char* name) {
    sarc_file->name = name;
}

void GetSarcFileData(Sarc::File* sarc_file, const u8** dst, u32* dst_len) {
    *dst = sarc_file->data.data();
    *dst_len = sarc_file->data.size();
}

void SetSarcFileData(Sarc::File* sarc_file, u8* src, u32 src_len) {
    sarc_file->data = std::vector<u8>(src, src + src_len);
}
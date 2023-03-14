#include "include/cs_sarc.h"

Sarc* SarcFromBinary(u8* src, u32 src_len) {
    return new auto{Sarc({src, src_len})};
}

void SarcToBinary(SarcWriter* writer, void** handle, u8** dst, u32* dst_len) {
    auto result = new auto{writer->Write()};
    *handle = result;
    *dst = result->second.data();
    *dst_len = result->second.size();
}

u32 GetNumFiles(Sarc* sarc) {
    return sarc->GetNumFiles();
}

u32 GetFileMapCount(SarcWriter* writer) {
    return writer->m_files.size();
}

util::Endianness GetEndianness(Sarc* sarc) {
    return sarc->GetEndianness();
}

void SetEndianness(SarcWriter* writer, util::Endianness endianess) {
    writer->SetEndianness(endianess);
}

bool GetFile(Sarc* sarc, char* name, const u8** dst, u32* dst_len) {
    auto file = sarc->GetFile(name);
    if (file.has_value()) {
        *dst = file.value().data.data();
        *dst_len = file.value().data.size();
        return true;
    }

    return false;
}

SarcWriter* NewSarcWriter(util::Endianness endian, SarcWriter::Mode mode) {
    return new auto{SarcWriter(endian, mode)};
}

SarcWriter* GetSarcWriter(Sarc* sarc) {
    return new auto{SarcWriter::FromSarc(*sarc)};
}

void SetWriterMode(SarcWriter* writer, SarcWriter::Mode mode) {
    writer->SetMode(mode);
}

bool SarcWriterGet(SarcWriter* writer, char* name, u8** dst, u32* dst_len) {
    auto vec = writer->m_files.at(name);
    *dst = vec.data();
    *dst_len = vec.size();
    return true;
}

void AddSarcFile(SarcWriter* writer, char* name, u8* src, u32 src_len) {
    writer->m_files.insert_or_assign(name, std::vector<u8>(src, src + src_len));
}

void RemoveSarcFile(SarcWriter* writer, char* name) {
    writer->m_files.erase(name);
}

void ClearSarcFiles(SarcWriter* writer) {
    writer->m_files.clear();
}
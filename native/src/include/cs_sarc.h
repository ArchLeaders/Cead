#pragma once
#define CEAD __declspec(dllexport)

#include <oead/sarc.h>

extern "C" {

using namespace oead;

CEAD Sarc* SarcFromBinary(u8* src, u32 src_len);
CEAD void SarcToBinary(SarcWriter* writer, void** handle, u8** dst, u32* dst_len);
CEAD u32 GetNumFiles(Sarc* sarc);
CEAD u32 GetFileMapCount(SarcWriter* writer);
CEAD util::Endianness GetEndianness(Sarc* sarc);
CEAD void SetEndianness(SarcWriter* writer, util::Endianness endianess);
CEAD bool GetFile(Sarc* sarc, char* name, const u8** dst, u32* dst_len);
CEAD SarcWriter* NewSarcWriter(util::Endianness endian, SarcWriter::Mode mode);
CEAD SarcWriter* GetSarcWriter(Sarc* sarc);
CEAD void SetWriterMode(SarcWriter* writer, SarcWriter::Mode mode);
CEAD bool SarcWriterGet(SarcWriter* writer, char* name, u8** dst, u32* dst_len);
CEAD void AddSarcFile(SarcWriter* writer, char* name, u8* src, u32 src_len);
CEAD void RemoveSarcFile(SarcWriter* writer, char* name);
CEAD void ClearSarcFiles(SarcWriter* writer);
}
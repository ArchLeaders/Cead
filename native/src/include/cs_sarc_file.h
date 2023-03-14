#pragma once
#define CEAD __declspec(dllexport)

#include <oead/sarc.h>

extern "C" {

using namespace oead;

CEAD const char* GetSarcFileName(Sarc::File* sarc_file);
CEAD void SetSarcFileName(Sarc::File* sarc_file, char* name);
CEAD void GetSarcFileData(Sarc::File* sarc_file, const u8** dst, u32* dst_len);
CEAD void SetSarcFileData(Sarc::File* sarc_file, u8* src, u32 src_len);
}
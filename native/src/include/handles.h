#pragma once
#define CEAD __declspec(dllexport)

#include <oead/byml.h>
#include <oead/types.h>

#include <string>
#include <vector>

extern "C" {

CEAD bool FreePtr(void* ptr);
}
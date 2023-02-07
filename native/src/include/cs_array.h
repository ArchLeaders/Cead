#pragma once
#define CEAD __declspec(dllexport)

#include <oead/byml.h>

#include <algorithm>

extern "C" {

using namespace oead;

CEAD Byml* ArrayGet(Byml::Array* array, int index);
CEAD void ArraySet(Byml::Array* array, int index, Byml* value);
CEAD void ArrayAdd(Byml::Array* array, Byml* value);
CEAD void ArrayRemove(Byml::Array* array, int index);
CEAD void ArrayClear(Byml::Array* array);
CEAD int ArrayLength(Byml::Array* array);
CEAD Byml* ArrayCurrent(Byml::Array* array, int index);
}
#pragma once
#define CEAD __declspec(dllexport)

#include <oead/byml.h>

extern "C" {

using namespace oead;

CEAD Byml* HashGet(Byml::Hash* hash, const char* key);
CEAD void HashSet(Byml::Hash* hash, const char* key, Byml* value);
CEAD void HashAdd(Byml::Hash* hash, const char* key, Byml* value);
CEAD void HashRemove(Byml::Hash* hash, const char* key);
CEAD bool HashContains(Byml::Hash* hash, const char* key);
CEAD void HashClear(Byml::Hash* hash);
CEAD int HashLength(Byml::Hash* hash);

CEAD void HashExpandIterator(absl::btree_map<std::string, Byml>::iterator* iterator, const char** key, Byml** value);
CEAD bool HashAdvance(Byml::Hash* hash, absl::btree_map<std::string, Byml>::iterator* iterator, absl::btree_map<std::string, Byml>::iterator** next);
CEAD auto* HashBegin(Byml::Hash* hash);
}
#include "include/cs_hash.h"

Byml* HashGet(Byml::Hash* hash, const char* key) {
    return &(*hash)[key];
}

void HashSet(Byml::Hash* hash, const char* key, Byml* value) {
    hash->insert_or_assign(key, *value);
}

void HashAdd(Byml::Hash* hash, const char* key, Byml* value) {
    hash->insert({key, *value});
}

void HashRemove(Byml::Hash* hash, const char* key) {
    hash->erase(key);
}

bool HashContains(Byml::Hash* hash, const char* key) {
    return hash->contains(key);
}

void HashClear(Byml::Hash* hash) {
    hash->clear();
}

int HashLength(Byml::Hash* hash) {
    return hash->size();
}

void HashExpandIterator(absl::btree_map<std::string, Byml>::iterator* iterator, const char** key, Byml** value) {
    auto it = *iterator;
    *key = it->first.c_str();
    *value = &it->second;
}

bool HashAdvance(Byml::Hash* hash, absl::btree_map<std::string, Byml>::iterator* iterator, absl::btree_map<std::string, Byml>::iterator** next) {
    std::advance(iterator, 1);
    if ((*iterator) != hash->end()) {
        return true;
    }

    return false;
}

auto* HashBegin(Byml::Hash* hash) {
    return &hash->begin();
}
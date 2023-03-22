#include "include/cs_hash.h"

Byml* HashGet(Byml::Hash* hash, const char* key) {
    return &hash->at(key);
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

bool HashContainsKey(Byml::Hash* hash, const char* key) {
    return hash->contains(key);
}

void HashClear(Byml::Hash* hash) {
    hash->clear();
}

int HashLength(Byml::Hash* hash) {
    return hash->size();
}

void HashCurrent(Byml::Hash::iterator* iterator, const char** key, Byml** value) {
    auto it = *iterator;
    *key = it->first.c_str();
    *value = &it->second;
}

bool HashAdvance(Byml::Hash* hash, Byml::Hash::iterator* iterator, Byml::Hash::iterator** next) {
    if (iterator == NULL) {
        *next = new auto{hash->begin()};
        return true;
    }

    if (++(*iterator) != hash->end()) {
        *next = iterator;
        return true;
    }

    return false;
}

Byml::Hash* BuildEmptyHash() {
    return new Byml::Hash{};
}

bool FreeHash(Byml::Hash* hash) {
    delete hash;
    return true;
}
#include "include/cs_array.h"

Byml* ArrayGet(Byml::Array* array, int index) {
    return &array->at(index);
}

void ArraySet(Byml::Array* array, int index, Byml* value) {
    array->assign(index, *value);
}

void ArrayAdd(Byml::Array* array, Byml* value) {
    array->push_back(*value);
}

void ArrayRemove(Byml::Array* array, int index) {
    array->erase(array->begin() + index);
}

bool ArrayContains(Byml::Array* array, Byml* value) {
    // return std::find(array->begin(), array->end(), value) != array->end();
    return false;
}

void ArrayClear(Byml::Array* array) {
    array->clear();
}

int ArrayLength(Byml::Array* array) {
    return array->size();
}

Byml* ArrayCurrent(Byml::Array* array, int index) {
    return &array->at(index);
}
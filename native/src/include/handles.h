#pragma once
#define HANDLES __declspec(dllexport)

#include <string>
#include <vector>
#include <oead/types.h>

#ifdef __cplusplus
extern "C" {
#endif

bool HANDLES FreeVector(void* vector_ptr) {
    delete static_cast<std::vector<u8>*>(vector_ptr);
    return true;
}

bool HANDLES FreeString(void* string_ptr) {
    delete static_cast<std::string*>(string_ptr);
    return true;
}

#ifdef __cplusplus
}
#endif
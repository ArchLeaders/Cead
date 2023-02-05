#include "include/handles.h"

bool FreePtr(void* ptr) {
    free(ptr);
    std::cout << "[c++] Freed: " << ptr << std::endl;
    return true;
}
#include "include/handles.h"

bool FreePtr(void* ptr) {
    free(ptr);
    return true;
}
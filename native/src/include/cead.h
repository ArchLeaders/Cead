#pragma once
#if _WIN32
#define CEAD __declspec(dllexport)
#else
#define CEAD __attribute__((cdecl))
#endif
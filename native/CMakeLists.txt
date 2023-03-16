cmake_minimum_required(VERSION 3.10)
project(Cead VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)
set(CMAKE_SHARED_LIBRARY_PREFIX "")

if(WIN32)
    set(CMAKE_CXX_STANDARD_LIBRARIES "-static -static-libgcc -static-libstdc++")
endif()

include(CTest)
enable_testing()

add_library(Cead SHARED
    ./src/include/cead.h

    ./src/include/handles.h
    ./src/handles.cpp

    ./src/include/cs_yaz0.h
    ./src/cs_yaz0.cpp

    ./src/include/cs_array.h
    ./src/cs_array.cpp
    ./src/include/cs_byml.h
    ./src/cs_byml.cpp
    ./src/include/cs_hash.h
    ./src/cs_hash.cpp

    ./src/include/cs_sarc.h
    ./src/cs_sarc.cpp

    ./src/include/cs_aamp.h
    ./src/cs_aamp.cpp
)

add_subdirectory(${PROJECT_SOURCE_DIR}/lib/oead)
target_link_libraries(Cead PRIVATE oead)
target_include_directories(Cead PRIVATE ./src/include)

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)

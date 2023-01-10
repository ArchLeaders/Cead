cmake_minimum_required(VERSION 3.10)
project(Cead VERSION 0.1.0)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)
set(CMAKE_SHARED_LIBRARY_PREFIX "")

include(CTest)
enable_testing()

add_definitions(-DSHARED_DLL)
add_library(Cead SHARED
    ./src/include/cs_yaz0.h
    ./src/cs_yaz0.cpp
)

add_subdirectory(${PROJECT_SOURCE_DIR}/lib/oead)
target_link_libraries(Cead PRIVATE oead)

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)
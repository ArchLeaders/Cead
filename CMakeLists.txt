cmake_minimum_required(VERSION 3.10)
project(Cead VERSION 0.1.0)

include(CTest)
enable_testing()

add_library(Cead SHARED
    ./src/Cead.cpp
)

add_subdirectory(${PROJECT_SOURCE_DIR}/lib/oead)
target_include_directories(Cead PUBLIC ${PROJECT_SOURCE_DIR}/lib/oead/src/include)
target_link_directories(Cead PRIVATE ${PROJECT_SOURCE_DIR}/lib/oead/src/include)

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)

#!/bin/bash
set -e 

docker run --rm \
    -e RUN_LOCAL=true \
    -e USE_FIND_ALGORITHM=true \
    -e VALIDATE_ALL_CODEBASE=false \
    -e VALIDATE_CSHARP=true \
    -e DEFAULT_BRANCH=main \
    -e LINTER_RULES_PATH=/ \
    -e LOG_LEVEL=WARN \
    -e FILTER_REGEX_EXCLUDE=".*(obj|bin|PackageCache)/*" \
    -e IGNORE_GENERATED_FILES=true \
    -v "$PWD":/tmp/lint \
    -w /tmp/lint \
    github/super-linter:v5

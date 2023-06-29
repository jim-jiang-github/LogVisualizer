#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
VERSION=$1
DOT_NET="net7.0"
SLN_PATH="$SCRIPT_DIR/../../src/LogVisualizer.sln"
PROJECT_DIRECTORY="$SCRIPT_DIR/../../src/LogVisualizer/LogVisualizer.csproj"
APP_NAME="$SCRIPT_DIR/../../src/LogVisualizer/bin/Release/$DOT_NET/win-x64/LogVisualizer.app"
PUBLISH_OUTPUT_DIRECTORY="$SCRIPT_DIR/../../src/LogVisualizer/bin/Release/$DOT_NET/win-x64/publish/."

echo "[INFO] restore."
dotnet restore $SLN_PATH
dotnet restore $PROJECT_DIRECTORY -r win-x64
echo "[INFO] publish $PROJECT_DIRECTORY ."
dotnet publish "$PROJECT_DIRECTORY" -r win-x64 --configuration Release -p:PublishSingleFile=true -p:PublishReadyToRun=true --self-contained -property:Version=$VERSION
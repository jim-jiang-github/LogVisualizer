#!/bin/bash

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
DOT_NET="net6.0"
SLN_PATH="$SCRIPT_DIR/../../src/LogVisualizer.sln"
PROJECT_DIRECTORY="$SCRIPT_DIR/../../src/LogVisualizer/LogVisualizer.csproj"
APP_NAME="$SCRIPT_DIR/../../src/LogVisualizer/bin/Release/$DOT_NET/osx-x64/LogVisualizer.app"
PUBLISH_OUTPUT_DIRECTORY="$SCRIPT_DIR/../../src/LogVisualizer/bin/Release/$DOT_NET/osx-x64/publish/."
INFO_PLIST="Info.plist"
ICON_FILE="logo.icns"
ENTITLEMENTS="$SCRIPT_DIR/LogVisualizer.entitlements"
SIGNING_IDENTITY="Apple Development: Jiang Jim jiang (4895XFR9H7)"

echo "[INFO] restore."
dotnet restore $SLN_PATH
dotnet restore $PROJECT_DIRECTORY -r osx-x64
echo "[INFO] publish $PROJECT_DIRECTORY ."
dotnet publish "$PROJECT_DIRECTORY" -r osx-x64 --configuration Release --self-contained -p:UseAppHost=true

if [ -d "$APP_NAME" ]
then
    echo "[INFO] remove $APP_NAME ."
    rm -rf "$APP_NAME"
fi

echo "[INFO] start create app. $APP_NAME ."
mkdir "$APP_NAME"
mkdir "$APP_NAME/Contents"
mkdir "$APP_NAME/Contents/MacOS"
mkdir "$APP_NAME/Contents/Resources"

cp "$SCRIPT_DIR/$INFO_PLIST" "$APP_NAME/Contents/$INFO_PLIST"
cp "$SCRIPT_DIR/$ICON_FILE" "$APP_NAME/Contents/Resources/$ICON_FILE"
cp -a "$PUBLISH_OUTPUT_DIRECTORY" "$APP_NAME/Contents/MacOS"
echo "[INFO] end create app. $APP_NAME ."

echo "[INFO] Signing app file"
codesign --force --timestamp --options=runtime --entitlements "$ENTITLEMENTS" --sign "$SIGNING_IDENTITY" --deep "$APP_NAME"

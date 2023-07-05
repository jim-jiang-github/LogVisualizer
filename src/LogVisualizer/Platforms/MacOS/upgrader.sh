#!/bin/bash

originalFolder="$1"
targetFolder="$2"
executablePath="$3"
needRestart="$4"
cpResult=1

while [ $cpResult -eq 1 ]; do
  sleep 1
  rsync -a --delete "$originalFolder/LogVisualizer.app" "$targetFolder"
  cpResult=$?
done

echo "remove $originalFolder"
rm -rf "$originalFolder"

if [ "$needRestart" = "True" ]; then
  echo "ready for start"
  open "$executablePath"
  sleep 1
else
  echo "No restart needed"
fi

echo "ready for exit"

rm "$0"
exit

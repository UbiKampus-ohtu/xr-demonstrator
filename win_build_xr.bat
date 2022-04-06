set currentpath=%cd%
tar -xf .\XR_enabled.zip -C .\Assets\
Unity.exe -quit -batchmode -logFile stdout.log -projectPath "%currentpath%" -buildWindows64Player "Build\Windows\xr-demonstrator.exe"
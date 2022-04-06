set currentpath=%cd%
tar -xf .\XR_disabled.zip -C .\Assets\
Unity.exe -quit -batchmode -logFile stdout.log -projectPath "%currentpath%" -buildWindows64Player "Build\Windows\Flat\xr-demonstrator.exe"
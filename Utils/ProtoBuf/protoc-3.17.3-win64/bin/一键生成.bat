@echo off
for %%i in (*.proto) do (
 	protoc --csharp_out=./ %%i
	echo From %%i To %%~ni.cs Successfuly!
)
pause
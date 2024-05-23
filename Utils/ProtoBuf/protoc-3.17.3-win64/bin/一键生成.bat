@echo off
for %%i in (*.proto) do (
 	_protoc --csharp_out=output %%i
	echo From %%i To %%~ni.cs Successfuly!
)
pause
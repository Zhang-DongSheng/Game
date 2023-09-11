SET UNITY_PATH="C:\Program Files\Unity\Hub\Editor\2022.2.17f1c1\Editor\Unity.exe"
SET PROJECT_PATH="D:\Project\Game"

%UNITY_PATH% -nographics -batchMode -projectPath %PROJECT_PATH% -executeMethod MonoHook.Test.BuildPipeline_StripDll_HookTest.BuildPlayer -quit -logFile %PROJECT_PATH%\logs\build_player.log
pause
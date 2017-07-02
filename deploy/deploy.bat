cls
@echo off

:: 设置参数
set UNITY_PATH='D:\Program Files\Unity\Editor\Unity.exe'
set UNITY_PROJECT_NAME=unityplayer
set UNITY_PROJECT_PATH=%cd%\..
set EXPORT_PROJECT_PATH=%cd%\..\platform\android
set EXTRA_PROJECT_DATA=%cd%\data
set UNITY_METHOD_NAME=DeployTool.Build
set UNITY_LOG_PATH=%cd%\unity_deploy_log.txt
set ANDROID_PROJECT_PATH=%cd%\..\proj.android-studio

cmd /c py3 deploy.py --android-deploy -a

REM if not %errorlevel%==0 ( goto fail)
REM if not %errorlevel%==0 ( goto fail ) else ( goto success )

REM :success
REM echo 打包成功
REM goto end

REM :fail
REM echo 打包失败
REM goto end

REM :end
pause

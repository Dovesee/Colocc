::-------------------------------------------------
:: <sunmary>
:: 根据指定工作目录信息和模板生成目标文件
:: </sunmary>
:: <param name="WorkDir">工作目录路径</param>
:: <returns>执行结果</returns>
::=================================================

::-------------------------------------------------
:: * Initialize *
@ECHO OFF
::SetLocal EnableExtensions
setlocal enabledelayedexpansion

Rem Initialize Script arguments
SET WorkDir=%1

Rem Initialize Constants
SET AssemblyInfo=ASSEMBLY_INFO.tmp

Rem GoTo Main Entry
GOTO Main
::=================================================

::-------------------------------------------------
:: * Main Entry *
:Main
Rem Check arguments
IF %WorkDir%=="" GOTO ARGUNENT_ERROR
PushD %WorkDir%

Rem Search TSVN Path

SET GIT_PATH="E:\Git\bin\sh.exe" --login -i
call %GIT_PATH% %WorkDir%git_ver

for /f "delims=" %%i in (%WorkDir%\git_version.tmp) do (set VERSION=%%i)&(goto :next)
:next
DEL /Q "%WorkDir%\git_version.tmp">NUL

COPY /y "%WorkDir%AssemblyInfo.cs" "%WorkDir%AssemblyInfo.cs.bak">NUL
SET FILESTR="%WorkDir%AssemblyInfo.cs"
FindStr /v "AssemblyVersion AssemblyFileVersion" %FILESTR%>%AssemblyInfo%

For /f "delims=" %%k In (%WorkDir%AssemblyInfo.tpl) do (
  set str=%%k
  set str=!str:GITVERSION=%VERSION%!
  echo !str! >> "%AssemblyInfo%"
)

COPY /y "%AssemblyInfo%" "%WorkDir%AssemblyInfo.cs"

GOTO SUCCESS
::=================================================

::-------------------------------------------------
:: * Error Handlers *
:ARGUNENT_ERROR
ECHO 传入的参数无效。
GOTO FAIL

:UNKNOE_ERROR
ECHO 未知错误。
GOTO FAIL
::=================================================

::-------------------------------------------------
:: * Program Exit *
:FAIL
DEL /Q "%AssemblyInfo%">NUL
DEL /Q "%WorkDir%AssemblyInfo.cs.bak" 2>NUL
ECHO "error"
popd
EXIT 1

:SUCCESS
DEL /Q "%AssemblyInfo%">NUL
DEL /Q "%WorkDir%AssemblyInfo.cs.bak" 2>NUL
ECHO "success"
popd
EXIT 0
::=================================================
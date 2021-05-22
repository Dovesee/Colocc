:: <param name="WorkDir">脚本工作目录路径</param>
:: <returns>执行结果</returns>

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

::获得工作目录的commit次数和最后一次commit 默认使用安装到c盘的git
SET GIT_PATH="C:\Program Files\Git\bin\sh.exe" --login -i
call %GIT_PATH% %WorkDir%git_ver

for /f "tokens=1" %%i in (%WorkDir%\git_version.tmp) do (set VERSION=%%i)
for /f "tokens=2" %%i in (%WorkDir%\git_version.tmp) do (set GITVERSIONCOMMIT=%%i)&(goto :next)

:next
DEL /Q "%WorkDir%\git_version.tmp">NUL

COPY /y "%WorkDir%AssemblyInfo.cs" "%WorkDir%AssemblyInfo.cs.bak">NUL

set BUILDTIME=%date:~0,10% %time:~0,5%

For /f "delims=" %%k In (%WorkDir%AssemblyInfo.tpl) do (
  set str=%%k
  set str=!str:G#COMMIT=%GITVERSIONCOMMIT%!
  set str=!str:G#V#C=%VERSION%!
  set str=!str:B#DATE=%BUILDTIME%!
  echo !str! >> "%AssemblyInfo%"
)

DEL /Q "%WorkDir%AssemblyInfo.cs">NUL

Ren ASSEMBLY_INFO.tmp AssemblyInfo.cs

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
DEL /Q "%WorkDir%AssemblyInfo.cs.bak" 2>NUL
ECHO "success"
popd
EXIT 0
::=================================================
echo off

if "%2"=="1.0" goto FW10
if "%2"=="1.1" goto FW11
if "%2"=="2.0" goto FW20
if "%2"=="" goto ERROR

:FW10
set FW=%SYSTEMROOT%\Microsoft.NET\Framework\v1.0.3705
goto START
:FW11
set FW=%SYSTEMROOT%\Microsoft.NET\Framework\v1.1.4322
goto START
:FW20
set FW=%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727
goto START


:START

set w32=Win32Resources\GenesisGlyphs.res
set outd=bin\Debug\Comzept.Genesis.NetRix.Resources.dll
set outr=bin\Release\Comzept.Genesis.NetRix.Resources.dll

rem Build Debug version of full version
rem ==============================================================================================================
if "%1"=="DBG" %FW%\csc.exe /t:library /debug+ /w:1 /optimize+ /win32res:"%w32%" /out:"%outd%" /noconfig /recurse:*.cs 

rem Build Release version of full version
rem ==============================================================================================================
if "%1"=="REL" %FW%\csc.exe /t:library /debug- /w:1 /optimize+ /win32res:"%w32%" /out:"%outr%" /noconfig /recurse:*.cs 

GOTO FINAL

:ERROR
echo "Error, no version provided"


:FINAL
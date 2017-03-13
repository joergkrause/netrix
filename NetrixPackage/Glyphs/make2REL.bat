echo off

set FW=%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727

set w32=Release\NetrixGlyphs.res
set outr=Release\GuruComponents.Netrix.Resources.dll

rem Build Release version of full version
rem ==============================================================================================================
%FW%\csc.exe /t:library /debug- /w:1 /optimize+ /win32res:"%w32%" /out:"%outr%" /noconfig /recurse:*.cs 

echo "Done"

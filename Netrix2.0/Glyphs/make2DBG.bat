echo off

set FW=%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727

set w32=Debug\NetrixGlyphs.res
set outd=Debug\GuruComponents.Netrix.Resources.dll

rem Build Debug version of full version
rem ==============================================================================================================
%FW%\csc.exe /t:library /debug+ /w:1 /optimize+ /win32res:"%w32%" /out:"%outd%" /noconfig /recurse:*.cs 

echo "Done"


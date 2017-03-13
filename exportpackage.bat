echo off
rem Create License Free Package for Private Customers
set svn="C:\Program Files\SlikSvn\bin\svn.exe"
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage /S /Q
echo Export
%svn% export -r HEAD -q --force D:\Apps\StudioApps\Netrix2.0-Joerg\Netrix2.0 D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage

echo Done...

echo off
echo CleanUp
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\Documentation /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\LicensingCore /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\Key /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\ToolBoxInstaller /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\PlugIns\VmlDesigner /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\PlugIns\XmlDesigner /S /Q
rd D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\PlugIns\TidySharp /S /Q

echo Copy DLLs
set rel=D:\Apps\StudioApps\Netrix2.0-Joerg\Netrix2.0\NetRixMain\bin\Release
set plg=D:\Apps\StudioApps\Netrix2.0-Joerg\Netrix2.0\PlugIns
set des=D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\DLLs\*.*
md D:\Apps\StudioApps\Netrix2.0-Joerg\NetrixPackage\DLLs\

copy %rel%\*.dll %des%
copy %plg%\AspDotNetDesigner\bin\Release\*.dll %des%
copy %plg%\HelpLine\bin\Release\*.dll %des%
copy %plg%\HtmlFormatter\bin\Release\*.dll %des%
copy %plg%\SpellChecker\bin\Release\*.dll %des%
copy %plg%\TableDesigner\bin\Release\*.dll %des%
copy %rel%\*.xml %des%
copy %plg%\AspDotNetDesigner\bin\Release\*.xml %des%
copy %plg%\HelpLine\bin\Release\*.xml %des%
copy %plg%\HtmlFormatter\bin\Release\*.xml %des%
copy %plg%\SpellChecker\bin\Release\*.xml %des%
copy %plg%\TableDesigner\bin\Release\*.xml %des%

echo Done...

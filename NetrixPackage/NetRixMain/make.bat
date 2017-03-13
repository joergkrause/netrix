echo off

rem ##############################################################################################################
rem Makefile for Net.Rix component. It compiles the debug and release version in one step
rem The purpose of this compilation method is to have access to the win32res compiler switch which
rem is needed to embedd Win32 ressources, used by the glyph module.
rem
rem Usage: %1 = "DBG" to compile debug, "REL" for release
rem        %2 = Framework version we compile for (recommended: 1.0)
rem        %3 = "NDOC" to create the XML documentation file, only recognized if %1 == REL
rem             This will call the csc.exe twice, first build and reflect to the doc and then the release build.
rem             This is necessary to exclude public classes from documentation. the DBG build will not create the doc.
rem
rem (C) 2003 by Comzept Systemhaus GmbH, Jörg Krause, <joerg@krause.net>; All rights reserved
rem
rem Created October 13th, 2003, Last modified Feb 17th, 2004
rem
rem 
rem 
rem ##############################################################################################################

rem Set to specific FW version

if "%2"=="1.0" goto FW10
if "%2"=="1.1" goto FW11
if "%2"=="" goto ERROR

:FW10
set FW=C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705
goto START
:FW11
set FW=C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322
goto START

:START

rem Compile RESX resources built with VS.NET into .resources files

resgen NetRix\HtmlEditor.resx
resgen NetRix\HtmlEditor.resx
resgen Licensing\Choice.resx

rem ################# Make File ################# 

set outd=bin\Debug\Comzept.Genesis.NetRix.Editor.dll
set outdlight=..\NetRixLight\bin\Debug\Comzept.Genesis.NetRix.Editor.dll
set outr=bin\Release\Comzept.Genesis.NetRix.Editor.dll
set outrlight=..\NetRixLight\bin\Release\Comzept.Genesis.NetRix.Editor.dll
set w32=Win32Resources\GenesisGlyphs.res
set r1=System.dll
set r2=System.Web.dll
set r3=System.Data.dll
set r4=System.Drawing.dll
set r5=System.Windows.Forms.dll
set r6=System.Xml.dll
set r7=System.Design.dll

rem This resource is not linked if light version is created (No UI in light version!) 

set rrdbg=..\NetRixUI\bin\Debug\Comzept.Genesis.NetRix.UI.dll
set rrrel=..\NetRixUI\bin\Release\Comzept.Genesis.NetRix.UI.dll

rem ################# Embedded Form Resources ################# 

set frK=/res:NetRix\HtmlEditor.resources,Comzept.Genesis.NetRix.HtmlEditor.resources
set frL=/res:NetRix\HtmlEditor.resources,Comzept.Genesis.NetRix.HtmlEditor.resources
set frM=/res:Licensing\Choice.resources,Comzept.Genesis.NetRix.Licensing.Choice.resources

rem Image Resources

set ir1=Resources\Cursors\DiagonalArrow.cur,Comzept.Genesis.NetRix.Resources.Cursors.DiagonalArrow.cur
set ir2=Resources\Cursors\DownArrow.cur,Comzept.Genesis.NetRix.Resources.Cursors.DownArrow.cur
set ir3=Resources\Cursors\InputText.cur,Comzept.Genesis.NetRix.Resources.Cursors.InputText.cur
set ir4=Resources\Cursors\RightArrow.cur,Comzept.Genesis.NetRix.Resources.Cursors.RightArrow.cur

set ir5=Resources\TableDesigner\TableActivator.ico,Comzept.Genesis.NetRix.Resources.TableDesigner.TableActivator.ico

set ir12=Resources\ToolBox.ico,Comzept.Genesis.NetRix.Resources.ToolBox.ico

set ir100=Resources\DragCursors\WSIconAnchor.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconAnchor.ico
set ir101=Resources\DragCursors\WSIconBreak.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconBreak.ico
set ir102=Resources\DragCursors\WSIconButton.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconButton.ico
set ir103=Resources\DragCursors\WSIconDIV.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconDIV.ico
set ir104=Resources\DragCursors\WSIconDropDownList.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconDropDownList.ico
set ir105=Resources\DragCursors\WSIconForm.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconForm.ico
set ir106=Resources\DragCursors\WSIconHRule.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconHRule.ico
set ir107=Resources\DragCursors\WSIconImage.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconImage.ico
set ir108=Resources\DragCursors\WSIconInputCheckBox.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputCheckBox.ico
set ir109=Resources\DragCursors\WSIconInputHidden.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputHidden.ico
set ir110=Resources\DragCursors\WSIconInputImage.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputImage.ico
set ir111=Resources\DragCursors\WSIconInputPassW.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputPassW.ico
set ir112=Resources\DragCursors\WSIconInputRadioButton.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputRadioButton.ico
set ir113=Resources\DragCursors\WSIconInputText.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconInputText.ico
set ir114=Resources\DragCursors\WSIconOK2Button.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconOK2Button.ico
set ir115=Resources\DragCursors\WSIconFileButton.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconFileButton.ico
set ir116=Resources\DragCursors\WSIconParagraph.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconParagraph.ico
set ir117=Resources\DragCursors\WSIconSelectList.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconSelectList.ico
set ir118=Resources\DragCursors\WSIconTable.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconTable.ico
set ir119=Resources\DragCursors\WSIconTextArea.ico,Comzept.Genesis.NetRix.Resources.DragCursors.WSIconTextArea.ico

rem Preset Resources

set imgres=/res:%ir1% /res:%ir2% /res:%ir3% /res:%ir4% /res:%ir5% /res:%ir12% /res:%ir100% /res:%ir101% /res:%ir102% /res:%ir103% /res:%ir104% /res:%ir105% /res:%ir106% /res:%ir107% /res:%ir108% /res:%ir109%  /res:%ir110% /res:%ir111% /res:%ir112% /res:%ir113% /res:%ir114% /res:%ir115% /res:%ir116% /res:%ir117% /res:%ir118% /res:%ir119%
set frmres=%frK% %frL% %frM%

rem ##############################################################################################################
rem Call CSC Compiler with all switches 
rem
rem Use preprocessor symbols as well: WITHUI, PROFESSIONAL == Full version (discard usage: Light version)
rem                                   NDOC == Create documentation (changes the documentor slightly, doc is still
rem                                           created even if the flag is not set)
rem                                   DEBUG, TRACE == make debug files
rem
rem                                   Alternative compiling process: %2 == FORDEMO to make demo licensing
rem
rem ##############################################################################################################

rem Build Debug version of full version
rem ==============================================================================================================
if "%1"=="DBG" %FW%\csc.exe /t:library /debug+ /w:1 /optimize+ /unsafe /define:WITHUI;DEBUG;TRACE;PROFESSIONAL;%2 %imgres% %frmres% /win32res:%w32% /out:%outd% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% /r:%rrdbg%

rem Build Debug version of light version
rem ==============================================================================================================
if "%1"=="DBG" %FW%\csc.exe /t:library /debug+ /w:1 /optimize+ /unsafe /define:DEBUG;TRACE %imgres% %frmres% /win32res:%w32% /out:%outdlight% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7%

rem Build documentation based on a full version release
rem ==============================================================================================================
if "%1%3"=="RELNDOC" %FW%\csc.exe /t:library /doc:Comzept.Genesis.NetRix.xml /unsafe /define:WITHUI;NDOC;PROFESSIONAL /incremental- /debug- /w:1 /optimize+ %imgres% %frmres% /win32res:%w32% /out:%outr% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% /r:%rrrel%

rem Build release of full version
rem ==============================================================================================================
if "%1"=="REL" %FW%\csc.exe /t:library /debug- /w:1 /optimize+ /unsafe /define:WITHUI;PROFESSIONAL;%2 %imgres% %frmres% /win32res:%w32% /out:%outr% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% /r:%rrrel%

rem Build release of light version
rem ==============================================================================================================
if "%1"=="REL" %FW%\csc.exe /t:library /debug- /w:1 /optimize+ /unsafe %imgres% %frmres% /win32res:%w32% /out:%outrlight% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7%

goto FINAL

:ERROR
echo ------------------------
echo (C) 2004 by Joerg Krause
echo ------------------------ 
echo Parameter error: Please use this syntax: 
echo "make DBG|REL 1.0|1.1 [NDOC]"
echo ------------------------ 

:FINAL
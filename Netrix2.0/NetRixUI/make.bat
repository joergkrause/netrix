echo off
echo Version 1.2 - 17.2.2004

rem ##############################################################################################################
rem Makefile for Net.Rix UI component. It compiles the debug and release version in one step
rem
rem Usage;: %1 "DBG" to compile debug, "REL" with release 
rem         %2 "1.0" or "1.1" for the version which we compile for
rem         %3 "NDOC" to create the documentation file, only recognized if %1 == REL
rem
rem (C) 2003-2004 by Comzept Systemhaus GmbH, Jörg Krause, <joerg@krause.net>; All rights reserved
rem (C) 2004-2010 by Guru Components LLC
rem
rem Created October 13th, 2003
rem Reviewed July 2010
rem ##############################################################################################################

rem Set to specific FW version

if "%2"=="1.0" goto FW10
if "%2"=="1.1" goto FW11
if "%2"=="" goto ERROR

:FW10
set FW=C:\WINDOWS\Microsoft.NET\Framework\v1.0.3705
rem set FWR=C:\Programme\Microsoft.NET\FrameworkSDK\Bin
set FWR="C:\Programme\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin"
goto START
:FW11
set FW=C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322
set FWR="C:\Programme\Microsoft Visual Studio .NET 2003\SDK\v1.1\Bin"
goto START

:START

echo Compiling for %FW% now:

rem Compile RESX resources built with VS.NET into .resources files

%FWR%\resgen UserInterface\ColorPickerUserControl\ColorPickerUserControl.resx
%FWR%\resgen UserInterface\ColorPickerUserControl\ColorPanel\ColorPanelUserControl.resx
%FWR%\resgen UserInterface\ColorPickerUserControl\ColorPanel\ColorPanel.resx
%FWR%\resgen UserInterface\ColorPickerUserControl\ColorPanel\ColorPanelForm.resx
%FWR%\resgen UserInterface\ColorPickerUserControl\ColorSlider\ColorSlider.resx
%FWR%\resgen UserInterface\ColorPickerUserControl\ColorSlider\ColorSliderForm.resx
%FWR%\resgen UserInterface\FontPickerForm\BaseFontListBox.resx
%FWR%\resgen UserInterface\FontPickerForm\FontListBox.resx
%FWR%\resgen UserInterface\FontPickerForm\FontPickerUserControl.resx
%FWR%\resgen UserInterface\FontPickerForm\FontForm.resx
%FWR%\resgen UserInterface\FontPickerForm\FontPicker.resx
%FWR%\resgen UserInterface\StyleEditorForm\StyleUserControl.resx
%FWR%\resgen UserInterface\StyleEditorForm\StyleEditorForm.resx
%FWR%\resgen UserInterface\PropertyGrid\LocalizedPropertyGrid.resx
%FWR%\resgen UserInterface\PropertyGrid\Toolbar.resx
%FWR%\resgen UserInterface\UnitEditorUserControl\UnitEditor.resx

rem resgen TypeEditors\UITypeEditorColor.resx
rem resgen TypeEditors\UITypeEditorComment.resx
rem resgen TypeEditors\UITypeEditorFont.resx
rem resgen TypeEditors\UITypeEditorInt.resx
rem resgen TypeEditors\UITypeEditorScript.resx
rem resgen TypeEditors\UITypeEditorString.resx
rem resgen TypeEditors\UITypeEditorStyleStyle.resx
rem resgen TypeEditors\UITypeEditorUnit.resx
rem resgen TypeEditors\UITypeEditorUrl.resx
	
rem ################# Neutral (language independent) Resource ################

%FWR%\resgen /compile Resource.resx

set lr=/res:Resource.resources,GuruComponents.Netrix.Resource.resources

rem ################# Make File ################# 

if "%1"=="REL" set target=bin\Release
if "%1"=="DBG" set target=bin\Debug

set outd=%target%\GuruComponents.Netrix.UI.dll
set outr=%target%\GuruComponents.Netrix.UI.dll

set r1=System.dll
set r2=System.Web.dll
set r3=System.Data.dll
set r4=System.Drawing.dll
set r5=System.Windows.Forms.dll
set r6=System.Xml.dll
set r7=System.Design.dll

rem ################# Embedded Form Resources ################# 

set fr1=/res:UserInterface\ColorPickerUserControl\ColorPickerUserControl.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorPickerUserControl.resources
set fr2=/res:UserInterface\ColorPickerUserControl\ColorPanel\ColorPanelUserControl.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanelUserControl.resources
set fr3=/res:UserInterface\ColorPickerUserControl\ColorPanel\ColorPanel.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanel.resources
set fr4=/res:UserInterface\ColorPickerUserControl\ColorPanel\ColorPanelForm.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorPanelForm.resources

set fr5=/res:UserInterface\ColorPickerUserControl\ColorSlider\ColorSlider.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorSlider.resources
set fr6=/res:UserInterface\ColorPickerUserControl\ColorSlider\ColorSliderForm.resources,GuruComponents.Netrix.UserInterface.ColorPicker.ColorSliderForm.resources

set fr7=/res:UserInterface\FontPickerForm\BaseFontListBox.resources,GuruComponents.Netrix.UserInterface.FontPickerControl.BaseFontListBox.resources
set fr8=/res:UserInterface\FontPickerForm\FontListBox.resources,GuruComponents.Netrix.UserInterface.FontPickerControl.FontListBox.resources
set fr9=/res:UserInterface\FontPickerForm\FontPickerUserControl.resources,GuruComponents.Netrix.UserInterface.FontPickerControl.FontPickerUserControl.resources
set fr0=/res:UserInterface\FontPickerForm\FontForm.resources,GuruComponents.Netrix.UserInterface.FontPickerControl.FontForm.resources
set frA=/res:UserInterface\FontPickerForm\FontPicker.resources,GuruComponents.Netrix.UserInterface.FontPickerControl.FontPicker.resources
set frB=/res:UserInterface\StyleEditorForm\StyleUserControl.resources,GuruComponents.Netrix.UserInterface.StyleControl.StyleUserControl.resources
set frC=/res:UserInterface\StyleEditorForm\StyleEditorForm.resources,GuruComponents.Netrix.UserInterface.StyleControl.StyleEditorForm.resources
set frD=/res:UserInterface\PropertyGrid\LocalizedPropertyGrid.resources,GuruComponents.Netrix.UserInterface.PropertyGrid.LocalizedPropertyGrid.resources
set frE=/res:UserInterface\PropertyGrid\Toolbar.resources,GuruComponents.Netrix.UserInterface.PropertyGrid.Toolbar.resources
set frF=/res:UserInterface\UnitEditorUserControl\UnitEditor.resources,GuruComponents.Netrix.UserInterface.UnitEditor.resources


set frG=/res:TypeEditors\UITypeEditorColor.resources,       GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorColor.resources
set frH=/res:TypeEditors\UITypeEditorComment.resources,     GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorComment.resources
set frI=/res:TypeEditors\UITypeEditorFont.resources,        GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorFont.resources
set frJ=/res:TypeEditors\UITypeEditorInt.resources,         GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorInt.resources
set frK=/res:TypeEditors\UITypeEditorScript.resources,      GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorScript.resources
set frL=/res:TypeEditors\UITypeEditorString.resources,      GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorString.resources
set frM=/res:TypeEditors\UITypeEditorStyleStyle.resources,  GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorStyleStyle.resources
set frN=/res:TypeEditors\UITypeEditorUnit.resources,        GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUnit.resources
set frO=/res:TypeEditors\UITypeEditorUrl.resources,         GuruComponents.Netrix.UserInterface.TypeEditors.UITypeEditorUrl.resources

set ir6=/res:Resources\UserInterface\AddFont.ico,           GuruComponents.Netrix.Resources.UserInterface.AddFont.ico
set irA=/res:Resources\UserInterface\RemoveFont.ico,        GuruComponents.Netrix.Resources.UserInterface.RemoveFont.ico
set ir7=/res:Resources\UserInterface\FontDown.ico,          GuruComponents.Netrix.Resources.UserInterface.FontDown.ico
set ir8=/res:Resources\UserInterface\FontUp.ico,            GuruComponents.Netrix.Resources.UserInterface.FontUp.ico

set ir9=/res:Resources\UserInterface\PropertyGridCustomTab.ico,GuruComponents.Netrix.Resources.UserInterface.PropertyGridCustomTab.ico

set ir10=/res:Resources\NoColor.ico,                        GuruComponents.Netrix.Resources.UserInterface.NoColor.ico
set ir11=/res:Resources\WrongColor.ico,                     GuruComponents.Netrix.Resources.UserInterface.WrongColor.ico

set ir12=/res:Resources\ToolBar\Alphabetical.ico,           GuruComponents.Netrix.Resources.ToolBar.Alphabetical.ico
set ir13=/res:Resources\ToolBar\Alphabetical_Hover.ico,     GuruComponents.Netrix.Resources.ToolBar.Alphabetical_Hover.ico
set ir14=/res:Resources\ToolBar\Alphabetical_Selected.ico,  GuruComponents.Netrix.Resources.ToolBar.Alphabetical_Selected.ico
set ir15=/res:Resources\ToolBar\Document.ico,               GuruComponents.Netrix.Resources.ToolBar.Document.ico
set ir16=/res:Resources\ToolBar\Document_Hover.ico,         GuruComponents.Netrix.Resources.ToolBar.Document_Hover.ico
set ir17=/res:Resources\ToolBar\Document_Selected.ico,      GuruComponents.Netrix.Resources.ToolBar.Document_Selected.ico
set ir18=/res:Resources\ToolBar\Events.ico,                 GuruComponents.Netrix.Resources.ToolBar.Events.ico
set ir19=/res:Resources\ToolBar\Events_Hover.ico,           GuruComponents.Netrix.Resources.ToolBar.Events_Hover.ico
set ir20=/res:Resources\ToolBar\Events_Selected.ico,        GuruComponents.Netrix.Resources.ToolBar.Events_Selected.ico
set ir21=/res:Resources\ToolBar\Standard.ico,               GuruComponents.Netrix.Resources.ToolBar.Standard.ico
set ir22=/res:Resources\ToolBar\Standard_Hover.ico,         GuruComponents.Netrix.Resources.ToolBar.Standard_Hover.ico
set ir23=/res:Resources\ToolBar\Standard_Selected.ico,      GuruComponents.Netrix.Resources.ToolBar.Standard_Selected.ico
set ir24=/res:Resources\ToolBar\Categorized.ico,            GuruComponents.Netrix.Resources.ToolBar.Categorized.ico
set ir25=/res:Resources\ToolBar\Categorized_Hover.ico,      GuruComponents.Netrix.Resources.ToolBar.Categorized_Hover.ico
set ir26=/res:Resources\ToolBar\Categorized_Selected.ico,   GuruComponents.Netrix.Resources.ToolBar.Categorized_Selected.ico

set f01=/res:Resources\ToolBar\Flag_us.ico,                 GuruComponents.Netrix.Resources.ToolBar.Flag_us.ico
set f02=/res:Resources\ToolBar\Flag_us_Hover.ico,           GuruComponents.Netrix.Resources.ToolBar.Flag_us_Hover.ico
set f03=/res:Resources\ToolBar\Flag_us_Selected.ico,        GuruComponents.Netrix.Resources.ToolBar.Flag_us_Selected.ico
set f04=/res:Resources\ToolBar\Flag_gb.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_gb.ico
set f05=/res:Resources\ToolBar\Flag_gb_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_gb_Hover.ico
set f06=/res:Resources\ToolBar\Flag_gb_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_gb_Selected.ico
set f07=/res:Resources\ToolBar\Flag_au.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_au.ico
set f08=/res:Resources\ToolBar\Flag_au_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_au_Hover.ico
set f09=/res:Resources\ToolBar\Flag_au_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_au_Selected.ico
set f10=/res:Resources\ToolBar\Flag_de.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_de.ico
set f11=/res:Resources\ToolBar\Flag_de_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_de_Hover.ico
set f12=/res:Resources\ToolBar\Flag_de_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_de_Selected.ico
set f13=/res:Resources\ToolBar\Flag_at.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_at.ico
set f14=/res:Resources\ToolBar\Flag_at_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_at_Hover.ico           
set f15=/res:Resources\ToolBar\Flag_at_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_at_Selected.ico
set f16=/res:Resources\ToolBar\Flag_ch.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_ch.ico
set f17=/res:Resources\ToolBar\Flag_ch_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_ch_Hover.ico
set f18=/res:Resources\ToolBar\Flag_ch_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_ch_Selected.ico
set f19=/res:Resources\ToolBar\Flag_cn.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_cn.ico
set f20=/res:Resources\ToolBar\Flag_cn_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_cn_Hover.ico
set f21=/res:Resources\ToolBar\Flag_cn_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_cn_Selected.ico
set f22=/res:Resources\ToolBar\Flag_jp.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_jp.ico      
set f23=/res:Resources\ToolBar\Flag_jp_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_jp_Hover.ico           
set f24=/res:Resources\ToolBar\Flag_jp_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_jp_Selected.ico
set f25=/res:Resources\ToolBar\Flag_fr.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_fr.ico      
set f26=/res:Resources\ToolBar\Flag_fr_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_fr_Hover.ico
set f27=/res:Resources\ToolBar\Flag_fr_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_fr_Selected.ico
set f28=/res:Resources\ToolBar\Flag_es.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_es.ico      
set f29=/res:Resources\ToolBar\Flag_es_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_es_Hover.ico
set f30=/res:Resources\ToolBar\Flag_es_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_es_Selected.ico
set f31=/res:Resources\ToolBar\Flag_ru.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_ru.ico      
set f32=/res:Resources\ToolBar\Flag_ru_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_ru_Hover.ico
set f33=/res:Resources\ToolBar\Flag_ru_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_ru_Selected.ico
set f34=/res:Resources\ToolBar\Flag_pt.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_pt.ico      
set f35=/res:Resources\ToolBar\Flag_pt_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_pt_Hover.ico
set f36=/res:Resources\ToolBar\Flag_pt_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_pt_Selected.ico
set f37=/res:Resources\ToolBar\Flag_it.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_it.ico      
set f38=/res:Resources\ToolBar\Flag_it_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_it_Hover.ico
set f39=/res:Resources\ToolBar\Flag_it_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_it_Selectedico
set f40=/res:Resources\ToolBar\Flag_eg.ico",                GuruComponents.Netrix.Resources.ToolBar.Flag_eg.ico      
set f41=/res:Resources\ToolBar\Flag_eg_Hover.ico",          GuruComponents.Netrix.Resources.ToolBar.Flag_eg_Hover.ico
set f42=/res:Resources\ToolBar\Flag_eg_Selected.ico",       GuruComponents.Netrix.Resources.ToolBar.Flag_eg_Selected.ico

set ir27=/res:Resources\ToolBox\ColorEditor.ico,            GuruComponents.Netrix.Resources.ToolBox.ColorEditor.ico
set ir28=/res:Resources\ToolBox\ColorPicker.ico,            GuruComponents.Netrix.Resources.ToolBox.ColorPicker.ico
set ir29=/res:Resources\ToolBox\FontPicker.ico,             GuruComponents.Netrix.Resources.ToolBox.FontPicker.ico
set ir30=/res:Resources\ToolBox\PropertyGrid.ico,           GuruComponents.Netrix.Resources.ToolBox.PropertyGrid.ico
set ir31=/res:Resources\ToolBox\StyleEditor.ico,            GuruComponents.Netrix.Resources.ToolBox.StyleEditor.ico
set ir32=/res:Resources\ToolBox\UnitEditor.ico,             GuruComponents.Netrix.Resources.ToolBox.UnitEditor.ico

set ir33=/res:Resources\UserInterface\ColorPickerPopUpButton.ico,        GuruComponents.Netrix.Resources.UserInterface.ColorPickerPopUpButton.ico
set ir34=/res:Resources\UserInterface\BorderAll.ico,        GuruComponents.Netrix.Resources.UserInterface.BorderAll.ico
set ir35=/res:Resources\UserInterface\BorderBottom.ico,     GuruComponents.Netrix.Resources.UserInterface.BorderBottom.ico
set ir36=/res:Resources\UserInterface\BorderLeft.ico,       GuruComponents.Netrix.Resources.UserInterface.BorderLeft.ico
set ir37=/res:Resources\UserInterface\BorderRight.ico,      GuruComponents.Netrix.Resources.UserInterface.BorderRight.ico
set ir38=/res:Resources\UserInterface\BorderTop.ico,        GuruComponents.Netrix.Resources.UserInterface.BorderTop.ico
set ir39=/res:Resources\UserInterface\LayoutPage.ico,       GuruComponents.Netrix.Resources.UserInterface.LayoutPage.ico
set ir40=/res:Resources\UserInterface\ListsPage.ico,        GuruComponents.Netrix.Resources.UserInterface.ListsPage.ico
set ir41=/res:Resources\UserInterface\OtherPage.ico,        GuruComponents.Netrix.Resources.UserInterface.OtherPage.ico
set ir42=/res:Resources\UserInterface\TablesPage.ico,       GuruComponents.Netrix.Resources.UserInterface.TablesPage.ico
set ir43=/res:Resources\UserInterface\PositionPage.ico,     GuruComponents.Netrix.Resources.UserInterface.PositionPage.ico
set ir44=/res:Resources\UserInterface\TextPage.ico,         GuruComponents.Netrix.Resources.UserInterface.TextPage.ico
set ir45=/res:Resources\UserInterface\FontPage.ico,         GuruComponents.Netrix.Resources.UserInterface.FontPage.ico

set ir50=/res:Resources\UserInterface\StyleEditor_Columns.bmp,              GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Columns.bmp
set ir51=/res:Resources\UserInterface\StyleEditor_Layout_DirectionLtr.bmp,  GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Layout_DirectionLtr.bmp
set ir52=/res:Resources\UserInterface\StyleEditor_Layout_DirectionRtl.bmp,  GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Layout_DirectionRtl.bmp
set ir53=/res:Resources\UserInterface\StyleEditor_Lists_Position.bmp,       GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Lists_Position.bmp
set ir54=/res:Resources\UserInterface\StyleEditor_Lists_Style.bmp,          GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Lists_Style.bmp
set ir55=/res:Resources\UserInterface\StyleEditor_Lists_Without.bmp,        GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Lists_Without.bmp
set ir56=/res:Resources\UserInterface\StyleEditor_Misc_Arrows.bmp,          GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Misc_Arrows.bmp
set ir57=/res:Resources\UserInterface\StyleEditor_PaddingMargin.bmp,        GuruComponents.Netrix.Resources.UserInterface.StyleEditor_PaddingMargin.bmp
set ir58=/res:Resources\UserInterface\StyleEditor_Position_Center.bmp,      GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Position_Center.bmp
set ir59=/res:Resources\UserInterface\StyleEditor_Position_Justify.bmp,     GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Position_Justify.bmp
set ir60=/res:Resources\UserInterface\StyleEditor_Position_Left.bmp,        GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Position_Left.bmp
set ir61=/res:Resources\UserInterface\StyleEditor_Position_Right.bmp,       GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Position_Right.bmp
set ir62=/res:Resources\UserInterface\StyleEditor_Tables_CaptionPos.bmp,    GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Tables_CaptionPos.bmp
set ir63=/res:Resources\UserInterface\StyleEditor_Tables_ColSpan.bmp,       GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Tables_ColSpan.bmp
set ir64=/res:Resources\UserInterface\StyleEditor_Tables_RowSpan.bmp,       GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Tables_RowSpan.bmp
set ir65=/res:Resources\UserInterface\StyleEditor_Tables_RuleStyle.bmp,     GuruComponents.Netrix.Resources.UserInterface.StyleEditor_Tables_RuleStyle.bmp

rem ##############################################################################################################
rem Preset Resources //imgres = Image Resources frmres = Form Resources
rem ##############################################################################################################

set frmres=%fr1% %fr2% %fr3% %fr4% %fr5% %fr6% %fr7% %fr8% %fr9% %fr0% %frA% %frB% %frC% %frD% %frE% %frF% 

rem We don't need this...
rem %frG% %frH% %frI% %frJ% %frK% %frL% %frM% %frN% %frO%

rem ##############################################################################################################
rem Call CSC Compiler with all switches          
rem ##############################################################################################################

if "%1"=="DBG" %FW%\csc /t:library /debug+ /define:DEBUG;TRACE /w:1 /optimize+ %frmres%  /out:%outd% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% %lr%
if "%1"=="REL" %FW%\csc /t:library /debug- /w:1 /optimize+  %frmres% /out:%outr% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% %lr%
if "%1%3"=="RELNDOC" %FW%\csc.exe /t:library /doc:GuruComponents.Netrix.UI.xml  /debug- /w:1 /optimize+  %frmres% /out:%outr% /noconfig /recurse:*.cs /r:%r1% /r:%r2% /r:%r3% /r:%r4% /r:%r5% /r:%r6% /r:%r7% %lr%

rem ##############################################################################################################
rem Building the satellite assemblies with Assembly Linker and using the release version as a template
rem ##############################################################################################################

rem Build resources from rex files to prepare for building the satellite assemblies, compile, copy to release folder

%FWR%\resgen /compile Resource.ar.resx 
%FWR%\resgen /compile Resource.de.resx
%FWR%\resgen /compile Resource.en.resx
%FWR%\resgen /compile Resource.es.resx
%FWR%\resgen /compile Resource.fr.resx
%FWR%\resgen /compile Resource.it.resx
%FWR%\resgen /compile Resource.ja.resx
%FWR%\resgen /compile Resource.pt.resx
%FWR%\resgen /compile Resource.ru.resx
%FWR%\resgen /compile Resource.zh-CHS.resx

%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.ar.resources,GuruComponents.Netrix.Resource /culture:ar /out:%target%\ar\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.de.resources,GuruComponents.Netrix.Resource /culture:de /out:%target%\de\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.en.resources,GuruComponents.Netrix.Resource /culture:en /out:%target%\en\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.es.resources,GuruComponents.Netrix.Resource /culture:es /out:%target%\es\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.fr.resources,GuruComponents.Netrix.Resource /culture:fr /out:%target%\fr\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.it.resources,GuruComponents.Netrix.Resource /culture:it /out:%target%\it\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.ja.resources,GuruComponents.Netrix.Resource /culture:ja /out:%target%\ja\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.pt.resources,GuruComponents.Netrix.Resource /culture:pt /out:%target%\pt\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.ru.resources,GuruComponents.Netrix.Resource /culture:ru /out:%target%\ru\GuruComponents.Netrix.UI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll
%FW%\al /t:lib /keyfile:C:\KeyStore\NetRixMain.key /embed:Resource.zh-CHS.resources,GuruComponents.Netrix.Resource /culture:zh-CHS /out:%target%\zh-CHS\NetRixUI.resources.dll /template:%target%\GuruComponents.Netrix.UI.dll

goto FINAL

:ERROR
echo ------------------------
echo (C) 2004 by Joerg Krause
echo ------------------------ 
echo Parameter error: Please use this syntax: 
echo "make DBG|REL 1.0|1.1 [NDOC]"
echo ------------------------ 

:FINAL
Windows Ribbon: Preview Ribbon Sample

Preview Ribbon is a 'source available' code sample provided by Microsoft, for the Windows Ribbon framework.

It is an example of a command line app that will compile and preview a Windows Ribbon framework xml file.

It is built using Windows Forms and is therefore also an example of functional managed wrappers for the framework.




Code Sample features:

* Compile and preview a Windows Ribbon framework XML file
* Display compilation output (errors, warnings, etc.)
* Scan markup for application modes and contextual tabs and maps
* Enable / disable contextual UI and app modes at runtime
* Change ribbon colorization settings at runtime
* Config file for defining ribbon colorization settings
* Config file to set custom locations for Link.exe, RC.exe, and UICC.exe





Noteworthy Files:
.\ReadMe.txt			- This file.

.\PreviewRibbon.exe		- The Preview Ribbon sample app

.\PreviewRibbon.exe.config	- The config file for the sample app

.\BasicRibbon.xml 		- Markup file containing example Ribbon UI markup.




Prerequisites:
     1. Microsoft Windows operating system capable of displaying the Windows Ribbon (Windows 7, Server 2008 R2; Vista support with the 'Platform Update for Windows Vista')
     2. Microsoft Windows SDK v7.0 or later





To use the sample:
=================

* This sample executable requires the Windows 7 SDK. Specifically it requires access to 'UICC.exe', 'RC.exe', and 'link.exe'. The sample searches commonly used directories for these. If they cannot be found the PreviewRibbon will display an error.

* "PreviewRibbon.exe.config" - To set custom locations for these three companion executables you can edit the config file, making sure to remove the comment delimeters. Fully qualified paths must be used.

* "PreviewRibbon.exe.config" - This config file will also allow you to set colorization values without using the in app UI. Simply uncomment the colorizaton line in the config file, replacing the default values with your own. #Note# Once colorization is enabled within the app it must be restarted to return to the ribbon's default colors.

* "PreviewRibbon.exe" - This executable takes a single command line argument, a valid ribbon XML file. It's usage is simply "RibbonPreview.exe BasicRibbon.xml". You can also drag and drop a markup file on top of the executable (doing this however, will not show you any reported errors or warnings).

* "PreviewRibbon.exe" - The executable works very well as a 'external tool' within Visual Studio 2008. Once it is added to the 'External Tools Menu' it becomes simple to routinely compile and preview your ribbon UI as you develop.

Comments:
The source code contains comments inline that help to describe what the code is doing. Read these for more information.

-----------------
Legal notice
Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation. 
Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.
© 2009 Microsoft Corporation. All rights reserved. 
Microsoft, MS-DOS, Windows, Windows NT, Windows Server, Windows Vista, Active Directory, ActiveSync, ActiveX, Direct3D, DirectDraw, DirectInput, DirectMusic, DirectPlay, DirectShow, DirectSound, DirectX, Expression, FrontPage, HighMAT, Internet Explorer, JScript, Microsoft Press, MSN, Outlook, PowerPoint, SideShow, Silverlight, Visual Basic, Visual C++, Visual InterDev, Visual J++, Visual Studio, WebTV, Windows Media, Win32, Win32s, and Zune are either registered trademarks or trademarks of Microsoft Corporation in the U.S.A. and/or other countries.
All other trademarks are property of their respective owners.
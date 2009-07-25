@echo off
::setlocal EnableDelayedExpansion

set VERSION=1.0.7
set NSIS="%ProgramFiles%\NSIS\makensis.exe" /V1 /DPRODUCT_VERSION=%VERSION%
set SRC=..\..\src\
set RELEASE=%SRC%LastHorizonte.Launcher\bin\Release\
set DEPLOY=.\deploy\%VERSION%\
set MSBUILD=%WINDIR%\Microsoft.NET\Framework\v3.5\MSBuild.exe
set DOWNLOADS=http://last-horizonte.googlecode.com/files/

pushd %~dp0

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
: Build solution
echo [assembly: System.Reflection.AssemblyVersion("%VERSION%")] > "%SRC%SolutionVersion.cs"
%MSBUILD% "%SRC%LastHorizonte.sln" /property:Configuration=Release

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
: Build packages

:: Setup
%NSIS% installer.nsi

:: Binaries
7za a -mx9 last-horizonte-%VERSION%-bin.zip    "%RELEASE%*.*" -x!*.pdb -x!*.vshost.exe

:: Update (same as binaries without launcher)
7za a -mx9 last-horizonte-%VERSION%-update.zip "%RELEASE%*.*" -x!*.pdb -x!*.vshost.exe -x!LastHorizonte.exe

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
: Move packages to deploy folder

mkdir "%DEPLOY%"
del /q "%DEPLOY%"
move last-horizonte-%VERSION%-installer.exe "%DEPLOY%"
move last-horizonte-%VERSION%-bin.zip       "%DEPLOY%"
move last-horizonte-%VERSION%-update.zip    "%DEPLOY%"

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
: Build UpdateVersion.xml in deploy folder

echo ^<VersionConfig^>^<AvailableVersion^>%VERSION%^</AvailableVersion^>          > "%DEPLOY%UpdateVersion.xml"
echo ^<AppFileUrl^>%DOWNLOADS%last-horizonte-%VERSION%-update.zip^</AppFileUrl^> >> "%DEPLOY%UpdateVersion.xml"
type UpdateVersion.template.xml                                                  >> "%DEPLOY%UpdateVersion.xml"

:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:end
popd

pause

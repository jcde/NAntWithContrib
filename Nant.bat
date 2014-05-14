@echo off
if %PROCESSOR_ARCHITECTURE%==x86 goto x86
"%ProgramFiles(x86)%\NAnt\bin\NAnt.exe" %*
goto end

:x86
"%ProgramFiles%\NAnt\bin\NAnt.exe" %*

:end
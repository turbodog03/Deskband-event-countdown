@echo off
%1 mshta vbscript:createobject("shell.application").shellexecute("%~s0","::","","runas",1)(window.close)&exit
cd /d %~dp0
"%~dp0\RegAsm.exe" /nologo /codebase "eventCountDown.dll"

Pause
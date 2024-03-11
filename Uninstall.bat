@echo off

"%~dp0\RegAsm.exe" /nologo /unregister "eventCountDown.dll"

taskkill /f /im "explorer.exe"
start explorer.exe

Pause
@echo off
@cd CineTest
@dotnet publish -c Release -r ubuntu.16.04-x64
@cd ..\CineTestApi
@dotnet publish -c Release -r ubuntu.16.04-x64
@pause
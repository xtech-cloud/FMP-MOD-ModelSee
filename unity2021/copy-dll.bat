
@echo off

REM !!! Generated by the fmp-cli 1.88.0.  DO NOT EDIT!

md ModelSee\Assets\3rd\fmp-xtc-modelsee

cd ..\vs2022
dotnet build -c Release

copy fmp-xtc-modelsee-lib-mvcs\bin\Release\netstandard2.1\*.dll ..\unity2021\ModelSee\Assets\3rd\fmp-xtc-modelsee\
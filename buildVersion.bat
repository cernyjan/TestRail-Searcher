@echo off

C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MsBuild.exe TestRail-Searcher.sln /t:Build /p:Configuration=Release /p:TargetFramework=v4.7.1

SET app=.\TestRail-Searcher\bin\Release\TestRail-Searcher.application
IF EXIST %app% (
    echo Deleting %app%
    del %app%
)

SET appp=.\TestRail-Searcher\bin\Release\app.publish
IF EXIST %appp% (
    echo Deleting %appp%
    rmdir /s/q %appp%
)

SET manifest=.\TestRail-Searcher\bin\Release\TestRail-Searcher.exe.manifest
IF EXIST %manifest% (
    echo Deleting %manifest%
    del %manifest%
)

SET db=.\TestRail-Searcher\bin\Release\Database.db
IF EXIST %db% (
    echo Deleting %db%
    del %db%
)

SET err=.\TestRail-Searcher\bin\Release\TestRail-Searcher_errors.log
IF EXIST %err% (
    echo Deleting %err%
    del %err%
)
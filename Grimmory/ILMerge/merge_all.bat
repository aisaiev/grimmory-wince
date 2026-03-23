@ECHO OFF

rem #    set .NET Compact Framework version
set net="v2, C:\WINDOWS\Microsoft.NET\Framework\v3.5"
set output=Output
set cflib=C:\Program Files\Microsoft.NET\SDK\CompactFramework\v3.5\WindowsCE

rem #    process arguments
set ILMergeSolution=%1ILMerge\ILMerge.exe

rem # determine programm files of x86 for 32 and 64 Platform
IF     EXIST "%PROGRAMFILES(x86)%" set prorgammFiles=%PROGRAMFILES(x86)%
IF NOT EXIST "%PROGRAMFILES(x86)%" set prorgammFiles=%PROGRAMFILES%

rem #	if ILMerge.exe not in the $(SolutionDir)ILMerge\
rem #		then try to use installed in prorgammFiles
IF     EXIST %ILMergeSolution% set ILMerge="%ILMergeSolution%"
IF NOT EXIST %ILMergeSolution% set ILMerge=%prorgammFiles%\Microsoft\ILMerge\ILMerge.exe

set target_path=%2
set target_file=%~nx2
set target_dir=%~dp2
set ConfigurationName=%3

rem #    set output path and result file path
set outdir=%target_dir%%output%
set result=%outdir%\%target_file%

rem #    print info
@echo     Start %ConfigurationName% Merging %target_file%. 
@echo Target: %target_path%
@echo target_dir: %target_dir%
@echo Config: %ConfigurationName% 

rem #    recreate outdir
IF EXIST "%outdir%" rmdir /S /Q "%outdir%"
md "%outdir%"

rem #    run merge cmd
@echo Merging: '"%ILMerge%" /wildcards /ndebug /closed /lib:"%cflib%" /out:"%result%" %target_path% "%target_dir%*.dll"'
"%ILMerge%" /wildcards /ndebug /closed /lib:"%cflib%" /out:"%result%" %target_path% "%target_dir%*.dll"

rem #    if succeded
IF %ErrorLevel% EQU 0 (
    
    rem #    clear real output folder and put there result assembly
    IF %ConfigurationName%==Release (

        del /Q %target_dir%*.dll 2>nul
        del /Q %target_dir%*.pdb 2>nul
        del /Q %target_dir%*.xml 2>nul
        
        copy /Y %result% %target_dir% >nul
        rmdir /S /Q %outdir% 2>nul
        set result=%target_path% 
        @echo Result: %target_file% "->  %target_path%"
    ) ELSE (
       @echo Result: %target_file% "->  %result%" )
   
   set status=succeded
   set errlvl=0    
) ELSE (
    set status=failed 
    set errlvl=1
    )

@echo Merge %status%
exit %errlvl%
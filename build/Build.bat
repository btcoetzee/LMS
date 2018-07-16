SET OUTPUTFILE="C:\null\LMSBuildOutput.txt"

REM -------------------------------------------------
REM LMS.LeadEntity
REM -------------------------------------------------
msbuild ..\LMS.LeadEntity\LMS.LeadEntity.sln >%OUTPUTFILE% 
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.Validator
REM -------------------------------------------------
msbuild ..\LMS.Validator\LMS.Validator.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.Decorator
REM -------------------------------------------------
msbuild ..\LMS.Decorator\LMS.Decorator.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.Publisher
REM -------------------------------------------------
msbuild ..\LMS.Publisher\LMS.Publisher.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.LoggerClient
REM -------------------------------------------------
msbuild ..\LMS.LoggerClient\LMS.LoggerClient.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.Resolution
REM -------------------------------------------------
msbuild ..\LMS.Resolution\LMS.Resolution.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.LeadCollector
REM -------------------------------------------------
msbuild ..\LMS.LeadCollector\LMS.LeadCollector.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.Campaign
REM -------------------------------------------------
msbuild ..\LMS.Campaign\LMS.Campaign.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
REM -------------------------------------------------
REM LMS.CampaignManager
REM -------------------------------------------------
msbuild ..\LMS.CampaignManager\LMS.CampaignManager.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause


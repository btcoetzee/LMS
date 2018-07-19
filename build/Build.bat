SET OUTPUTFILE="C:\null\LMSBuildOutput.txt"
@ECHO OFF
ECHO -------------------------------------------------
ECHO LMS.LeadEntity
ECHO -------------------------------------------------
msbuild ..\LMS.LeadEntity\LMS.LeadEntity.sln >%OUTPUTFILE% 
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.Validator
ECHO -------------------------------------------------
msbuild ..\LMS.Validator\LMS.Validator.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.Decorator
ECHO -------------------------------------------------
msbuild ..\LMS.Decorator\LMS.Decorator.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.Publisher
ECHO -------------------------------------------------
msbuild ..\LMS.Publisher\LMS.Publisher.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.LoggerClient
ECHO -------------------------------------------------
msbuild ..\LMS.LoggerClient\LMS.LoggerClient.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.Resolution
ECHO -------------------------------------------------
msbuild ..\LMS.Resolution\LMS.Resolution.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.LeadCollector
ECHO -------------------------------------------------
msbuild ..\LMS.LeadCollector\LMS.LeadCollector.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.Campaign
ECHO -------------------------------------------------
msbuild ..\LMS.Campaign\LMS.Campaign.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.CampaignManager
ECHO -------------------------------------------------
msbuild ..\LMS.CampaignManager\LMS.CampaignManager.sln >%OUTPUTFILE%  
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause
ECHO -------------------------------------------------
ECHO LMS.IoC
ECHO -------------------------------------------------
msbuild ..\LMS.IoC\LMS.IoC.sln >%OUTPUTFILE% 
type %OUTPUTFILE%  |  findstr /I /B /R  "Build" |findstr "succeeded." 
if %ERRORLEVEL% NEQ 0 TYPE %OUTPUTFILE%  & ECHO "BUILD FAILED." & pause


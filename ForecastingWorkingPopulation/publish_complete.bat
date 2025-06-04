@echo off
setlocal enabledelayedexpansion

echo ===================================================
echo Pudblish app ForecastingWorkingPopulation
echo ===================================================
echo.

:: Создаем директорию для публикации, если она не существует
if not exist "..\publish" mkdir ..\publish

:: Проверяем наличие файла базы данных
if not exist "Population.db" (
    echo [Error] File db not found Population.db!
    goto :error
)

:: Останавливаем все процессы, которые могут блокировать файлы базы данных
echo [INFO] Clossing all processes that may block the database files...
taskkill /f /im ForecastingWorkingPopulation.exe 2>nul

:: Ждем немного, чтобы процессы корректно завершились
timeout /t 2 /nobreak >nul

:: Проверяем, не заблокирован ли файл базы данных
echo [INFO] Check if the database file is locked...
copy /b "Population.db" nul > nul 2>&1
if %errorlevel% neq 0 (
    echo [Error] Db file was blocked other process!
    echo Close all applications that can use the database and try again..
    goto :error
)

:: Создаем резервную копию базы данных перед публикацией
echo [INFO] Creating a backup copy of the database...
copy /Y "Population.db" "Population.db.backup" > nul
if %errorlevel% neq 0 (
    echo [WARNING] Failed to create a backup copy of the database.
    choice /C YN /M "Continue publishing without backup?"
    if !errorlevel! equ 2 goto :error
) else (
    echo [INFO] The database backup was created successfully: Population.db.backup
)

:: Публикуем приложение с улучшенными параметрами
echo [INFO] Starting the publishing process...
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None -p:DebugSymbols=false -o ..\publish

if %errorlevel% neq 0 (
    echo [ERROR] The application could not be published!
    goto :error
)

:: Проверяем, что исполняемый файл был создан
if not exist "..\publish\ForecastingWorkingPopulation.exe" (
    echo [ERROR] The executable file has not been created!
    goto :error
)

:: Копируем файл базы данных в директорию публикации
echo [INFO] Copying the database file to the publication directory...
copy /Y "Population.db" "..\publish\Population.db" > nul
if %errorlevel% neq 0 (
    echo [WARNING] The database file could not be copied to the publication directory.
    echo The application may not work correctly without the database file.
    choice /C YN /M "Continue without copying the database?"
    if !errorlevel! equ 2 goto :error
) else (
    echo [INFO] The database file has been successfully copied to the publication directory.
)

:: Копируем README.md в директорию публикации
if exist "README.md" (
    echo [INFO] Copying README.md to the publication directory...
    copy /Y "README.md" "..\publish\README.md" > nul
)

echo.
echo ===================================================
echo The publication has been completed successfully!
echo ===================================================
echo.
echo The echo executable file is located in
echo the directory:..\publish\ForecastingWorkingPopulation.exe echo.
echo IMPORTANT: Make sure that the database file is Population.the db is located in the same
echo directory as the executable file. ForecastingWorkingPopulation.exe
echo.
echo To launch the application, open the directory..\publish and run ForecastingWorkingPopulation.exe
echo.
goto :end

:error
echo.
echo ===================================================
echo The publication ended with an error!
echo ===================================================
echo.

:end
pause

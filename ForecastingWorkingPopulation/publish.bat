@echo off
echo Публикация приложения ForecastingWorkingPopulation в режиме единого исполняемого файла
echo.

:: Создаем директорию для публикации, если она не существует
if not exist ".\publish" mkdir .\publish

:: Публикуем приложение
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true -p:IncludeNativeLibrariesForSelfExtract=true -o .\publish

echo.
echo Публикация завершена. Исполняемый файл находится в директории publish.
echo.
pause

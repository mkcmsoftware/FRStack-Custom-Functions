net session >nul 2>&1
if %errorlevel% == 0 (
        echo Copying DLL to FRStack3
        copy %1 %2
)
set %errorlevel% = 0


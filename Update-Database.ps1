Write-Host "Применение миграций к базе данных..." -ForegroundColor Blue

dotnet ef database update `
    --project .\src\DAL `
    --startup-project .\src\Menu

if ($LASTEXITCODE -eq 0) {
    Write-Host "Миграции успешно применены" -ForegroundColor Green
} else {
    Write-Host "Ошибка при применении миграций" -ForegroundColor Red
    Read-Host "Нажмите Enter для выхода"
    exit 1
}

Write-Host "`nСписок миграций:" -ForegroundColor Blue
dotnet ef migrations list `
    --project .\src\DAL `
    --startup-project .\src\API

Read-Host "`nНажмите Enter для выхода"
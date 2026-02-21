$migrationName = Read-Host "Введите название миграции"

Write-Host "Создание миграции $migrationName" -ForegroundColor Blue

dotnet ef migrations add $migrationName `
    --project .\src\DAL `
    --startup-project .\src\Menu\Menu `
    --output-dir Migrations

if ($LASTEXITCODE -eq 0) {
    Write-Host "Миграция успешно создана" -ForegroundColor Green
} else {
    Write-Host "Ошибка при создании миграции" -ForegroundColor Red
    Read-Host "Нажмите Enter для выхода"
    exit 1
}

Read-Host "Нажмите Enter для выхода"
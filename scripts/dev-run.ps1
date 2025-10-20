# Script de auxílio para desenvolvimento local
# 1) sobe o Mongo (docker-compose)
# 2) aguarda Mongo na porta 27017
# 3) roda a aplicação (dotnet run)
# 4) executa testes básicos (POST + GET em /api/v1/motos)

param()

Write-Host "Starting Mongo via docker-compose..."
docker-compose up -d mongo

Write-Host "Waiting for Mongo to be available on localhost:27017..."
$maxTries = 30
$tries = 0
while ($tries -lt $maxTries) {
    try {
        $tcp = New-Object System.Net.Sockets.TcpClient
        $async = $tcp.BeginConnect('127.0.0.1', 27017, $null, $null)
        $wait = $async.AsyncWaitHandle.WaitOne(1000)
        if ($wait -and $tcp.Connected) {
            $tcp.Close()
            break
        }
    } catch {
        # ignore
    }
    Start-Sleep -Seconds 1
    $tries++
    Write-Host "Waiting... ($tries)"
}

if ($tries -ge $maxTries) {
    Write-Warning "Mongo did not start in time. Check Docker and try again."
    exit 1
}

Write-Host "Mongo is up. Starting the API (dotnet run)..."
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run" -WorkingDirectory (Get-Location)

Write-Host "Waiting for API to respond on /docs..."
$tries = 0
while ($tries -lt $maxTries) {
    try {
        $res = Invoke-WebRequest -Uri http://localhost:5049/docs -UseBasicParsing -Method GET -ErrorAction SilentlyContinue
        if ($res.StatusCode -eq 200) { break }
    } catch {
        # ignore
    }
    Start-Sleep -Seconds 1
    $tries++
    Write-Host "Waiting for API... ($tries)"
}

if ($tries -ge $maxTries) {
    Write-Warning "API did not respond in time. Check logs."
    exit 1
}

Write-Host "API is running. Executing a basic POST and GET test for /api/v1/motos"
$body = @{ modelo='Test Moto'; placa='TST1234'; status='Active'; ano=2025 } | ConvertTo-Json
try {
    $post = Invoke-RestMethod -Method Post -Uri http://localhost:5049/api/v1/motos -Body $body -ContentType 'application/json'
    Write-Host "POST result:"; $post | ConvertTo-Json
} catch {
    Write-Warning "POST failed: $_"
}

try {
    $list = Invoke-RestMethod -Method Get -Uri http://localhost:5049/api/v1/motos
    Write-Host "GET result count: " ($list | Measure-Object).Count
} catch {
    Write-Warning "GET failed: $_"
}

Write-Host "Dev run finished. Open http://localhost:5049/docs to view Swagger UI."
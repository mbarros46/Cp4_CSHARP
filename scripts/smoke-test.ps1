param(
    [int]$MaxWaitSeconds = 120
)

Write-Host "[smoke] Starting docker compose (build if needed)..."
docker compose up --build -d

$start = Get-Date
function Elapsed { (Get-Date) - $start }

Write-Host "[smoke] Waiting for API readiness (/health/ready)..."
$ready = $false
while (-not $ready) {
    try {
        $resp = curl -s -o /dev/null -w "%{http_code}" http://localhost:5049/health/ready
        if ($resp -eq '200') { $ready = $true; break }
    } catch {
        # ignore
    }
    if (Elapsed.TotalSeconds -ge $MaxWaitSeconds) {
        Write-Error "[smoke] Timeout waiting for API readiness after $MaxWaitSeconds seconds."
        docker compose logs --no-color
        exit 2
    }
    Start-Sleep -Seconds 2
}

Write-Host "[smoke] API ready. Performing POST -> GET smoke test..."

$payload = @{
    Modelo = "Honda CG 160"
    Placa  = "TEST" + ([guid]::NewGuid().ToString().Substring(0,6)).ToUpper()
    Status = "Active"
    Ano    = 2024
} | ConvertTo-Json

$post = Invoke-RestMethod -Method Post -Uri http://localhost:5049/api/v1.0/motos -Body $payload -ContentType 'application/json' -ErrorAction Stop
if (-not $post.Id) { Write-Error "[smoke] POST did not return an Id"; docker compose logs --no-color; exit 3 }

Write-Host "[smoke] Created Moto Id: $($post.Id)"

$get = Invoke-RestMethod -Method Get -Uri "http://localhost:5049/api/v1.0/motos/$($post.Id)" -ErrorAction Stop
if ($get -and $get.Id -eq $post.Id) {
    Write-Host "[smoke] GET succeeded — smoke test passed"
    exit 0
} else {
    Write-Error "[smoke] GET failed to retrieve created moto"
    docker compose logs --no-color
    exit 4
}

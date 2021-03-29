---
title: Use PowerShell to Spin Up an Integration Test API
date: "2020-05-12"
description: "In developing CI/CD pipelines, I have often come across the need to spin up an api just to run integration tests against."
---

In developing CI/CD pipelines, I have often come across the need to spin up an api just to run integration tests against.  Below is a PowerShell script I came up with to startup an api on another process, check that it is alive, and then run the integration tests against that api.  

> Works like a charm!

```powershell
# Assume the API is not alive.
$isAlive = $false

Set-Location "path/to/integration/test/api"

# Start Test API in the Background
start dotnet run

# Wait for Process

# Define a Check-Process Function
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
Function Check-Process() {
    return Invoke-RestMethod -Method get `
        -ContentType 'Application/Json' `
        -Uri "https://localhost:5001/health" `
        -ErrorAction SilentlyContinue
}

# Check if the API is alive at most 10 times.
For ($i = 0; $i -le 10; $i++) {
    Write-Output "Sleeping for 5 seconds."
    Start-Sleep -Seconds 5
    Write-Output "Trying https://localhost:5001/health"
    $isAlive = Check-Process -eq "Healthy"
    if ($isAlive) {
        Break
    }
}

# Stop if the api is not alive at this point.
if (-Not($isAlive)) {
    Throw "The Integration Src API is not alive after 50 seconds."
}

Set-Location "path/to/integration/tests"

# Run Integration Tests
dotnet run

# Get Process Id so that we can kill it later.
$api_pid = (Get-NetTCPConnection -LocalPort 5001).OwningProcess[0]

# Kill the API Process
Stop-Process -Id $api_pid

```
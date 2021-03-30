---
title: SQL SERVER on Docker with SQLCMD
date: "2020-08-02"
description: "Dockerize SQL SERVER with SQLCMD!"
---

Let's revisit a previous [post](/2019-12-07-docker-sql-server/) about using Docker to create a SQL SERVER image.  Previously, I connected to the DB with SQL SERVER Management Studio (SSMS), but now, I'd like to use `sqlcmd` to avoid running other tools.

Let's get the latest mssql image for Linux this time from https://hub.docker.com/_/microsoft-mssql-server.

```powershell
# Pull the Image
docker pull mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-16.04

# Run the Container
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=Password12345' -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2019-CU5-ubuntu-16.04

# Start an Interactive Shell
docker exec -it sql1 /bin/bash

# Test SQLCMD
opt/mssql-tools/bin/sqlcmd -?
```

> Output

```
Microsoft (R) SQL Server Command Line Tool
Version 17.5.0002.1 Linux
Copyright (C) 2017 Microsoft Corporation. All rights reserved.

usage: sqlcmd            [-U login id]          [-P password]
  [-S server or Dsn if -D is provided]
  [-H hostname]          [-E trusted connection]
  [-N Encrypt Connection][-C Trust Server Certificate]
  [-d use database name] [-l login timeout]     [-t query timeout]
  [-h headers]           [-s colseparator]      [-w screen width]
  [-a packetsize]        [-e echo input]        [-I Enable Quoted Identifiers]
  [-c cmdend]
  [-q "cmdline query"]   [-Q "cmdline query" and exit]
  [-m errorlevel]        [-V severitylevel]     [-W remove trailing spaces]
  [-u unicode output]    [-r[0|1] msgs to stderr]
  [-i inputfile]         [-o outputfile]
  [-k[1|2] remove[replace] control characters]
  [-y variable length type display width]
  [-Y fixed length type display width]
  [-p[1] print statistics[colon format]]
  [-R use client regional setting]
  [-K application intent]
  [-M multisubnet failover]
  [-b On error batch abort]
  [-D Dsn flag, indicate -S is Dsn]
  [-X[1] disable commands, startup script, environment variables [and exit]]
  [-x disable variable substitution]
  [-g enable column encryption]
  [-G use Azure Active Directory for authentication]
  [-? show syntax summary]
```

```powershell
# Run SQLCMD
opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Password12345 -Q "SELECT * FROM INFORMATION_SCHEMA.TABLES"
```

| TABLE_CATALOG | TABLE_SCHEMA | TABLE_NAME            | TABLE_TYPE |
| ------------- | ------------ | --------------------- | ---------- |
| master        | dbo          | spt_fallback_db       | BASE TABLE |
| master        | dbo          | spt_fallback_dev      | BASE TABLE |
| master        | dbo          | spt_fallback_usg      | BASE TABLE |
| master        | dbo          | spt_values            | VIEW       |
| master        | dbo          | spt_monitor           | BASE TABLE |
| master        | dbo          | MSreplication_options | BASE TABLE |

> Very Nice!
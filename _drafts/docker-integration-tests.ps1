dNet='api-int'
appName='biosproapi'
docker rm -f $appName || $true
docker network create $dNet || $true
docker rm -f biosproapi || $true
docker build -t biosproapi:latest -f scripts/docker/Dockerfile .
connectionString="ConnectionStrings:GCSDB=Data Source=ausdwvmcsweb02.aus.amer.dell.com,1434;Initial Catalog=BIOSProDB_DEV_develop;Persist Security Info=True;User ID=BIOSProDBUser;Password=asdlfkjADRH4152!w56h;TrustServerCertificate=True;Encrypt=yes;MultipleActiveResultSets=True"
docker run --rm -p 5000:80 -d --network $dNet --env $connectionString --env DEVELOPMENT_USERNAME="AMERICAS\Joel_Vandiver" --name biosproapi biosproapi:latest
docker build -f tests/GCS.Api.Expecto/Dockerfile -t api-int .
docker run --rm --network $dNet -e BASE_URL=http://$appName api-int 

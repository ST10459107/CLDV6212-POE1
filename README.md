# CoffeeNChill - Part 1

## Docker Setup (Member 3 - Ukhona)

### Build the image

docker build -t coffeenchill-functions:v1.0 .

### Create the shared network

docker network create coffeenchill-network

### Run Azurite

docker run -d --name coffeenchill-azurite --network coffeenchill-network --restart unless-stopped -p 10000:10000 -p 10001:10001 -p 10002:10002 -v C:\\azurite-docker-data:/data mcr.microsoft.com/azure-storage/azurite azurite --location /data --blobHost 0.0.0.0 --queueHost 0.0.0.0 --tableHost 0.0.0.0 --skipApiVersionCheck

### Run the Functions container

docker run -d --name coffeenchill-functions --network coffeenchill-network -p 7071:80 -e AzureWebJobsStorage="DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://coffeenchill-azurite:10000/devstoreaccount1;QueueEndpoint=http://coffeenchill-azurite:10001/devstoreaccount1;TableEndpoint=http://coffeenchill-azurite:10002/devstoreaccount1;" coffeenchill-functions:v1.0

### Docker Hub

Public image: https://hub.docker.com/r/allieukhona/coffeenchill-functions
docker pull allieukhona/coffeenchill-functions:v1.0



\### Azurite image used

mcr.microsoft.com/azure-storage/azurite:latest



\### Member 3 - Ukhona

\- Wrote the multi-stage Dockerfile for the Azure Functions project

\- Fixed the target framework to net8.0 for Docker/runtime compatibility

\- Built and ran the Functions container standalone, connected to Azurite over a Docker network

\- Published the image to Docker Hub as allieukhona/coffeenchill-functions:v1.0


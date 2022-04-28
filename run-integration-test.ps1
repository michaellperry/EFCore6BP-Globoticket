docker build -t globoticket/integration-test:latest -f IntegrationTest/IntegrationTest.Dockerfile .
if (-not $?) {throw "Docker build failed"}

docker compose -f IntegrationTest\docker-compose.yaml up --exit-code-from test --renew-anon-volumes
docker compose -f IntegrationTest\docker-compose.yaml down -v
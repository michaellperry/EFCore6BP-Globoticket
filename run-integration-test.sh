#!/bin/bash
set -e

docker build -t globoticket/integration-test:latest -f IntegrationTest/IntegrationTest.Dockerfile .
docker compose -f IntegrationTest/docker-compose.yaml up --exit-code-from test --renew-anon-volumes
docker compose -f IntegrationTest/docker-compose.yaml down -v
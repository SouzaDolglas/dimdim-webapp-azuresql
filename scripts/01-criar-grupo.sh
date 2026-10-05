#!/usr/bin/env bash
# Cria o grupo de recursos que vai conter tudo do projeto
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"

az group create --name "$RG" --location "$LOCATION" --output table

#!/usr/bin/env bash
# Apaga TODOS os recursos do projeto (rodar só depois da correção!)
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"
az group delete --name "$RG" --yes --no-wait

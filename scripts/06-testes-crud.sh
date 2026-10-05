#!/usr/bin/env bash
# Testa o CRUD das duas tabelas pela API e mostra o SELECT no banco depois de cada operação
set -euo pipefail
source "$(dirname "$0")/00-variaveis.sh"
: "${SQL_PASSWORD:?exporte SQL_PASSWORD antes de rodar}"

URL="https://${WEBAPP}.azurewebsites.net"
J="$(dirname "$0")/../json"
sel() { sqlcmd -S "${SQL_SERVER}.database.windows.net" -d "$SQL_DB" -U "$SQL_ADMIN" -P "$SQL_PASSWORD" -W -Q "$1"; }
passo() { echo; echo "=================== $1 ==================="; }

passo "POST cliente";  CLI=$(curl -s -X POST "$URL/api/clientes" -H 'Content-Type: application/json' -d @"$J/cliente-post.json"); echo "$CLI"
ID_CLI=$(echo "$CLI" | python3 -c 'import sys,json;print(json.load(sys.stdin)["id"])')
sel "SELECT * FROM cliente WHERE id=$ID_CLI"

passo "PUT cliente";   curl -s -X PUT "$URL/api/clientes/$ID_CLI" -H 'Content-Type: application/json' -d @"$J/cliente-put.json"; echo
sel "SELECT * FROM cliente WHERE id=$ID_CLI"

passo "POST conta";    CT=$(sed "s/\"clienteId\": 1/\"clienteId\": $ID_CLI/" "$J/conta-post.json" | curl -s -X POST "$URL/api/contas" -H 'Content-Type: application/json' -d @-); echo "$CT"
ID_CT=$(echo "$CT" | python3 -c 'import sys,json;print(json.load(sys.stdin)["id"])')
sel "SELECT * FROM conta WHERE id=$ID_CT"

passo "PUT conta";     sed "s/\"clienteId\": 1/\"clienteId\": $ID_CLI/" "$J/conta-put.json" | curl -s -X PUT "$URL/api/contas/$ID_CT" -H 'Content-Type: application/json' -d @-; echo
sel "SELECT * FROM conta WHERE id=$ID_CT"

passo "GET contas";    curl -s "$URL/api/contas/$ID_CT"; echo

passo "DELETE conta";  curl -s -o /dev/null -w "HTTP %{http_code}\n" -X DELETE "$URL/api/contas/$ID_CT"
sel "SELECT COUNT(*) AS restantes FROM conta WHERE id=$ID_CT"

passo "DELETE cliente"; curl -s -o /dev/null -w "HTTP %{http_code}\n" -X DELETE "$URL/api/clientes/$ID_CLI"
sel "SELECT COUNT(*) AS restantes FROM cliente WHERE id=$ID_CLI"

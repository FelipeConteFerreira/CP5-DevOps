#!/usr/bin/env bash
# Remove tudo (evita cobranças) depois da apresentação.
source "$(dirname "$0")/00-variaveis.sh"
az group delete -n "$RG" --yes --no-wait

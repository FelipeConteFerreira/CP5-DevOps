#!/usr/bin/env bash
# Gera o Service Principal para o workflow do GitHub Actions.
# Cadastre no GitHub (Settings > Secrets and variables > Actions):
#   AZURE_CREDENTIALS = JSON impresso abaixo | AZURE_WEBAPP_NAME = nome do Web App
set -e
source "$(dirname "$0")/00-variaveis.sh"
SUB=$(az account show --query id -o tsv)
az ad sp create-for-rbac --name "sp-dimdim-$SUFIXO" --role contributor \
  --scopes "/subscriptions/$SUB/resourceGroups/$RG" --sdk-auth
echo "AZURE_WEBAPP_NAME = $APP"

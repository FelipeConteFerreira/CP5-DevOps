#!/usr/bin/env bash
# Cria Resource Group, Web App, Azure SQL, Application Insights e configurações.
set -e
source "$(dirname "$0")/00-variaveis.sh"

az group create -n "$RG" -l "$LOC"

# Application Insights (workspace criado automaticamente)
az extension add -n application-insights --only-show-errors 2>/dev/null || true
az monitor app-insights component create -g "$RG" -a "$INSIGHTS" -l "$LOC" --application-type web
AI_CONN=$(az monitor app-insights component show -g "$RG" -a "$INSIGHTS" --query connectionString -o tsv)

# Azure SQL Database (PaaS)
az sql server create -g "$RG" -n "$SQLSRV" -l "$LOC" -u "$SQL_ADMIN" -p "$SQL_PASSWORD"
az sql db create -g "$RG" -s "$SQLSRV" -n "$SQLDB" --service-objective S0
az sql server firewall-rule create -g "$RG" -s "$SQLSRV" -n AllowAzureServices \
  --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0

# App Service Plan + Web App (.NET 8, Linux)
az appservice plan create -g "$RG" -n "$PLAN" --sku B1 --is-linux
az webapp create -g "$RG" -p "$PLAN" -n "$APP" --runtime "DOTNETCORE:8.0"

# Configurações (segredos ficam só no Azure, nunca no código)
az webapp config appsettings set -g "$RG" -n "$APP" --settings \
  APPLICATIONINSIGHTS_CONNECTION_STRING="$AI_CONN" \
  ApplicationInsightsAgent_EXTENSION_VERSION="~3" >/dev/null
az webapp config connection-string set -g "$RG" -n "$APP" -t SQLAzure --settings \
  DefaultConnection="Server=tcp:$SQLSRV.database.windows.net,1433;Database=$SQLDB;User ID=$SQL_ADMIN;Password=$SQL_PASSWORD;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" >/dev/null

echo "Recursos criados. Web App: https://$APP.azurewebsites.net"

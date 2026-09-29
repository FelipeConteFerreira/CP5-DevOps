#!/usr/bin/env bash
# Executa scripts/ddl.sql no Azure SQL. Requer sqlcmd (mssql-tools) instalado.
# Alternativa sem instalar nada: portal Azure > SQL Database > Query editor > cole o ddl.sql.
set -e
source "$(dirname "$0")/00-variaveis.sh"
MEU_IP=$(curl -s https://api.ipify.org)
az sql server firewall-rule create -g "$RG" -s "$SQLSRV" -n MeuIP \
  --start-ip-address "$MEU_IP" --end-ip-address "$MEU_IP"
sleep 10
sqlcmd -S "$SQLSRV.database.windows.net" -d "$SQLDB" -U "$SQL_ADMIN" -P "$SQL_PASSWORD" \
  -N -i "$(dirname "$0")/ddl.sql"
echo "Tabelas Cliente e Transacao criadas."

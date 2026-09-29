#!/usr/bin/env bash
# Variáveis compartilhadas. Use: source scripts/00-variaveis.sh
# Nenhuma senha fica gravada aqui: ela é pedida no terminal.
export SUFIXO="${SUFIXO:-$(echo $RANDOM | md5sum | cut -c1-5)}"   # fixe com: export SUFIXO=abc12
export RG="rg-dimdim"
export LOC="brazilsouth"            # se o SQL der erro de região, use: eastus2
export PLAN="plan-dimdim"
export APP="dimdim-app-$SUFIXO"
export SQLSRV="sql-dimdim-$SUFIXO"
export SQLDB="dimdimdb"
export INSIGHTS="ai-dimdim"
export SQL_ADMIN="dimdimadmin"
if [ -z "$SQL_PASSWORD" ]; then
  read -s -p "Defina a senha do SQL (min. 8, maiúscula, número e símbolo): " SQL_PASSWORD; echo
  export SQL_PASSWORD
fi
echo "Sufixo: $SUFIXO | Web App: $APP | SQL Server: $SQLSRV"
echo "Anote o sufixo! Para reutilizar depois: export SUFIXO=$SUFIXO"

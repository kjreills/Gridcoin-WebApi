#!/bin/bash

rpcport=${RPC_PORT:-9332}
rpcuser=${RPC_USER:-gridcoinrpc}

if [ -z "$RPC_PASSWORD" ]; then
  echo "RPC_PASSWORD is not set. You must set a non-default RPC password to run this container.";
  exit 1;
fi

sed -i "s/RPC_PORT/$rpcport/" ./.GridcoinResearch/gridcoinresearch.conf
sed -i "s/RPC_USER/$rpcuser/" ./.GridcoinResearch/gridcoinresearch.conf
sed -i "s/RPC_PASSWORD/$RPC_PASSWORD/" ./.GridcoinResearch/gridcoinresearch.conf

fi

gridcoinresearchd -datadir=/home/gridcoin/.GridcoinResearch/ -daemon

INFO=$(gridcoinresearchd getinfo)

while [ "$INFO" = "error: couldn't connect to server" ];
do
  sleep 5;
  INFO=$(gridcoinresearchd getinfo);
done

trap "gridcoinresearchd stop; exit 0" SIGTERM SIGINT
while true; do :; done;

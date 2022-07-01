#!/bin/bash

rpcport=${RPC_PORT:-9332}
rpcuser=${RPC_USER:-gridcoinrpc}
rpcpassword=${RPC_PASSWORD:-rpcpassword}

sed -i "s/RPC_PORT/$rpcport/" ./.GridcoinResearch/testnet/gridcoinresearch.conf
sed -i "s/RPC_USER/$rpcuser/" ./.GridcoinResearch/testnet/gridcoinresearch.conf
sed -i "s/RPC_PASSWORD/$rpcpassword/" ./.GridcoinResearch/testnet/gridcoinresearch.conf

gridcoinresearchd -testnet -daemon

INFO=$(gridcoinresearchd -testnet getinfo)

while [ "$INFO" = "error: couldn't connect to server" ];
do
  sleep 5;
  INFO=$(gridcoinresearchd -testnet getinfo);
done

trap "gridcoinresearchd -testnet stop; exit 0" SIGTERM SIGINT
while true; do :; done;

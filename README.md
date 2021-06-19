# Gridcoin.WebApi

The goal of this project is to create a secure, easy to use Web API that can be
used to access a Gridcoin node over RPC. This service should be hosted on a
network such that it can connect to the Gridcoin node without sending traffic over
the internet. This API uses JWT Bearer token authentication to ensure that you don't
have to send your RPC credentials in requests to this API.

## Development Setup

This API needs access to the Gridcoin node that you are using for development.
To setup the environment variables for RPC communication, run the following commands,
replacing the values with those configured in your Testnet wallet config file.

```sh
dotnet user-secrets set "Gridcoin:Uri" "http://localhost:9332"
dotnet user-secrest set "Gridcoin:Username" "rpcusername"
dotnet user-secrest set "Gridcoin:Password" "averylongandsecurerpcpassword"
```



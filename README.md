# Gridcoin.WebApi

The goal of this project is to create a secure, easy to use Web API that can be
used to access a Gridcoin node without exposing your Gridcoin node directly over RPC. This service should be hosted on a
network such that it can connect to the Gridcoin node without sending traffic over
the internet. This API uses JWT Bearer token authentication to ensure that you don't
have to send your RPC credentials in requests to this API.

## Development Setup

This API needs access to the Gridcoin node that you are using for development.
To setup the environment variables for RPC communication, run the following commands,
replacing the values with those configured in your Testnet wallet config file.

```sh
dotnet user-secrets set "Gridcoin:Uri" "http://localhost:9332"
dotnet user-secrets set "Gridcoin:Username" "rpcusername"
dotnet user-secrets set "Gridcoin:Password" "averylongandsecurerpcpassword"
```

You will also need to provide a way to authenticate using JWT Bearer authentication.
Auth0 offers free accounts that should be sufficient for development use at least.
Once you have configured your OAuth provider of choice, you will need to setup
your development environment with the JWT Authority and Audience, as follows:

```sh
dotnet user-secrets set "Authentication:Authority" "https://mysubdomain.auth0.com/"
dotnet user-secrets set "Authentication:Audience" "https://myaudience.example.com"
```

When configuring your OAuth provider, you will need to setup the authorization scopes
used by this API.

- `read:info` is used for actions that are readonly in nature, such as "getinfo" and "gettransaction"
- `create:address` is the scope for authorizing reading and creation of new addresses
- `create:transaction` is the scope for authorizing the creation of transactions, such as "sendtoaddress"

using CSharpInWeb3SmartContracts.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace CSharpInWeb3SmartContracts.GraphQL
{
    public class UniswapV3GraphQL
    {
        private readonly IGraphQLClient _client;
        public UniswapV3GraphQL(IGraphQLClient client)
        {
            _client = client;
        }

        // "0x1f9840a85d5af5bf1d1762f925bdaddc4201f984"
        public async Task<Token> GetTokenData(string tokenId)
        {
            GraphQLRequest? query = new GraphQLRequest();
            query.Query = @"
                        query tokenQuery($tokenId: ID!) {
                          token(id:$tokenId) 
                            {
                            symbol
                            name
                            decimals
                            volumeUSD
                            poolCount
                            }
                        }";
            query.Variables = new { tokenId };

            GraphQLResponse<TokenResponse>? response = await _client.SendQueryAsync<TokenResponse>(query);
            return response.Data.Token;
        }

        public async Task<dynamic> GetMostLiquidPools(int numberOfPools)
        {
            GraphQLRequest? query = new GraphQLRequest();
            query.Query = "{                                                              " +
             $"    pools(first:{numberOfPools}, orderBy: liquidity, orderDirection: desc)" +
             "     {                                                                     " +
             "      id                                                                   " +
             "      token0 {id name}                                                     " +
             "      token1 {id name}                                                     " +
             "      liquidity                                                            " +
             "    }                                                                      " +
             "  }                                                                        ";

            GraphQLResponse<dynamic>? response = await _client.SendQueryAsync<dynamic>(query);
            return response.Data;
        }

        public async Task<dynamic> GetPoolData(int poolId)
        {
            GraphQLRequest? query = new GraphQLRequest();
            query.Query = "{\r\n  pool(id: \"0x8ad599c3a0ff1de082011efddc58f1908eb6e6d8\") {\r\n    tick\r\n    token0 {\r\n      symbol\r\n      id\r\n      decimals\r\n    }\r\n    token1 {\r\n      symbol\r\n      id\r\n      decimals\r\n    }\r\n    feeTier\r\n    sqrtPrice\r\n    liquidity\r\n  }\r\n}";

            GraphQLResponse<dynamic>? response = await _client.SendQueryAsync<dynamic>(query);
        }

    }
}

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

        // 0x8ad599c3a0ff1de082011efddc58f1908eb6e6d8
        public async Task<dynamic> GetPoolData(string poolAddress)
        {
            GraphQLRequest? query = new GraphQLRequest();
            query.Query = "  {                                " +
                                $"    pool(id: \"{poolAddress}\")  " +
                                "  {    					  " +
                                "      tick                   " +
                                "      token0 {               " +
                                "        symbol               " +
                                "        id                   " +
                                "        decimals             " +
                                "      }                      " +
                                "      token1 {               " +
                                "        symbol               " +
                                "        id                   " +
                                "        decimals             " +
                                "      }                      " +
                                "      feeTier                " +
                                "      sqrtPrice              " +
                                "      liquidity              " +
                                "    }                        " +
                                "  }                          ";

            GraphQLResponse<dynamic>? response = await _client.SendQueryAsync<dynamic>(query);
            return response.Data;
        }

        // 0x7858e59e0c01ea06df3af3d20ac7b0003275d4bf
        public async Task<dynamic> GetRecentSwapsWithinAPool(string poolAddress)
        {
            GraphQLRequest? query = new GraphQLRequest();
            query.Query = "	{														   " +
                            "	swaps(orderBy: timestamp, orderDirection: desc, where:     " +
                            "	 { " +
                            $"   pool: \"{poolAddress}\"    " +
                            "   }  " +
                            "	) {                                                        " +
                            "	  pool {                                                   " +
                            "	    token0 {                                               " +
                            "	      id                                                   " +
                            "	      symbol                                               " +
                            "	    }                                                      " +
                            "	    token1 {                                               " +
                            "	      id                                                   " +
                            "	      symbol                                               " +
                            "	    }                                                      " +
                            "	  }                                                        " +
                            "	  sender                                                   " +
                            "	  recipient                                                " +
                            "	  amount0                                                  " +
                            "	  amount1                                                  " +
                            "	 }                                                         " +
                            "	}                                                          ";

            GraphQLResponse<dynamic>? response = await _client.SendQueryAsync<dynamic>(query);
            return response.Data;
        }

        public async Task<dynamic> GetPositionData(int positionId)
        {

        }
    }
}

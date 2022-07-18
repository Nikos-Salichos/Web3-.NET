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
        }

    }
}

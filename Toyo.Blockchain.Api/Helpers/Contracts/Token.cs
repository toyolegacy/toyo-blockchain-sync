﻿using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Threading.Tasks;
using Toyo.Blockchain.Domain.Dtos;

namespace Toyo.Blockchain.Api.Helpers.Contracts
{
    public static class Token
    {
        public static async Task<TotalSupplyOutputDto> GetTotalSupply(Web3 web3, ulong toBlock, string contractAddress)
        {
            var function = new TotalSupplyFunction();
            var handler = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            var output = await handler.
                QueryDeserializingToObjectAsync<TotalSupplyOutputDto>(
                    function,
                    contractAddress,
                    new BlockParameter(toBlock));
            return output;
        }

        public static async Task<OwnerOfOutputDto> GetTokenOwner(Web3 web3, ulong toBlock, BigInteger tokenId, string contractAddress)
        {
            var function = new OwnerOfFunction()
            {
                TokenId = tokenId
            };
            var handler = web3.Eth.GetContractQueryHandler<OwnerOfFunction>();
            var output = await handler.
                QueryDeserializingToObjectAsync<OwnerOfOutputDto>(
                    function,
                    contractAddress,
                    new BlockParameter(toBlock));
            return output;
        }

        public static async Task<TokenUriOutputDto> GetTokenUri(Web3 web3, ulong toBlock, BigInteger tokenId, string contractAddress)
        {
            var function = new TokenUriFunction()
            {
                TokenId = tokenId
            };
            var handler = web3.Eth.GetContractQueryHandler<TokenUriFunction>();
            var output = await handler.
                QueryDeserializingToObjectAsync<TokenUriOutputDto>(
                    function,
                    contractAddress,
                    new BlockParameter(toBlock));
            return output;
        }

        private const int HASH_START_INDEX = 7;
        private static readonly Dictionary<string, int> uriCache = new();

        public static int? GetTokenTypeId(string uri)
        {
            var uriExists = uriCache.Any(x => x.Key == uri);

            if (uriExists)
            {
                return uriCache[uri];
            }

            using WebClient client = new();

            int? typeId = null;

            var jsonContent = GetJson(client, uri);

            if (!string.IsNullOrEmpty(jsonContent))
            {
                var jsonObject = JObject.Parse(jsonContent).SelectTokens("$.attributes..value").First();
                typeId = jsonObject.Value<int>();
                uriCache[uri] = typeId.Value;
                return typeId;
            }

            return typeId;
        }

        public static string GetTokenTypeName(string uri)
        {
            const string jsonPath = "$.name";
            return GetJsonValue(uri, jsonPath);
        }

        public static string GetTokenTypeImage(string uri)
        {
            const string jsonPath = "$.image";
            return GetJsonValue(uri, jsonPath);
        }

        private static string GetJsonValue(string uri, string jsonPath)
        {
            using WebClient client = new();

            var value = string.Empty;
            var jsonContent = GetJson(client, uri);

            if (!string.IsNullOrEmpty(jsonContent))
            {
                var jsonObject = JObject.Parse(jsonContent).SelectTokens(jsonPath).First();
                return jsonObject.Value<string>();
            }

            return value;
        }

        public static string GetJson(WebClient client, string uri)
        {
            string json = string.Empty;

            if (uri.ToLower().Contains("ipfs"))
            {
                string ipfsHash = uri[HASH_START_INDEX..];
                uri = $"https://ipfs.io/ipfs/{ipfsHash}";
            }

            try
            {
                json = client.DownloadString(uri);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Uri {uri} | Exception {ex.Message} | InnerException {ex.InnerException?.Message}");
            }

            return json;
        }
    }
}

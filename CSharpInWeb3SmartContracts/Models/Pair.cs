﻿namespace CSharpInWeb3SmartContracts.Models
{
    public class Pair
    {
        public string Id { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public Token Token0 { get; set; } = new Token();
        public Token Token1 { get; set; } = new Token();
    }
}

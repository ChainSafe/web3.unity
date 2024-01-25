namespace ChainSafe.Gaming.Exchangers.Ramp
{
    public struct OnRampPurchaseData
    {
        public double AppliedFee { get; set; }
        public AssetInfo Asset { get; set; }
        public double AssetExchangeRate { get; set; }
        public double BaseRampFee { get; set; }
        public string CreatedAt { get; set; }
        public string CryptoAmount { get; set; }
        public string EndTime { get; set; }
        public string EscrowAddress { get; set; }
        public string EscrowDetailsHash { get; set; }
        public string FiatCurrency { get; set; }
        public double FiatValue { get; set; }
        public string FinalTxHash { get; set; }
        public string Id { get; set; }
        public double NetworkFee { get; set; }
        public string PaymentMethodType { get; set; }
        public string ReceiverAddress { get; set; }
        public string Status { get; set; }
        public string UpdatedAt { get; set; }

        public override string ToString()
        {
            return
                $"{nameof(AppliedFee)}: {AppliedFee}, {nameof(Asset)}: {Asset}, {nameof(AssetExchangeRate)}: " +
                $"{AssetExchangeRate}, {nameof(BaseRampFee)}: {BaseRampFee}, {nameof(CreatedAt)}: " +
                $"{CreatedAt}, {nameof(CryptoAmount)}: {CryptoAmount}, {nameof(EndTime)}: {EndTime}, " +
                $"{nameof(EscrowAddress)}: {EscrowAddress}, {nameof(EscrowDetailsHash)}: {EscrowDetailsHash}," +
                $" {nameof(FiatCurrency)}: {FiatCurrency}, {nameof(FiatValue)}: {FiatValue}, {nameof(FinalTxHash)}: " +
                $"{FinalTxHash}, {nameof(Id)}: {Id}, {nameof(NetworkFee)}: {NetworkFee}, {nameof(PaymentMethodType)}: " +
                $"{PaymentMethodType}, {nameof(ReceiverAddress)}: {ReceiverAddress}, {nameof(Status)}: " +
                $"{Status}, {nameof(UpdatedAt)}: {UpdatedAt}";
        }

        public OnRampPurchaseData(double appliedFee, AssetInfo asset, double assetExchangeRate,
            double baseRampFee, string createdAt, string cryptoAmount,
            string endTime, string escrowAddress, string escrowDetailsHash,
            string fiatCurrency, double fiatValue, string finalTxHash,
            string id, double networkFee, string paymentMethodType,
            string receiverAddress, string status, string updatedAt)
        {
            AppliedFee = appliedFee;
            Asset = asset;
            AssetExchangeRate = assetExchangeRate;
            BaseRampFee = baseRampFee;
            CreatedAt = createdAt;
            CryptoAmount = cryptoAmount;
            EndTime = endTime;
            EscrowAddress = escrowAddress;
            EscrowDetailsHash = escrowDetailsHash;
            FiatCurrency = fiatCurrency;
            FiatValue = fiatValue;
            FinalTxHash = finalTxHash;
            Id = id;
            NetworkFee = networkFee;
            PaymentMethodType = paymentMethodType;
            ReceiverAddress = receiverAddress;
            Status = status;
            UpdatedAt = updatedAt;
        }

        public struct AssetInfo
        {
            public string Address { get; set; }
            public int Decimals { get; set; } // Assuming the decimals are whole numbers.
            public string Name { get; set; }
            public string Symbol { get; set; }
            public string Type { get; set; }

            public AssetInfo(string address, int decimals, string name, string symbol, string type)
            {
                Address = address;
                Decimals = decimals;
                Name = name;
                Symbol = symbol;
                Type = type;
            }

            public override string ToString()
            {
                return
                    $"{nameof(Address)}: {Address}, {nameof(Decimals)}: " +
                    $"{Decimals}, {nameof(Name)}: {Name}, {nameof(Symbol)}: {Symbol}, {nameof(Type)}: {Type}";
            }
        }
    }
}
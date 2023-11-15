using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using UsaEPay.NET.Converter;
using UsaEPay.NET.Models.Classes;
using UsaEPay.NET.Models.Enumerations.Event;
using UsaEPay.NET.Models.Events;

namespace UsaEPay.NET.Models
{
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, GenerationMode = JsonSourceGenerationMode.Default)]
    [JsonSerializable(typeof(UsaEPayGetRequest))]
    [JsonSerializable(typeof(UsaEPayRequest))]
    [JsonSerializable(typeof(AmountDetail))]
    [JsonSerializable(typeof(Address))]
    [JsonSerializable(typeof(CreditCard))]
    [JsonSerializable(typeof(Check))]
    [JsonSerializable(typeof(LineItem))]
    [JsonSerializable(typeof(Traits))]
    [JsonSerializable(typeof(UsaEPayResponse))]
    [JsonSerializable(typeof(SavedCard))]
    [JsonSerializable(typeof(AVS))]
    [JsonSerializable(typeof(CVC))]
    [JsonSerializable(typeof(Batch))]
    [JsonSerializable(typeof(Bin))]
    [JsonSerializable(typeof(Fraud))]
    [JsonSerializable(typeof(Receipts))]
    [JsonSerializable(typeof(UsaEPayBatchListResponse))]
    [JsonSerializable(typeof(UsaEPayBatchTransactionResponse))]
    [JsonSerializable(typeof(UsaEPayTransactionParams))]
    [JsonSerializable(typeof(ACHStatusEventResponse))]
    [JsonSerializable(typeof(ACHEventBody))]
    [JsonSerializable(typeof(ACHChanges))]
    [JsonSerializable(typeof(ACHChangeDetails))]
    [JsonSerializable(typeof(ACHObject))]
    [JsonSerializable(typeof(ACHCheck))]
    [JsonSerializable(typeof(BaseEventResponse))]
    [JsonSerializable(typeof(BaseEventBody))]
    [JsonSerializable(typeof(Merchant))]
    [JsonSerializable(typeof(CardUpdateEventResponse))]
    [JsonSerializable(typeof(CardEventBody))]
    [JsonSerializable(typeof(CardChanges))]
    [JsonSerializable(typeof(CardChangeDetails))]
    [JsonSerializable(typeof(CardObject))]
    [JsonSerializable(typeof(CardOriginal))]
    [JsonSerializable(typeof(CardSource))]
    [JsonSerializable(typeof(CardChanges))]
    [JsonSerializable(typeof(SettlementEventResponse))]
    [JsonSerializable(typeof(SettlementEventBody))]
    [JsonSerializable(typeof(SettlementObject))]
    [JsonSerializable(typeof(TransactionEventResponse))]
    [JsonSerializable(typeof(TransactionEventBody))]
    [JsonSerializable(typeof(EventType))]
    [JsonSerializable(typeof(IUsaEPayRequest))]
    [JsonSerializable(typeof(IUsaEPayResponse))]
    public partial class USAePaySerializerContext : JsonSerializerContext
    {
    }
}

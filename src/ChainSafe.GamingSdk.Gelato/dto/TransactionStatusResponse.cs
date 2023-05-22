using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Relay
{
    public enum TaskState
    {
        CheckPending,
        ExecPending,
        ExecSuccess,
        ExecReverted,
        WaitingForConfirmation,
        Blacklisted,
        Cancelled,
        NotFound,
    }

    public class TransactionStatusResponse
    {
        /// <summary>
        ///    QUANTITY - The task object
        /// </summary>
        [JsonProperty(PropertyName = "task")]
        public RelayedTask Task { get; set; }
    }

    public class RelayedTask
    {
        /// <summary>
        ///    QUANTITY - The transaction chain id.
        /// </summary>
        [JsonProperty(PropertyName = "chainId")]
        public HexBigInteger ChainId { get; set; }

        /// <summary>
        ///     DATA - task ID for the relayed transaction.
        /// </summary>
        [JsonProperty(PropertyName = "taskId")]
        public string TaskId { get; set; }

        /// <summary>
        ///     DATA - status for the state of a relayer's task
        /// </summary>
        [JsonProperty(PropertyName = "taskState")]
        public TaskState TaskState { get; set; }

        /// <summary>
        ///     DATA - Creation date of the task.
        /// </summary>
        [JsonProperty(PropertyName = "creationDate")]
        public long CreationDate { get; set; }

        /// <summary>
        ///     DATA - Optional last time the status was queried by the relayer if available.
        /// </summary>
        [JsonProperty(PropertyName = "lastCheckDate")]
        public long LastCheckDate { get; set; }

        /// <summary>
        ///     DATA - Optional last message retrieved in a check if available.
        /// </summary>
        [JsonProperty(PropertyName = "lastCheckMessage")]
        public long LastCheckMessage { get; set; }

        /// <summary>
        ///     DATA - Optional transaction hash identifier if available.
        /// </summary>
        [JsonProperty(PropertyName = "transactionHash")]
        public string TransactionHash { get; set; }

        /// <summary>
        ///     DATA - Optional block number for the transaction if available.
        /// </summary>
        [JsonProperty(PropertyName = "blockNumber")]
        public string BlockNumber { get; set; }

        /// <summary>
        ///     DATA - Optional execution date if the transaction if available.
        /// </summary>
        [JsonProperty(PropertyName = "executionDate")]
        public long ExecutionDate { get; set; }
    }
}
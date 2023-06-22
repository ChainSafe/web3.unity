using Nethereum.Hex.HexTypes;
using Newtonsoft.Json;

namespace ChainSafe.GamingSdk.Gelato.Dto
{
    public enum TaskState
    {
        /// <summary>
        /// Transaction status is being retrieved
        /// </summary>
        CheckPending,

        /// <summary>
        /// Transaction is submitted
        /// </summary>
        ExecPending,

        /// <summary>
        /// Transaction succeeded
        /// </summary>
        ExecSuccess,

        /// <summary>
        /// Transaction failed
        /// </summary>
        ExecReverted,

        /// <summary>
        /// Transaction is awaiting node confirmations
        /// </summary>
        WaitingForConfirmation,

        /// <summary>
        /// Account is banned from submission of transactions
        /// </summary>
        Blacklisted,

        /// <summary>
        /// Transaction was cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// TaskId was not found
        /// </summary>
        NotFound,
    }

    public class TransactionStatusResponse
    {
        /// <summary>
        ///    QUANTITY - The task object.
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
        ///     DATA - status for the state of a relayer's task.
        /// </summary>
        [JsonProperty(PropertyName = "taskState")]
        public TaskState TaskState { get; set; }

        /// <summary>
        ///     DATA - Creation date of the task.
        /// </summary>
        [JsonProperty(PropertyName = "creationDate")]
        public string CreationDate { get; set; }

        /// <summary>
        ///     DATA - Optional last time the status was queried by the relayer if available.
        /// </summary>
        [JsonProperty(PropertyName = "lastCheckDate")]
        public string LastCheckDate { get; set; }

        /// <summary>
        ///     DATA - Optional last message retrieved in a check if available.
        /// </summary>
        [JsonProperty(PropertyName = "lastCheckMessage")]
        public string LastCheckMessage { get; set; }

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
        public string ExecutionDate { get; set; }
    }
}
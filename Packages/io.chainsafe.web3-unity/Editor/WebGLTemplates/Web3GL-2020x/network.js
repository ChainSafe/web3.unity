// Used to set the network, chains can be found at: https://chainlist.org/
// Onboard JS chain config objects, set this to your build information

// interface Chain {
//   namespace?: 'evm';
//   id: ChainId;
//   rpcUrl: string;
//   label: string;
//   token: TokenSymbol;
//   color?: string;
//   icon?: string;
//   providerConnectionInfo?: ConnectionInfo;
//   publicRpcUrl?: string;
//   blockExplorerUrl?: string;
// }

window.networks = [
  {
    id: 11155111,
    label: "Ethereum Sepolia",
    token: "Seth",
    rpcUrl: `https://sepolia.infura.io/v3/287318045c6e455ab34b81d6bcd7a65f`,
  }
]
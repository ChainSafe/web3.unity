using System;
using System.Diagnostics;
using ChainSafe.GamingSDK.EVM.Tests.Node;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using NUnit.Framework;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Utils;

namespace ChainSafe.GamingSDK.EVM.Tests
{
    using Web3 = ChainSafe.GamingWeb3.Web3;

    [TestFixture]
    public class ChainSafeRpcTests
    {
        private const string Nft721ByteCode = "60806040523480156200001157600080fd5b506040518060400160405280600881526020017f436f6e74726163740000000000000000000000000000000000000000000000008152506040518060400160405280600481526020017f4d4e46540000000000000000000000000000000000000000000000000000000081525081600090816200008f919062000412565b508060019081620000a1919062000412565b505050620000c4620000b8620000ca60201b60201c565b620000d260201b60201c565b620004f9565b600033905090565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905081600660006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b600081519050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052602260045260246000fd5b600060028204905060018216806200021a57607f821691505b60208210810362000230576200022f620001d2565b5b50919050565b60008190508160005260206000209050919050565b60006020601f8301049050919050565b600082821b905092915050565b6000600883026200029a7fffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffffff826200025b565b620002a686836200025b565b95508019841693508086168417925050509392505050565b6000819050919050565b6000819050919050565b6000620002f3620002ed620002e784620002be565b620002c8565b620002be565b9050919050565b6000819050919050565b6200030f83620002d2565b620003276200031e82620002fa565b84845462000268565b825550505050565b600090565b6200033e6200032f565b6200034b81848462000304565b505050565b5b8181101562000373576200036760008262000334565b60018101905062000351565b5050565b601f821115620003c2576200038c8162000236565b62000397846200024b565b81016020851015620003a7578190505b620003bf620003b6856200024b565b83018262000350565b50505b505050565b600082821c905092915050565b6000620003e760001984600802620003c7565b1980831691505092915050565b6000620004028383620003d4565b9150826002028217905092915050565b6200041d8262000198565b67ffffffffffffffff811115620004395762000438620001a3565b5b62000445825462000201565b6200045282828562000377565b600060209050601f8311600181146200048a576000841562000475578287015190505b620004818582620003f4565b865550620004f1565b601f1984166200049a8662000236565b60005b82811015620004c4578489015182556001820191506020850194506020810190506200049d565b86831015620004e45784890151620004e0601f891682620003d4565b8355505b6001600288020188555050505b505050505050565b61287f80620005096000396000f3fe608060405234801561001057600080fd5b506004361061010b5760003560e01c806370a08231116100a2578063a22cb46511610071578063a22cb465146102a4578063b88d4fde146102c0578063c87b56dd146102dc578063e985e9c51461030c578063f2fde38b1461033c5761010b565b806370a082311461022e578063715018a61461025e5780638da5cb5b1461026857806395d89b41146102865761010b565b806323b872dd116100de57806323b872dd146101aa57806340d097c3146101c657806342842e0e146101e25780636352211e146101fe5761010b565b806301ffc9a71461011057806306fdde0314610140578063081812fc1461015e578063095ea7b31461018e575b600080fd5b61012a60048036038101906101259190611a7d565b610358565b6040516101379190611ac5565b60405180910390f35b61014861043a565b6040516101559190611b70565b60405180910390f35b61017860048036038101906101739190611bc8565b6104cc565b6040516101859190611c36565b60405180910390f35b6101a860048036038101906101a39190611c7d565b610512565b005b6101c460048036038101906101bf9190611cbd565b610629565b005b6101e060048036038101906101db9190611d10565b610689565b005b6101fc60048036038101906101f79190611cbd565b6106b7565b005b61021860048036038101906102139190611bc8565b6106d7565b6040516102259190611c36565b60405180910390f35b61024860048036038101906102439190611d10565b61075d565b6040516102559190611d4c565b60405180910390f35b610266610814565b005b610270610828565b60405161027d9190611c36565b60405180910390f35b61028e610852565b60405161029b9190611b70565b60405180910390f35b6102be60048036038101906102b99190611d93565b6108e4565b005b6102da60048036038101906102d59190611f08565b6108fa565b005b6102f660048036038101906102f19190611bc8565b61095c565b6040516103039190611b70565b60405180910390f35b61032660048036038101906103219190611f8b565b6109c4565b6040516103339190611ac5565b60405180910390f35b61035660048036038101906103519190611d10565b610a58565b005b60007f80ac58cd000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916148061042357507f5b5e139f000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916145b80610433575061043282610adb565b5b9050919050565b60606000805461044990611ffa565b80601f016020809104026020016040519081016040528092919081815260200182805461047590611ffa565b80156104c25780601f10610497576101008083540402835291602001916104c2565b820191906000526020600020905b8154815290600101906020018083116104a557829003601f168201915b5050505050905090565b60006104d782610b45565b6004600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050919050565b600061051d826106d7565b90508073ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff160361058d576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016105849061209d565b60405180910390fd5b8073ffffffffffffffffffffffffffffffffffffffff166105ac610b90565b73ffffffffffffffffffffffffffffffffffffffff1614806105db57506105da816105d5610b90565b6109c4565b5b61061a576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016106119061212f565b60405180910390fd5b6106248383610b98565b505050565b61063a610634610b90565b82610c51565b610679576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610670906121c1565b60405180910390fd5b610684838383610ce6565b505050565b610691610fdf565b600061069d600761105d565b90506106a9600761106b565b6106b38282611081565b5050565b6106d2838383604051806020016040528060008152506108fa565b505050565b6000806106e38361109f565b9050600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1603610754576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161074b9061222d565b60405180910390fd5b80915050919050565b60008073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff16036107cd576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016107c4906122bf565b60405180910390fd5b600360008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020549050919050565b61081c610fdf565b61082660006110dc565b565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905090565b60606001805461086190611ffa565b80601f016020809104026020016040519081016040528092919081815260200182805461088d90611ffa565b80156108da5780601f106108af576101008083540402835291602001916108da565b820191906000526020600020905b8154815290600101906020018083116108bd57829003601f168201915b5050505050905090565b6108f66108ef610b90565b83836111a2565b5050565b61090b610905610b90565b83610c51565b61094a576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610941906121c1565b60405180910390fd5b6109568484848461130e565b50505050565b606061096782610b45565b600061097161136a565b9050600081511161099157604051806020016040528060008152506109bc565b8061099b84611381565b6040516020016109ac92919061231b565b6040516020818303038152906040525b915050919050565b6000600560008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008373ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060009054906101000a900460ff16905092915050565b610a60610fdf565b600073ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff1603610acf576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610ac6906123b1565b60405180910390fd5b610ad8816110dc565b50565b60007f01ffc9a7000000000000000000000000000000000000000000000000000000007bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916827bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916149050919050565b610b4e8161144f565b610b8d576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610b849061222d565b60405180910390fd5b50565b600033905090565b816004600083815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550808273ffffffffffffffffffffffffffffffffffffffff16610c0b836106d7565b73ffffffffffffffffffffffffffffffffffffffff167f8c5be1e5ebec7d5bd14f71427d1e84f3dd0314c0f7b2291e5b200ac8c7c3b92560405160405180910390a45050565b600080610c5d836106d7565b90508073ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff161480610c9f5750610c9e81856109c4565b5b80610cdd57508373ffffffffffffffffffffffffffffffffffffffff16610cc5846104cc565b73ffffffffffffffffffffffffffffffffffffffff16145b91505092915050565b8273ffffffffffffffffffffffffffffffffffffffff16610d06826106d7565b73ffffffffffffffffffffffffffffffffffffffff1614610d5c576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610d5390612443565b60405180910390fd5b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1603610dcb576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610dc2906124d5565b60405180910390fd5b610dd88383836001611490565b8273ffffffffffffffffffffffffffffffffffffffff16610df8826106d7565b73ffffffffffffffffffffffffffffffffffffffff1614610e4e576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401610e4590612443565b60405180910390fd5b6004600082815260200190815260200160002060006101000a81549073ffffffffffffffffffffffffffffffffffffffff02191690556001600360008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff168152602001908152602001600020600082825403925050819055506001600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282540192505081905550816002600083815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550808273ffffffffffffffffffffffffffffffffffffffff168473ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef60405160405180910390a4610fda8383836001611496565b505050565b610fe7610b90565b73ffffffffffffffffffffffffffffffffffffffff16611005610828565b73ffffffffffffffffffffffffffffffffffffffff161461105b576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161105290612541565b60405180910390fd5b565b600081600001549050919050565b6001816000016000828254019250508190555050565b61109b82826040518060200160405280600081525061149c565b5050565b60006002600083815260200190815260200160002060009054906101000a900473ffffffffffffffffffffffffffffffffffffffff169050919050565b6000600660009054906101000a900473ffffffffffffffffffffffffffffffffffffffff16905081600660006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff1602179055508173ffffffffffffffffffffffffffffffffffffffff168173ffffffffffffffffffffffffffffffffffffffff167f8be0079c531659141344cd1fd0a4f28419497f9722a3daafe3b4186f6b6457e060405160405180910390a35050565b8173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff1603611210576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611207906125ad565b60405180910390fd5b80600560008573ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060006101000a81548160ff0219169083151502179055508173ffffffffffffffffffffffffffffffffffffffff168373ffffffffffffffffffffffffffffffffffffffff167f17307eab39ab6107e8899845ad3d59bd9653f200f220920489ca2b5937696c31836040516113019190611ac5565b60405180910390a3505050565b611319848484610ce6565b611325848484846114f7565b611364576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161135b9061263f565b60405180910390fd5b50505050565b606060405180602001604052806000815250905090565b6060600060016113908461167e565b01905060008167ffffffffffffffff8111156113af576113ae611ddd565b5b6040519080825280601f01601f1916602001820160405280156113e15781602001600182028036833780820191505090505b509050600082602001820190505b600115611444578080600190039150507f3031323334353637383961626364656600000000000000000000000000000000600a86061a8153600a85816114385761143761265f565b5b049450600085036113ef575b819350505050919050565b60008073ffffffffffffffffffffffffffffffffffffffff166114718361109f565b73ffffffffffffffffffffffffffffffffffffffff1614159050919050565b50505050565b50505050565b6114a683836117d1565b6114b360008484846114f7565b6114f2576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016114e99061263f565b60405180910390fd5b505050565b60006115188473ffffffffffffffffffffffffffffffffffffffff166119ee565b15611671578373ffffffffffffffffffffffffffffffffffffffff1663150b7a02611541610b90565b8786866040518563ffffffff1660e01b815260040161156394939291906126e3565b6020604051808303816000875af192505050801561159f57506040513d601f19601f8201168201806040525081019061159c9190612744565b60015b611621573d80600081146115cf576040519150601f19603f3d011682016040523d82523d6000602084013e6115d4565b606091505b506000815103611619576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016116109061263f565b60405180910390fd5b805181602001fd5b63150b7a0260e01b7bffffffffffffffffffffffffffffffffffffffffffffffffffffffff1916817bffffffffffffffffffffffffffffffffffffffffffffffffffffffff191614915050611676565b600190505b949350505050565b600080600090507a184f03e93ff9f4daa797ed6e38ed64bf6a1f01000000000000000083106116dc577a184f03e93ff9f4daa797ed6e38ed64bf6a1f01000000000000000083816116d2576116d161265f565b5b0492506040810190505b6d04ee2d6d415b85acef81000000008310611719576d04ee2d6d415b85acef8100000000838161170f5761170e61265f565b5b0492506020810190505b662386f26fc10000831061174857662386f26fc10000838161173e5761173d61265f565b5b0492506010810190505b6305f5e1008310611771576305f5e10083816117675761176661265f565b5b0492506008810190505b612710831061179657612710838161178c5761178b61265f565b5b0492506004810190505b606483106117b957606483816117af576117ae61265f565b5b0492506002810190505b600a83106117c8576001810190505b80915050919050565b600073ffffffffffffffffffffffffffffffffffffffff168273ffffffffffffffffffffffffffffffffffffffff1603611840576040517f08c379a0000000000000000000000000000000000000000000000000000000008152600401611837906127bd565b60405180910390fd5b6118498161144f565b15611889576040517f08c379a000000000000000000000000000000000000000000000000000000000815260040161188090612829565b60405180910390fd5b611897600083836001611490565b6118a08161144f565b156118e0576040517f08c379a00000000000000000000000000000000000000000000000000000000081526004016118d790612829565b60405180910390fd5b6001600360008473ffffffffffffffffffffffffffffffffffffffff1673ffffffffffffffffffffffffffffffffffffffff16815260200190815260200160002060008282540192505081905550816002600083815260200190815260200160002060006101000a81548173ffffffffffffffffffffffffffffffffffffffff021916908373ffffffffffffffffffffffffffffffffffffffff160217905550808273ffffffffffffffffffffffffffffffffffffffff16600073ffffffffffffffffffffffffffffffffffffffff167fddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef60405160405180910390a46119ea600083836001611496565b5050565b6000808273ffffffffffffffffffffffffffffffffffffffff163b119050919050565b6000604051905090565b600080fd5b600080fd5b60007fffffffff0000000000000000000000000000000000000000000000000000000082169050919050565b611a5a81611a25565b8114611a6557600080fd5b50565b600081359050611a7781611a51565b92915050565b600060208284031215611a9357611a92611a1b565b5b6000611aa184828501611a68565b91505092915050565b60008115159050919050565b611abf81611aaa565b82525050565b6000602082019050611ada6000830184611ab6565b92915050565b600081519050919050565b600082825260208201905092915050565b60005b83811015611b1a578082015181840152602081019050611aff565b60008484015250505050565b6000601f19601f8301169050919050565b6000611b4282611ae0565b611b4c8185611aeb565b9350611b5c818560208601611afc565b611b6581611b26565b840191505092915050565b60006020820190508181036000830152611b8a8184611b37565b905092915050565b6000819050919050565b611ba581611b92565b8114611bb057600080fd5b50565b600081359050611bc281611b9c565b92915050565b600060208284031215611bde57611bdd611a1b565b5b6000611bec84828501611bb3565b91505092915050565b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b6000611c2082611bf5565b9050919050565b611c3081611c15565b82525050565b6000602082019050611c4b6000830184611c27565b92915050565b611c5a81611c15565b8114611c6557600080fd5b50565b600081359050611c7781611c51565b92915050565b60008060408385031215611c9457611c93611a1b565b5b6000611ca285828601611c68565b9250506020611cb385828601611bb3565b9150509250929050565b600080600060608486031215611cd657611cd5611a1b565b5b6000611ce486828701611c68565b9350506020611cf586828701611c68565b9250506040611d0686828701611bb3565b9150509250925092565b600060208284031215611d2657611d25611a1b565b5b6000611d3484828501611c68565b91505092915050565b611d4681611b92565b82525050565b6000602082019050611d616000830184611d3d565b92915050565b611d7081611aaa565b8114611d7b57600080fd5b50565b600081359050611d8d81611d67565b92915050565b60008060408385031215611daa57611da9611a1b565b5b6000611db885828601611c68565b9250506020611dc985828601611d7e565b9150509250929050565b600080fd5b600080fd5b7f4e487b7100000000000000000000000000000000000000000000000000000000600052604160045260246000fd5b611e1582611b26565b810181811067ffffffffffffffff82111715611e3457611e33611ddd565b5b80604052505050565b6000611e47611a11565b9050611e538282611e0c565b919050565b600067ffffffffffffffff821115611e7357611e72611ddd565b5b611e7c82611b26565b9050602081019050919050565b82818337600083830152505050565b6000611eab611ea684611e58565b611e3d565b905082815260208101848484011115611ec757611ec6611dd8565b5b611ed2848285611e89565b509392505050565b600082601f830112611eef57611eee611dd3565b5b8135611eff848260208601611e98565b91505092915050565b60008060008060808587031215611f2257611f21611a1b565b5b6000611f3087828801611c68565b9450506020611f4187828801611c68565b9350506040611f5287828801611bb3565b925050606085013567ffffffffffffffff811115611f7357611f72611a20565b5b611f7f87828801611eda565b91505092959194509250565b60008060408385031215611fa257611fa1611a1b565b5b6000611fb085828601611c68565b9250506020611fc185828601611c68565b9150509250929050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052602260045260246000fd5b6000600282049050600182168061201257607f821691505b60208210810361202557612024611fcb565b5b50919050565b7f4552433732313a20617070726f76616c20746f2063757272656e74206f776e6560008201527f7200000000000000000000000000000000000000000000000000000000000000602082015250565b6000612087602183611aeb565b91506120928261202b565b604082019050919050565b600060208201905081810360008301526120b68161207a565b9050919050565b7f4552433732313a20617070726f76652063616c6c6572206973206e6f7420746f60008201527f6b656e206f776e6572206f7220617070726f76656420666f7220616c6c000000602082015250565b6000612119603d83611aeb565b9150612124826120bd565b604082019050919050565b600060208201905081810360008301526121488161210c565b9050919050565b7f4552433732313a2063616c6c6572206973206e6f7420746f6b656e206f776e6560008201527f72206f7220617070726f76656400000000000000000000000000000000000000602082015250565b60006121ab602d83611aeb565b91506121b68261214f565b604082019050919050565b600060208201905081810360008301526121da8161219e565b9050919050565b7f4552433732313a20696e76616c696420746f6b656e2049440000000000000000600082015250565b6000612217601883611aeb565b9150612222826121e1565b602082019050919050565b600060208201905081810360008301526122468161220a565b9050919050565b7f4552433732313a2061646472657373207a65726f206973206e6f74206120766160008201527f6c6964206f776e65720000000000000000000000000000000000000000000000602082015250565b60006122a9602983611aeb565b91506122b48261224d565b604082019050919050565b600060208201905081810360008301526122d88161229c565b9050919050565b600081905092915050565b60006122f582611ae0565b6122ff81856122df565b935061230f818560208601611afc565b80840191505092915050565b600061232782856122ea565b915061233382846122ea565b91508190509392505050565b7f4f776e61626c653a206e6577206f776e657220697320746865207a65726f206160008201527f6464726573730000000000000000000000000000000000000000000000000000602082015250565b600061239b602683611aeb565b91506123a68261233f565b604082019050919050565b600060208201905081810360008301526123ca8161238e565b9050919050565b7f4552433732313a207472616e736665722066726f6d20696e636f72726563742060008201527f6f776e6572000000000000000000000000000000000000000000000000000000602082015250565b600061242d602583611aeb565b9150612438826123d1565b604082019050919050565b6000602082019050818103600083015261245c81612420565b9050919050565b7f4552433732313a207472616e7366657220746f20746865207a65726f2061646460008201527f7265737300000000000000000000000000000000000000000000000000000000602082015250565b60006124bf602483611aeb565b91506124ca82612463565b604082019050919050565b600060208201905081810360008301526124ee816124b2565b9050919050565b7f4f776e61626c653a2063616c6c6572206973206e6f7420746865206f776e6572600082015250565b600061252b602083611aeb565b9150612536826124f5565b602082019050919050565b6000602082019050818103600083015261255a8161251e565b9050919050565b7f4552433732313a20617070726f766520746f2063616c6c657200000000000000600082015250565b6000612597601983611aeb565b91506125a282612561565b602082019050919050565b600060208201905081810360008301526125c68161258a565b9050919050565b7f4552433732313a207472616e7366657220746f206e6f6e20455243373231526560008201527f63656976657220696d706c656d656e7465720000000000000000000000000000602082015250565b6000612629603283611aeb565b9150612634826125cd565b604082019050919050565b600060208201905081810360008301526126588161261c565b9050919050565b7f4e487b7100000000000000000000000000000000000000000000000000000000600052601260045260246000fd5b600081519050919050565b600082825260208201905092915050565b60006126b58261268e565b6126bf8185612699565b93506126cf818560208601611afc565b6126d881611b26565b840191505092915050565b60006080820190506126f86000830187611c27565b6127056020830186611c27565b6127126040830185611d3d565b818103606083015261272481846126aa565b905095945050505050565b60008151905061273e81611a51565b92915050565b60006020828403121561275a57612759611a1b565b5b60006127688482850161272f565b91505092915050565b7f4552433732313a206d696e7420746f20746865207a65726f2061646472657373600082015250565b60006127a7602083611aeb565b91506127b282612771565b602082019050919050565b600060208201905081810360008301526127d68161279a565b9050919050565b7f4552433732313a20746f6b656e20616c7265616479206d696e74656400000000600082015250565b6000612813601c83611aeb565b915061281e826127dd565b602082019050919050565b6000602082019050818103600083015261284281612806565b905091905056fea264697066735822122033944bfb5e7f7bec6657be72915ae11109b3284fb985e16f7854dff14ce313fa64736f6c63430008120033";
        private const string Nft721ABI = "[{ \"inputs\": [ { \"internalType\": \"address\", \"name\": \"owner\", \"type\": \"address\" } ], \"name\": \"balanceOf\", \"outputs\": [ { \"internalType\": \"uint256\", \"name\": \"\", \"type\": \"uint256\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"name\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"ownerOf\", \"outputs\": [ { \"internalType\": \"address\", \"name\": \"\", \"type\": \"address\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"address\", \"name\": \"to\", \"type\": \"address\" } ], \"name\": \"safeMint\", \"outputs\": [], \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"inputs\": [], \"name\": \"symbol\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }, { \"inputs\": [ { \"internalType\": \"uint256\", \"name\": \"tokenId\", \"type\": \"uint256\" } ], \"name\": \"tokenURI\", \"outputs\": [ { \"internalType\": \"string\", \"name\": \"\", \"type\": \"string\" } ], \"stateMutability\": \"view\", \"type\": \"function\" }]";

        private Web3 firstAccount;
        private string firstWalletAddress;
        private string secondaryWalletAddress;
        private string nft721Address;
        private Process node;

        [OneTimeSetUp]
        public void SetUp()
        {
            node = Emulator.CreateInstance();

            // We shouldn't be relaying on a .Result from ValueTask
            // that is why we need to explicitly wait for it to finish
            var firstAccountTask = Web3Util.CreateWeb3().AsTask();
            firstAccountTask.Wait();
            firstAccount = firstAccountTask.Result;

            var secondAccountTask = Web3Util.CreateWeb3(1).AsTask();
            secondAccountTask.Wait();

            var firstWalletAddressTask = firstAccount.Signer.GetAddress();
            firstWalletAddressTask.Wait();

            firstWalletAddress = firstWalletAddressTask.Result;
            var secondaryWalletAddressTask = secondAccountTask.Result.Signer.GetAddress();
            secondaryWalletAddressTask.Wait();
            secondaryWalletAddress = secondaryWalletAddressTask.Result;

            var amount = new HexBigInteger(1000000);
            var txTask = firstAccount.TransactionExecutor.SendTransaction(new TransactionRequest
            {
                To = secondaryWalletAddress,
                Value = amount,
            });
            txTask.Wait();
            Console.Out.WriteLine("Setup Transaction Hash: {0}", txTask.Result.Hash);

            nft721Address = Web3Util.DeployContracts(firstAccount, Nft721ByteCode, Nft721ABI);
            Console.Out.WriteLine("Smart Contract Address: {0}", nft721Address);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            node?.Kill();
        }

        [Test]
        public void GetNetworkTest()
        {
            var network = firstAccount.RpcProvider.LastKnownNetwork;
            Assert.AreEqual("GoChain Testnet", network.Name);
            Assert.AreEqual(31337, network.ChainId);
        }

        [Test]
        public void GetBalanceTest()
        {
            var balance = firstAccount.RpcProvider.GetBalance(firstWalletAddress).Result;
            var balanceFormatted = Units.FormatEther(balance);
            Assert.Greater(balanceFormatted, "9990");
        }

        [Test]
        public void GetCodeTest()
        {
            var code = firstAccount.RpcProvider.GetCode(firstWalletAddress).Result;
            Assert.AreEqual("0x", code);

            code = firstAccount.RpcProvider.GetCode(nft721Address).Result;
            Assert.AreNotEqual("0x", code);
        }

        [Test]
        public void GetLastBlockTest()
        {
            var latestBlock = firstAccount.RpcProvider.GetBlock().Result;
            Assert.AreEqual("0x0000000000000000", latestBlock.Nonce);
            Assert.AreEqual("30000000", latestBlock.GasLimit.ToString());
            Assert.True(latestBlock.BlockHash.StartsWith("0x"));
            Assert.True(latestBlock.ParentHash.StartsWith("0x"));
            Assert.IsNotEmpty(latestBlock.Number.ToString());
            Assert.IsNotEmpty(latestBlock.Timestamp.ToString());
        }

        [Test]
        public void GetBlockNumberTest()
        {
            var currentBlockNumber = firstAccount.RpcProvider.GetBlockNumber().Result;
            Assert.Greater(currentBlockNumber.ToString(), "0");
        }

        [Test]
        public void GetBlockByNumberTest()
        {
            var currentBlockNumber = firstAccount.RpcProvider.GetBlockNumber().Result;
            var blockByNumber = firstAccount.RpcProvider.GetBlock(new BlockParameter(currentBlockNumber)).Result;
            Assert.AreEqual(blockByNumber.Number, currentBlockNumber);
        }

        [Test]
        public void GetBlockWithTransactionsTest()
        {
            var currentBlockNumber = firstAccount.RpcProvider.GetBlockNumber().Result;
            var blockParameter = new BlockParameter(currentBlockNumber.ToUlong());
            var blockWithTx = firstAccount.RpcProvider.GetBlockWithTransactions(blockParameter).Result;

            var firstTransaction = blockWithTx.Transactions[0];
            Assert.AreEqual(31337, firstTransaction.ChainId.ToUlong());
            Assert.AreEqual(currentBlockNumber, firstTransaction.BlockNumber);
        }

        [Test]
        public void GetPreviousBlockTest()
        {
            var currentBlockNumber = firstAccount.RpcProvider.GetBlockNumber().Result;
            var previousBlockNumber = currentBlockNumber.ToUlong() - 1;
            var previousBlock = firstAccount.RpcProvider.GetBlock(new BlockParameter(previousBlockNumber)).Result;
            Assert.AreEqual(previousBlockNumber, previousBlock.Number.ToUlong());
            Assert.True(previousBlock.BlockHash.StartsWith("0x"));
        }

        [Test]
        public void GetTransactionCountTest()
        {
            var txCount = firstAccount.RpcProvider.GetTransactionCount(firstWalletAddress).Result;
            Assert.Greater(txCount.ToUlong(), 0);
        }

        [Test]
        public void GetFeeDataTest()
        {
            var feeData = firstAccount.RpcProvider.GetFeeData().Result;
            Assert.Greater(Units.FormatUnits(feeData.MaxFeePerGas, "gwei"), "0");
            Assert.Greater(Units.FormatUnits(feeData.GasPrice, "gwei"), "0");
            Assert.Greater(Units.FormatUnits(feeData.MaxPriorityFeePerGas, "gwei"), "0");
        }

        [Test]
        public void GetGasPriceTest()
        {
            var gasPrice = firstAccount.RpcProvider.GetGasPrice().Result;
            var gwei = Units.FormatUnits(gasPrice, "gwei");
            Assert.Greater(gasPrice.ToString(), "0");
            Assert.Greater(gasPrice.ToString(), gwei);
        }

        [Test]
        public void GetTransactionReceiptTest()
        {
            var latestBlockWithTx = firstAccount.RpcProvider.GetBlockWithTransactions().Result;
            var firstTransactionInBlock = latestBlockWithTx.Transactions[0];
            var receipt =
                firstAccount.RpcProvider.GetTransactionReceipt(
                    firstTransactionInBlock.Hash).Result;
            Assert.AreEqual(firstTransactionInBlock.Hash, receipt.TransactionHash);
            Assert.AreEqual(firstTransactionInBlock.BlockHash, receipt.BlockHash);
        }

        [Test]
        public void CallContractMethodTest()
        {
            var address = firstAccount.Signer.GetAddress().Result;
            var contract = firstAccount.ContractBuilder.Build(Nft721ABI, nft721Address);

            var ret = contract.Send("safeMint", new object[] { address }).Result;

            var name = contract.Call("name").Result;
            Assert.AreEqual("Contract", name[0]);

            var symbol = contract.Call("symbol").Result;
            Assert.AreEqual("MNFT", symbol[0]);

            var tokenUri = contract.Call("tokenURI", new object[] { 0 }).Result;
            Assert.AreEqual(string.Empty, tokenUri[0]);

            var ownerOf = contract.Call("ownerOf", new object[] { 0 }).Result;
            StringAssert.AreEqualIgnoringCase(address, ownerOf[0].ToString());

            var balanceOf = contract.Call("balanceOf", new[] { ownerOf[0] }).Result;
            Assert.GreaterOrEqual(balanceOf[0].ToString(), "1");
        }

        [Test]
        public void EstimateGasContractMethodTest()
        {
            var contract = firstAccount.ContractBuilder.Build(Nft721ABI, firstWalletAddress);
            var result = contract.EstimateGas("ownerOf", new object[] { 1 }).Result;
            Assert.AreEqual("21204", result.ToString());
        }

        [Test]
        public void GetAccountsTest()
        {
            var accounts = firstAccount.RpcProvider.ListAccounts().Result;
            Assert.AreEqual(10, accounts?.Length);
            foreach (var account in accounts)
            {
                var accountBalance = firstAccount.RpcProvider.GetBalance(account).Result;
                Assert.GreaterOrEqual(Units.FormatEther(accountBalance), "0");
            }
        }

        [Test]
        public void SendContractTest()
        {
            var contract = firstAccount.ContractBuilder.Build(Nft721ABI, nft721Address);
            var ret = contract.Send("safeMint", new object[] { secondaryWalletAddress }).Result;

            Assert.IsNotNull(ret);
        }

        [Test]
        public void SendContractOverrideGasTest()
        {
            var contract = firstAccount.ContractBuilder.Build(Nft721ABI, nft721Address);
            var ret = contract.Send("safeMint", new object[] { secondaryWalletAddress }, new TransactionRequest
            {
                GasLimit = new HexBigInteger("10000"),
                GasPrice = new HexBigInteger("100000000"),
            }).Result;

            Assert.IsNotNull(ret);
        }
    }
}
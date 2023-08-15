// SPDX-License-Identifier: GPL-3.0

pragma solidity >=0.8.2 <0.9.0;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/token/ERC721/ERC721.sol";
import "@openzeppelin/contracts/utils/Counters.sol";


contract Contract is ERC721, Ownable {
    using Counters for Counters.Counter;
        Counters.Counter private _tokenIdCounter;
        constructor() ERC721("Contract", "MNFT") {}

        function safeMint(address to) public onlyOwner {
            uint256 tokenId = _tokenIdCounter.current();
            _tokenIdCounter.increment();
            _safeMint(to, tokenId);
    }
}

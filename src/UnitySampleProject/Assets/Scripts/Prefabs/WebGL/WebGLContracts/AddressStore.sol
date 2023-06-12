// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

contract AddressStore {
    address[] public bought;

    // set the addresses in store
    function setStore(address[] calldata _addresses) public {
        bought = _addresses;
    }
    function getStore()public view returns( address  [] memory){
    return bought;
    }
}
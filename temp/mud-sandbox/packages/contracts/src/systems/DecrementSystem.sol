// SPDX-License-Identifier: MIT
pragma solidity >=0.8.24;

import {System} from "@latticexyz/world/src/System.sol";
import {Counter} from "../codegen/index.sol";

contract DecrementSystem is System {
    function decrement() public returns (uint32) {
        uint32 counter = Counter.get();
        uint32 newValue = counter - 1;
        Counter.set(newValue);
        return newValue;
    }
}

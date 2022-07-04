// SPDX-License-Identifier: UNLICENSED
pragma solidity >= 0.8.0;

contract CarFactory{ 
    Car[] public Cars;
    function createCar() public {
        Car car = new Car();
        Cars.push(car);
    }
}

contract Car{

}
// SPDX-License-Identifier: UNLICENSED

pragma solidity ^ 0.8.0;

contract Oracle{
    address Owner;
    uint256 public Random;

    constructor() {
        Owner = msg.sender;
    }

    function feedRandomNumbers(uint256 random) external{
        require(msg.sender == Owner);
        Random = random;
    }

}

contract Lottery{
    Oracle oracle;
    address public owner;
    address payable[] public players;
    uint public lotteryId;
    mapping(uint256 => address payable) public lotteryHistory;

    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }

    constructor(address oracleAddress){
        oracle = Oracle(oracleAddress) ;
        owner = msg.sender;
        lotteryId = 1;
    }

    function enter() public payable{
        require(msg.value == 0.01 ether, "Minimum amount to participate is 0.01 ether"); //minimum amount of value
        players.push(payable(msg.sender)); //address of player entering lottery
    }

    
    function getRandomNumber() public view returns(uint256){
        return uint(keccak256(abi.encodePacked(owner, block.timestamp, oracle.Random,block.difficulty)));
    }

    function getBalance() public view returns(uint256){
        return address(this).balance;
    }

    function getPlayers() public view returns(address payable[] memory){
        return players;
    }

    function getWinnerByLottery(uint256 historyLotteryId)public view returns(address payable){
        return  lotteryHistory[historyLotteryId];
    }


    function pickWinner() public onlyOwner{
        uint256 index = getRandomNumber() % players.length;
        players[index].transfer(address(this).balance);

        lotteryHistory[lotteryId] = players[index];
        lotteryId++;


        // reset for the next winner after each lottery end
        players = new address payable[](0);
    }

}

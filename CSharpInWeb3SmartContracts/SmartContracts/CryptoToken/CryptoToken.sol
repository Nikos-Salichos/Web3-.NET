// SPDX-License-Identifier: UNLICENSED
pragma solidity >= 0.8.15;


contract Ownable  {
  address public owner;


  event OwnershipRenounced(address indexed previousOwner);
  event OwnershipTransferred(address indexed previousOwner, address indexed newOwner);


  /**
   * @dev The Ownable constructor sets the original `owner` of the contract to the sender
   * account.
   */
  constructor() {
    owner = msg.sender;
  }

  /**
   * @dev Throws if called by any account other than the owner.
   */
  modifier onlyOwner() {
    require(msg.sender == owner);
    _;
  }

  /**
   * @dev Allows the current owner to transfer control of the contract to a newOwner.
   * @param newOwner The address to transfer ownership to.
   */
  function transferOwnership(address newOwner) public onlyOwner {
    require(newOwner != address(0));
    emit OwnershipTransferred(owner, newOwner);
    owner = newOwner;
  }

  /**
   * @dev Allows the current owner to relinquish control of the contract.
   */
  function renounceOwnership() public onlyOwner {
    emit OwnershipRenounced(owner);
    owner = address(0);
  }
}

contract Authorizable is Ownable {

    mapping(address => bool) public authorized;

    event AuthorizableAddressAdded(address addr);
    event AuthorizableAddressRemoved(address addr);

    modifier onlyAuthorized() {
        require(authorized[msg.sender] || owner == msg.sender);
        _;
    }

    function addAuthorizedAddress(address addr) onlyOwner public returns(bool success) {
        if (!authorized[addr]) {
            authorized[addr] = true;
            emit AuthorizableAddressAdded(addr);
            success = true; 
        }
    }

    function addAuthorizedAddresses(address[] memory addrs) onlyOwner public returns(bool success) {
        for (uint256 i = 0; i < addrs.length; i++) {
            if (addAuthorizedAddress(addrs[i])) {
                success = true;
            }
        }
    }

    function removeAddressFromAuthorized(address addr) onlyOwner public returns(bool success) {
        if (authorized[addr]) {
            authorized[addr] = false;
            emit AuthorizableAddressRemoved(addr);
            success = true;
        }
    }

     function removeAddressesFromWhitelist(address[] memory addrs) onlyOwner public returns(bool success) {
         for (uint256 i = 0; i < addrs.length; i++) {
             if (removeAddressFromAuthorized(addrs[i])) {
             success = true;
                }
         }
     }

}

contract Pausable is Authorizable{
  event Pause();
  event Unpause();
  event NotPausable();

  bool public paused = false;
  bool public canPause = true;

  /**
   * @dev Modifier to make a function callable only when the contract is not paused.
   */
  modifier whenNotPaused() {
    require(!paused || msg.sender == owner);
    _;
  }

  /**
   * @dev Modifier to make a function callable only when the contract is paused.
   */
  modifier whenPaused() {
    require(paused);
    _;
  }

  /**
     * @dev called by the owner to pause, triggers stopped state
     **/
    function pause() onlyOwner whenNotPaused public {
        require(canPause == true);
        paused = true;
        emit Pause();
    }

  /**
   * @dev called by the owner to unpause, returns to normal state
   */
  function unpause() onlyOwner whenPaused public {
    require(paused == true);
    paused = false;
    emit Unpause();
  }
  
  /**
     * @dev Prevent the token from ever being paused again
     **/
    function notPausable() onlyOwner public{
        paused = false;
        canPause = false;
        emit NotPausable();
    }
}

contract CryptoCoin is Pausable{

    uint256 public immutable cap;
    string public name;
    string public symbol;
    uint256 public decimals = 18;
    uint256 public  totalSupply;

    event Transfer(address indexed from, address indexed to, uint256 value);
    event Approval(address indexed owner, address indexed spender, uint256 value);
    event IncreaseApproval(address indexed owner,address indexed spender,uint256 value);
    event DecreaseApproval(address indexed owner,address indexed spender,uint256 value);
    event Sent(address from, address to, uint amount);
    event Burn(address from,uint256 amount);
    event Locked(address indexed owner, uint256 indexed amount);

    mapping(address => uint256) public  balanceOf;
    mapping(address => mapping(address => uint256)) public allowed;
    mapping(address => uint256) locked;

    constructor(uint256 initialSupply, string memory tokenName, string memory tokenSymbol, uint256 tokenCap )   { 
        require(tokenCap > 0, "Token: cap is 0");
        cap = tokenCap*100** uint256(decimals);
        totalSupply =  initialSupply*10** uint256(decimals);
        balanceOf[msg.sender] = totalSupply;
        name = tokenName;
        symbol = tokenSymbol;
        owner = msg.sender;
    }

    modifier onlyTrader(){                   
        require(owner == msg.sender);    
        _;
    }

    function increaseLockedAmount(address _owner, uint256 _amount) onlyOwner public returns (uint256) {
        uint256 lockingAmount = locked[_owner]+_amount;
        require(getBalance(_owner) >= lockingAmount, "Locking amount must not exceed balance");
        locked[_owner] = lockingAmount;
        emit Locked(_owner, lockingAmount);
        return lockingAmount;
    }

    function decreaseLockedAmount(address _owner, uint256 _amount) onlyOwner public returns (uint256) {
        require(locked[_owner] > 0, "Cannot go negative. Already at 0 locked tokens.");
        if (_amount > locked[_owner]) {
            _amount = locked[_owner];
        }
        uint256 lockingAmount = locked[_owner]-_amount;
        locked[_owner] = lockingAmount;
        emit Locked(_owner, lockingAmount);
        return lockingAmount;
    }

    function getLockedAmount(address _owner) view public returns (uint256) {
        return locked[_owner];
    }

    function getUnlockedAmount(address _owner) view public returns (uint256) {
        return balanceOf[_owner]-locked[_owner];
    }

    function getBalance(address tokenOwner) public view returns (uint256) {
        return balanceOf[tokenOwner];
    }

    function mint(address receiver , uint amount)public onlyAuthorized whenNotPaused{
        require(totalSupply + amount <= cap, "Token: cap exceeded");
        balanceOf[receiver] += amount;
        totalSupply += amount;
    }

    function allowance(address owner, address spender)  public whenNotPaused view returns (uint) {
        return allowed[owner][spender];
    }

     function destroySmartContract(address payable _to) public onlyOwner {
        require(msg.sender == owner, "You are not the owner");
        selfdestruct(_to);
    }

    function approve(address spender, uint256 amount) public whenNotPaused  returns (bool) {
        allowed[msg.sender][spender] = amount;
        emit Approval(msg.sender, spender, amount);
        return true;
    }

    function transfer(address receiver, uint256 amount) public whenNotPaused  returns (bool) {
        require(receiver != address(0),"Address is not valid");
        require(amount <= balanceOf[msg.sender] - locked[msg.sender] ,"Funds are not enough" );
        balanceOf[msg.sender] = balanceOf[msg.sender] -amount;
        balanceOf[receiver] = balanceOf[receiver] + amount;
        emit Transfer(msg.sender, receiver, amount);
        return true;
    }

    function increaseApproval(address spender, uint256 addedAmount)public whenNotPaused  returns(bool success){
        allowed[msg.sender][spender] = addedAmount;
        emit IncreaseApproval(msg.sender, spender, addedAmount);
        return true;
    }

    function decreaseApproval(address spender, uint subtractedAmount) public whenNotPaused returns (bool) {
        uint256 oldAmount = allowed[msg.sender][spender];
        
        if (subtractedAmount > oldAmount) {
            allowed[msg.sender][spender] = 0;
        } 
        else 
        {
            allowed[msg.sender][spender] = oldAmount - subtractedAmount;
        }

        emit DecreaseApproval(msg.sender, spender, subtractedAmount);

        return true;
    }


    function transferFrom(address from, address to, uint256 value) public  whenNotPaused returns (bool success){
        require(to != address(0),"Address is not valid");
        require(value <= balanceOf[from] - locked[from],"Funds are not enough" );
        require(value <= allowed[from][msg.sender] - locked[from],"There is no approval" );
        balanceOf[from] -=value;
        balanceOf[to] +=value;
        allowed[from][msg.sender] -= value;
        return true;
    }

    function burn(uint256 amount) public onlyAuthorized whenNotPaused returns(bool success){
        require(balanceOf[msg.sender] >= amount);
        balanceOf[msg.sender] -= amount;
        totalSupply -= amount;
        emit Burn(msg.sender,amount);
        return true;
    }

    function burnFrom (address from, uint256 amount) public onlyAuthorized whenNotPaused returns (bool success){
        require(amount <= allowed[from][msg.sender]);
        balanceOf[from] -=amount;
        allowed[from][msg.sender]  -=amount;
        totalSupply -= amount;
        emit Burn(from, amount);
        return true;
    }


}

contract CryptoTokenSale{

    address admin;
    CryptoCoin public TokenContract;
    uint256 public TokenPrice;
    uint256 public TotalTokensSold;

    event Sell(address buyer, uint256 amount);

    constructor( CryptoCoin tokenContract, uint256 tokenPrice){
        admin = msg.sender;
        TokenContract = tokenContract;
        TokenPrice = tokenPrice;
    }


    //Prepei na metaferw tokens apo to arxiko contract se auto edw gia na doulepsei to buyTokens
    function buyTokens(uint256 numberOfTokens)public payable{
        require(msg.value == numberOfTokens * TokenPrice , "msg.value must be equal number of tokens in wei");
        require(TokenContract.balanceOf(address(this)) >= numberOfTokens , "Cannot purchase more tokens than available");
        require(TokenContract.transfer(msg.sender,  numberOfTokens));

        TotalTokensSold += numberOfTokens;
        emit Sell(msg.sender, numberOfTokens);
    }

    function endSale() public {
        require(msg.sender == admin);
        selfdestruct(payable(admin));
    }
}


// Πρέπει στο αρχικό token με την διεύθυνση του owner να κάνω increase approval και να βάλω spender address την
// διεύθυνση του contract του airdrop token και added το amount που θέλω να στείλω
// Την function airdropToken πρέπει να την εκτελεί ο owner του contract


contract Airdrop {
    
    address owner;
    
    constructor() {
        owner = msg.sender;
    }
    
    receive() external payable{}
    
    function airdropToken(address _tokenAddress, address[] memory _recipients, uint256 _amount) public {
        require(CryptoCoin(_tokenAddress).allowance(msg.sender,address(this))>0, "contract is not allowed to spend that token");
        //In tokenAddress you need to approve airdrop address
        for (uint i=0;i<_recipients.length;i++){
           CryptoCoin(_tokenAddress).transferFrom(msg.sender, _recipients[i], _amount);
        }
    }
}

contract StoryDao is Ownable {

    mapping(address => bool) whitelist;
    uint256 public whitelistedNumber = 0;
    mapping(address => bool) blacklist;
    event Whitelisted(address addr, bool status);
    event Blacklisted(address addr, bool status);

    uint256 public daofee = 100; // hundreds of a percent, 100 is 1%
    uint256 public whitelistfee =  0.0001 ether; 
    uint256 public submissionZeroFee = 0.0001 ether;

    event SubmissionCommissionChanged(uint256 newFee);
    event SubmissionFeeChanged(uint256 newFee);
    event WhitelistFeeChanged(uint256 newFee);

    uint256 public durationDays = 21; // duration of story's chapter in days
    uint256 public durationSubmissions = 1000; // duration of story's chapter in entries

    function changeDaoFee(uint256 _fee) onlyOwner external {
        require(_fee < daofee, "New fee must be lower than old fee.");
        daofee = _fee;
        emit SubmissionCommissionChanged(_fee);
    }

    function changeWhitelistFee(uint256 _fee) onlyOwner external {
        require(_fee < whitelistfee, "New fee must be lower than old fee.");
        whitelistfee = _fee;
        emit WhitelistFeeChanged(_fee);
    }

    function lowerSubmissionFee(uint256 _fee) onlyOwner external {
        require(_fee < submissionZeroFee, "New fee must be lower than old fee.");
        submissionZeroFee = _fee;
        emit SubmissionFeeChanged(_fee);
    }

    function changeDurationDays(uint256 _days) onlyOwner external {
        require(_days >= 1);
        durationDays = _days;
    }

    function changeDurationSubmissions(uint256 _subs) onlyOwner external {
        require(_subs > 99);
        durationSubmissions = _subs;
    }
}

contract Staking {

    CryptoCoin public yourToken;

    address public owner;

    address public sender;

    struct Position{
        uint positionId;
        address walletAddress;
        uint createdDate;
        uint unlockDate;
        uint percentInterest;
        uint weiStaked;
        uint weiInterest;
        bool open;
    }

    Position position;

    uint public currentPositionId;
    mapping(uint => Position) positions;
    mapping(address => uint[]) public positionIdsByAddress;
    mapping(uint => uint) public stakingTiers;
    uint[] public lockPeriods;

    constructor(address tokenAddress) payable{
        require(tokenAddress != address(0), "Token address cannot be null-address");
        owner = msg.sender;
        currentPositionId - 0;

        stakingTiers[30] = 700; // 7%APY
        stakingTiers[90] = 1000; //10% APY
        stakingTiers[180] = 2000; //20% APY

        lockPeriods.push(30);
        lockPeriods.push(90);
        lockPeriods.push(180);

        yourToken = CryptoCoin(tokenAddress);
    }

    function stakeToken(uint numberOfDays, uint amount) external {
        require(stakingTiers[numberOfDays] > 0, "Staking tier of these days did not found");

        yourToken.transferFrom(msg.sender,address(this), amount); 

        positions[currentPositionId] = Position(
            currentPositionId,
            msg.sender,
            block.timestamp,
            block.timestamp + (numberOfDays * 1 days),
            stakingTiers[numberOfDays],
            amount,
            calculateInterest(stakingTiers[numberOfDays], amount),
            true
        );

        positionIdsByAddress[msg.sender].push(currentPositionId);
        currentPositionId += 1;
    }

    function calculateInterest(uint basisPoints, uint weiAmount) private pure returns(uint){
        return basisPoints / 10000 * weiAmount;
    }

    function modifyLockPeriods(uint numberOfDays, uint basisPoints) external{
        require(owner == msg.sender, "Only owner may modify staking periods");

        stakingTiers[numberOfDays] = basisPoints;
        lockPeriods.push(numberOfDays);
    }

    function getLockPeriods() external view returns(uint[] memory){
        return lockPeriods;
    }

    function getInterestRate(uint numberOfDays) external view returns(uint){
        return stakingTiers[numberOfDays];
    }

    function getPositionById(uint positionId) external view returns(Position memory){
        return positions[positionId];
    }

    function getPositionIdsForAddress(address walletAddress) external view returns(uint[] memory){
        return positionIdsByAddress[walletAddress];
    }

    function changeUnlockDate(uint positionId, uint newUnlockDate) external{
        require(owner == msg.sender, "Only owner may modify staking periods");
        positions[positionId].unlockDate = newUnlockDate;
    }

    //Should add stakingAddress in Authorizable
    function closePosition(uint positionId) external{
        require(positions[positionId].walletAddress == msg.sender,"Only position creator may modify position");
        require(positions[positionId].open == true,"Position is closed");

        positions[positionId].open = false;

        if (block.timestamp > positions[positionId].unlockDate){
            uint amount = positions[positionId].weiStaked;
            yourToken.transfer(msg.sender,positions[positionId].weiStaked);
            yourToken.mint(msg.sender,amount);
        }else{
            uint amount = positions[positionId].weiStaked/2;
            yourToken.transfer(msg.sender,amount);
            yourToken.transfer(address(0x000000000000000000000000000000000000dEaD),amount);
        }
    }

      receive() external payable{}
}


contract CryptoCoinFactory{ 

    CryptoCoin[] public CryptoCoins;

    function createCryptoCoin(uint256 initialSupply, string memory tokenName, string memory tokenSymbol, uint256 tokenCap) public {
        CryptoCoin cryptoCoin = new CryptoCoin(initialSupply,tokenName,tokenSymbol,tokenCap);
        CryptoCoins.push(cryptoCoin);
    }

    function getAllCryptoCoins() public pure returns (CryptoCoin[] memory allCryptoCoins)
    {
        return allCryptoCoins;
    }
}


contract CloneFactory {

  function createClone(address target) internal returns (address result) {
    bytes20 targetBytes = bytes20(target);
    assembly {
      let clone := mload(0x40)
      mstore(clone, 0x3d602d80600a3d3981f3363d3d373d3d3d363d73000000000000000000000000)
      mstore(add(clone, 0x14), targetBytes)
      mstore(add(clone, 0x28), 0x5af43d82803e903d91602b57fd5bf30000000000000000000000000000000000)
      result := create(0, clone, 0x37)
    }
  }

  function isClone(address target, address query) internal view returns (bool result) {
    bytes20 targetBytes = bytes20(target);
    assembly {
      let clone := mload(0x40)
      mstore(clone, 0x363d3d373d3d3d363d7300000000000000000000000000000000000000000000)
      mstore(add(clone, 0xa), targetBytes)
      mstore(add(clone, 0x1e), 0x5af43d82803e903d91602b57fd5bf30000000000000000000000000000000000)

      let other := add(clone, 0x40)
      extcodecopy(query, other, 0, 0x2d)
      result := and(
        eq(mload(clone), mload(other)),
        eq(mload(add(clone, 0xd)), mload(add(other, 0xd)))
      )
    }
  }
}

contract CryptoCoinFactoryClone is Ownable, CloneFactory {

  address public libraryAddress;

  event CryptoCoinCreated(address newFoundation);

  function CryptoCoinFactoryWithLibrary(address _libraryAddress) public {
    libraryAddress = _libraryAddress;
  }
}
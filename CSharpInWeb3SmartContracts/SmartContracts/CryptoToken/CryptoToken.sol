//SPDX-License-Identifier: Unlicensed
pragma solidity >=0.8.0;


contract Ownable {
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

contract Pausable is Ownable{
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

interface IERC20 {

    function totalSupply() external view returns (uint256);
    function balanceOf(address account) external view returns (uint256);
    function allowance(address owner, address spender) external view returns (uint256);

    function transfer(address recipient, uint256 amount) external returns (bool);
    function approve(address spender, uint256 amount) external returns (bool);
    function transferFrom(address sender, address recipient, uint256 amount) external returns (bool);

    event Transfer(address indexed from, address indexed to, uint256 value);
    event Approval(address indexed owner, address indexed spender, uint256 value);
}

 contract CryptoToken is IERC20,Pausable{

    uint256 public immutable cap;
    string public name;
    string public symbol;
    uint256 public decimals = 18;
    uint256 public override totalSupply;

    event IncreaseApproval(address indexed owner,address indexed spender,uint256 value);
    event DecreaseApproval(address indexed owner,address indexed spender,uint256 value);
    event Sent(address from, address to, uint amount);
    event Burn(address from,uint256 amount);

    mapping(address => uint256) public override balanceOf;
    mapping(address => mapping(address => uint256)) public allowed;

    constructor(uint256 initialSupply, string memory tokenName, string memory tokenSymbol, uint256 tokenCap )   { 
        require(tokenCap > 0, "Token: cap is 0");
        cap = tokenCap*100** uint256(decimals);
        totalSupply =  initialSupply*10** uint256(decimals);
        balanceOf[msg.sender] = totalSupply;
        name = tokenName;
        symbol = tokenSymbol;
        owner = msg.sender;
    }


    function getBalance(address tokenOwner) public view returns (uint256) {
        return balanceOf[tokenOwner];
    }

    function mint(address receiver , uint amount)public onlyOwner whenNotPaused{
        require(msg.sender == owner);
        require(totalSupply + amount <= cap, "Token: cap exceeded");
        balanceOf[receiver] += amount;
        totalSupply += amount;
    }

    function allowance(address owner, address delegate) override public whenNotPaused view returns (uint) {
        return allowed[owner][delegate];
    }

     function destroySmartContract(address payable _to) public onlyOwner {
        require(msg.sender == owner, "You are not the owner");
        selfdestruct(_to);
    }

    function approve(address delegate, uint256 amount) public whenNotPaused override returns (bool) {
        allowed[msg.sender][delegate] = amount;
        emit Approval(msg.sender, delegate, amount);
        return true;
    }

    function transfer(address receiver, uint256 amount) public whenNotPaused override returns (bool) {
        require(amount <= balanceOf[msg.sender]);
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


    function transferFrom(address from , address to, uint256 value) public override whenNotPaused returns (bool success){
        require(value <= balanceOf[from]); 
        require(value <= allowed[from][msg.sender]);
        balanceOf[from] -=value;
        balanceOf[to] +=value;
        allowed[from][msg.sender] -= value;
        return true;
    }

    function burn(uint256 amount) public onlyOwner whenNotPaused returns(bool success){
        require(balanceOf[msg.sender] >= amount);
        balanceOf[msg.sender] -= amount;
        totalSupply -= amount;
        emit Burn(msg.sender,amount);
        return true;
    }

    function burnFrom (address from, uint256 amount) public onlyOwner whenNotPaused returns (bool success){
        require(msg.sender == owner);
        require(amount <= allowed[from][msg.sender]);
        balanceOf[from] -=amount;
        allowed[from][msg.sender]  -=amount;
        totalSupply -= amount;
        emit Burn(from, amount);
        return true;
    }


}
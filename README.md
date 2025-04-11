
# Welcome to DracoArts

![Logo](https://dracoarts-logo.s3.eu-north-1.amazonaws.com/DracoArts.png)




#  Ethereum Wallet Integration for Unity
### A Nethereum-based solution for checking ETH balances and transferring tokens in Unity
This Unity project demonstrates how to integrate Ethereum wallet functionalities using Nethereum, allowing users to:
- ✅ Check ETH balance on the Sepolia testnet.
- ✅ Transfer ETH to another wallet address.
- ✅ View transaction details (hash, status).

Built with C# and Nethereum, this system connects to Infura as the Ethereum node provider and supports Sepolia testnet transactions.

⚙️ Features
-
 ## 1. WalletBalance
- Check ETH Balance: Fetches and displays the wallet's ETH balance from the blockchain.

- Auto-Refresh: Updates the balance when the wallet address changes.

- Error Handling: Shows network errors and missing wallet states.

## 2.  TransferToken
- Send ETH: Transfers a specified amount of ETH to a recipient address.

- Transaction Tracking: Displays transaction hash and status.

- Copy Hash: Allows users to copy the transaction hash for verification.

# 🚀 Setup Instructions

## 1. Prerequisites

- Unity (2021.3 or later)

- Nethereum (Install via NuGet or Unity Package Manager)

- Infura API Key (Free account at [infura.io](https://www.infura.io/))
## 2. Configuration
### 1. Replace Infura Endpoint:

- Update the Infura URL in both scripts


      var web3 = new Web3("https://sepolia.infura.io/v3/YOUR_INFURA_API_KEY");

###  2.   Wallet Integration:

 - Ensure WalletManager is properly set up to handle wallet connections (e.g., MetaMask, WalletConnect).

### 3 UI Setup:

- Attach the scripts to Unity UI panels.

- Link buttons (checkBalanceButton, TransferButton) and text fields (balanceText, statusText).

## 🔄 How It Works

### Checking Balance
- User clicks "Check Balance".

- Fetches the wallet address from PlayerPrefs.

- Connects to Sepolia via Infura.

- Retrieves the balance in ETH (converted from Wei).

- Updates the UI with the balance or an error message.

### Transferring ETH
- User enters:

- Recipient Address

- Amount (ETH)

 - Validates inputs.

- Sends the transaction via the connected wallet.

 - Displays the transaction hash upon success.

 - Allows copying the hash for blockchain explorers like Etherscan.

 ## ⚠️ Important Notes
- Testnet Only: Uses Sepolia ETH (get free test ETH from a faucet).

- Security: Never hardcode private keys—use wallet providers like MetaMask.
## Usage/Examples
  CheckBalance

      public async Task CheckBalance()
    {
     

        try
        {
            // Reset UI state
            balanceText.text = "";
            statusText.text = "Fetching balance...";
            
            WalletAddress = PlayerPrefs.GetString("WalletAddress");
            if(string.IsNullOrEmpty(WalletAddress))
            {
                statusText.text = "No wallet address found";
                return;
            }

            var web3 = new Web3("https://sepolia.infura.io/v3/3e4f6c287aac409abbd04ebe4e832182");
            var balance = await web3.Eth.GetBalance.SendRequestAsync(WalletAddress);
            decimal etherAmount = Web3.Convert.FromWei(balance);
            
            // Update UI together
            balanceText.text = $"{etherAmount} ETH";
            statusText.text = "Balance updated";
            
           
        }
        catch (Exception e)
        {
            // Set both texts together for error state
            statusText.text = "Network error";
            balanceText.text = "Unavailable";
            Debug.LogError(e);
        
        }
    
    }

Transfer Token

     public async void SendTransaction(string toAddress, decimal amount)
    {
        if (walletManager._currentAccount == null)
        {
            statusText.text = "No wallet available";
            return;
        }

        try
        {
            var web3 = new Web3(walletManager._currentAccount, "https://sepolia.infura.io/v3/3e4f6c287aac409abbd04ebe4e832182");
            var transaction = await web3.Eth.GetEtherTransferService()
                .TransferEtherAndWaitForReceiptAsync(toAddress, amount);

            statusText.text = "Transaction complete!";
            TransctionHashText.text = $"Hash: {transaction.TransactionHash}";
            transctionLoading.SetActive(false);
            copyHashButton.interactable=true;
            // clear amount 
            amountInput.text="";

        }
        catch (Exception e)
        {
            statusText.text = $"Transaction failed: {e.Message}";
            Debug.LogError(e);
        }
    }
## Image

## Check Wallet Balance
![](https://github.com/AzharKhemta/Gif-File-images/blob/main/Check%20Balance%20Nethereum.gif?raw=true)

## Transcation Token
![](https://github.com/AzharKhemta/Gif-File-images/blob/main/Nethereum%20Transcation%20Token.gif?raw=true)


## Authors

- [@MirHamzaHasan](https://github.com/MirHamzaHasan)
- [@WebSite](https://mirhamzahasan.com)


## 🔗 Links

[![linkedin](https://img.shields.io/badge/linkedin-0A66C2?style=for-the-badge&logo=linkedin&logoColor=white)](https://www.linkedin.com/company/mir-hamza-hasan/posts/?feedView=all/)
## Documentation

[Nethereum Documentation:](https://docs.nethereum.com/en/latest/unity3d-introduction/)

[Infura API Dashboard](https://www.infura.io/)

[Sepolia Etherscan](https://sepolia.etherscan.io/)


## Tech Stack
**Client:** Unity,C#

**Plugin:** Nethereum




using System;
using System.Threading.Tasks;
using Nethereum.Web3;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WalletBalance : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject walletDisplayPanel;
    public GameObject checkBalancePanel;
    
    [Header("UI Element")]
    public Button checkBalanceButton;
    public Button closeButton;
    public Text balanceText;
    public Text statusText;

    private string WalletAddress;
    private bool isCheckingBalance = false;

    void Start()
    {
        checkBalanceButton.onClick.AddListener(RefreshBalance);
    }

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

    public async void RefreshBalance()
    {
        walletDisplayPanel.SetActive(false);
        checkBalancePanel.SetActive(true);
        await CheckBalance();
    }

    // Call this when wallet address changes
    public void WalletPanel()
    {

        // Reset UI when address changes
        walletDisplayPanel.SetActive(true);
        checkBalancePanel.SetActive(false);
        statusText.text = "Wallet";
        balanceText.text = "";
    }
}
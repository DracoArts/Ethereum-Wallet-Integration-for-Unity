using System;
using System.Collections;
using System.Collections.Generic;
using Nethereum.Model;
using Nethereum.Web3;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
public class TransferToken : MonoBehaviour
{


    [Header("UI Panel")]
    public GameObject transferTokenPanel;
    public GameObject walletDisplayPanel;
    public GameObject transctionLoading;

    [Header("UI Element")]
    public InputField addressInput;
    public InputField amountInput;
    public Text TransctionHashText;
    public Text statusText;
    public Button TransferButton;
    public Button  closeButton;
    public Button copyHashButton;
    [SerializeField]
    private WalletManager walletManager;
    void Start()
    {
          TransferButton.onClick.AddListener(OnSendButtonClicked);
          closeButton .onClick.AddListener(WalletPanel);
          copyHashButton.onClick.AddListener(CopyPhraseToClipboard);
          copyHashButton.interactable=false;
    }
    public void OnSendButtonClicked()
    {
        if (!decimal.TryParse(amountInput.text, out decimal amount))
        {
            statusText.text = "Invalid amount";
            return;
        }

        SendTransaction(addressInput.text, amount);
        transctionLoading.SetActive(true);
    }

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
   
    public void WalletPanel(){
      transferTokenPanel.SetActive(false);
      walletDisplayPanel.SetActive(true);

    }

    public void CopyPhraseToClipboard()
    {
        if (!string.IsNullOrEmpty(TransctionHashText.text))
        {
            GUIUtility.systemCopyBuffer = TransctionHashText.text;
            statusText.text = "Hash copied to clipboard!";
        }
    }
}

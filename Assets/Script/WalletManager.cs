
using UnityEngine;
using UnityEngine.UI;
using System;
using Nethereum.Web3.Accounts;
using Nethereum.Web3;
using NBitcoin;
using Nethereum.HdWallet;
using System.Threading.Tasks;

public class WalletManager : MonoBehaviour
{
    [Header("UI Panel ")]
    public GameObject walletCreationPanel;
    public GameObject walletImportPanel;
    public GameObject walletDisplayPanel;
    public GameObject newWalletCreatePanel;
    public GameObject walletTransctionPanel;
    public GameObject checkBalancePanel;
    
    [Header("UI Text ")]
    public InputField importPrivateKeyInput;
    public InputField currentWalletDisplay;
    public InputField PriavteKeyDisplay;


    public Text statusText;
    public Text phraseDisplay;

    [Header("UI Button")]
    public Button createNewButton;
    public Button importExistingButton;
    public Button confirmImportButton;
    public Button copyPhraseButton;
    public Button clearWalletButton;

    public Account _currentAccount;
    private string _currentPhrase;
    private string WalletAddress;

    void Start()
    {
        // Initialize UI
        walletCreationPanel.SetActive(true);
        walletImportPanel.SetActive(false);
        walletDisplayPanel.SetActive(false);
        walletTransctionPanel.SetActive(false);
        checkBalancePanel.SetActive(false);


        createNewButton.onClick.AddListener(ShowWalletCreation);
        importExistingButton.onClick.AddListener(ShowWalletImport);
        confirmImportButton.onClick.AddListener(ImportWalletFromPrivateKey);
        copyPhraseButton.onClick.AddListener(CopyPhraseToClipboard);
        clearWalletButton.onClick.AddListener(ClearCurrentWallet);

        // Check if wallet exists in player prefs (for demo only - not secure for production)
        CheckForExistingWallet();
    }

    private void CheckForExistingWallet()
    {
        // In a real game, use proper secure storage instead of PlayerPrefs
        if (PlayerPrefs.HasKey("WalletAddress"))
        {
             WalletAddress  = PlayerPrefs.GetString("WalletAddress");
            string privateKey = ""; // In real implementation, get from secure storage

            walletCreationPanel.SetActive(false);
            walletDisplayPanel.SetActive(true);
            newWalletCreatePanel.SetActive(false);
            currentWalletDisplay.text = $"Existing Wallet Found!\nAddress: {WalletAddress}";
            statusText.text = "Loaded existing wallet";
        }
    }

    private void ShowWalletCreation()
    {
        walletImportPanel.SetActive(false);
        GenerateNewWallet();
    }

    private void ShowWalletImport()
    {
        walletCreationPanel.SetActive(false);
        walletImportPanel.SetActive(true);
        statusText.text = "Enter your private key";
    }

    public void GenerateNewWallet()
    {
        try
        {
            
            _currentPhrase = GenerateMnemonic(12);
            var wallet = new Wallet(_currentPhrase, null);
            _currentAccount = wallet.GetAccount(0);

            phraseDisplay.text = _currentPhrase;
            currentWalletDisplay.text = $"Address: {_currentAccount.Address}";
            PriavteKeyDisplay.text = $"Private Key: {MaskPrivateKey(_currentAccount.PrivateKey)}";
            Debug.Log(_currentAccount.PrivateKey);



            // Save wallet (insecure demo only - use proper secure storage in production)
            PlayerPrefs.SetString("WalletAddress", _currentAccount.Address);
            // Never save private key or phrase in PlayerPrefs in real applications!

            walletCreationPanel.SetActive(false);
            walletDisplayPanel.SetActive(true);
            newWalletCreatePanel.SetActive(true);
            statusText.text = "New wallet created successfully!";
        }
        catch (Exception e)
        {
            statusText.text = $"Error: {e.Message}";
            Debug.LogError(e);
        }
    }

    public void ImportWalletFromPrivateKey()
    {
        string privateKey = importPrivateKeyInput.text.Trim();

        if (string.IsNullOrEmpty(privateKey))
        {
            statusText.text = "Please enter a private key";
            return;
        }

        try
        {
            // Create account from private key
            _currentAccount = new Account(privateKey);
            _currentPhrase = null; // No phrase for imported wallets

            // Display wallet
            currentWalletDisplay.text = $"Wallet Imported!\nAddress: {_currentAccount.Address}";

            // Save wallet (insecure demo only)
            PlayerPrefs.SetString("WalletAddress", _currentAccount.Address);

            walletImportPanel.SetActive(false);
            walletDisplayPanel.SetActive(true);
            newWalletCreatePanel.SetActive(false);
            statusText.text = "Wallet imported successfully!";
        }
        catch (Exception e)
        {
            statusText.text = $"Invalid private key: {e.Message}";
            Debug.LogError(e);
        }
    }

    private string GenerateMnemonic(int wordCount)
    {
        var mnemonic = new Mnemonic(Wordlist.English, wordCount == 12 ? WordCount.Twelve : WordCount.TwentyFour);
        return mnemonic.ToString();
    }

    private string MaskPrivateKey(string privateKey)
    {
        if (privateKey.Length < 10) return privateKey;
        return privateKey.Substring(0, 8) + "..." + privateKey.Substring(privateKey.Length - 4);
    }

    public void CopyPhraseToClipboard()
    {
        if (!string.IsNullOrEmpty(_currentPhrase))
        {
            GUIUtility.systemCopyBuffer = _currentPhrase;
            statusText.text = "Recovery phrase copied to clipboard!";
        }
    }

    public void ClearCurrentWallet()
    {
        _currentAccount = null;
        _currentPhrase = null;

        // Clear saved data (demo only)
        PlayerPrefs.DeleteKey("WalletAddress");

        walletDisplayPanel.SetActive(false);
        walletCreationPanel.SetActive(true);
        statusText.text = "Wallet cleared. Create or import a wallet.";
    }
  
}
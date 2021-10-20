/*
 * Pixel Framework
 * 
 * This framework allows you to develop games for mobile devices even faster than you normally do.
 * It includes a lot of useful classes, resources, and examples to get your project started.
 * The framework will be especially useful for developers of hyper-casual games.
 *
 * This framework was developed specifically for Pixel Incubator members with the support of TinyPlay.
 * You can read more about Incubator at the official website:
 * https://pixinc.club/
 *
 * @developer       Ilya Rastorguev
 * @autor           TinyPlay, Inc.
 * @version         1.0
 * @url             https://pixinc.club/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */
namespace PixelFramework.Managers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Purchasing;
    using UnityEngine.Purchasing.Security;
    using PixelFramework.Core.ContentManagement;
    
    /// <summary>
    /// In-App Purchases Manager
    /// </summary>
    public class IAPManager : IGameManager, IStoreListener
    {
        // IAP Events
        public UnityEvent<IAPManagerConfigs> OnAudioSettingsChanged = new UnityEvent<IAPManagerConfigs>();
        public UnityEvent OnIAPReady = new UnityEvent();
        public UnityEvent<InitializationFailureReason> OnIAPInitializationError =
            new UnityEvent<InitializationFailureReason>();

        // Callbacks
        private Dictionary<string, Action<string, DateTime, string>> PurchaseCompleteCallbacks =
            new Dictionary<string, Action<string, DateTime, string>>();
        private Dictionary<string, Action<string, string>> PurchaseErrorCallbacks =
            new Dictionary<string, Action<string, string>>();
        
        // Private Params
        private static IAPManager _instance;
        private IAPManagerConfigs _config = new IAPManagerConfigs();
        
        // Unity IAP
        private bool _iapInitialized = false;
        private bool _iapReady = false;
        private IStoreController _controller;
        private IExtensionProvider _extensions;
        
        #region Base Manager Logic
        /// <summary>
        /// IAP Manager Constructor
        /// </summary>
        /// <param name="config"></param>
        private IAPManager(IAPManagerConfigs config = null)
        {
            if (config != null) _config = config;
            
            if (!_iapInitialized)
            {
                _iapInitialized = true;
                ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
                foreach (IAPProduct product in _config.Products)
                {
                    builder.AddProduct(product.productID, product.productType, product.productIDList);
                }
                
                
                UnityPurchasing.Initialize (this, builder);
            }
        }
        
        /// <summary>
        /// Get IAP Manager Instance
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IAPManager Instance(IAPManagerConfigs config = null)
        {
            if (_instance == null)
                _instance = new IAPManager(config);
            return _instance;
        }
        
        /// <summary>
        /// Set Current IAP Manager State
        /// </summary>
        /// <param name="config"></param>
        public IAPManager SetState(IAPManagerConfigs config)
        {
            _config = config;
            return _instance;
        }

        /// <summary>
        /// Get Current IAP Manager State
        /// </summary>
        /// <returns></returns>
        public IAPManagerConfigs GetCurrentState()
        {
            return _config;
        }
        
        /// <summary>
        /// Load Manager State
        /// </summary>
        public void LoadState()
        {
            string path = "/iap_settings.dat";
            _config = FileReader.ReadObjectFromFile<IAPManagerConfigs>(path, SerializationType.EncryptedJSON);
        }

        /// <summary>
        /// Save Manager State
        /// </summary>
        public void SaveState()
        {
            string path = "/iap_settings.dat";
            FileReader.SaveObjectToFile(path, _config, SerializationType.EncryptedJSON);
            if(OnAudioSettingsChanged!=null) OnAudioSettingsChanged.Invoke(_config);
        }
        #endregion
        
        #region IAP Manager Logic
        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="extensions"></param>
        public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
        {
            this._controller = controller;
            this._extensions = extensions;
            _iapReady = true;
            if(OnIAPReady!=null) OnIAPReady.Invoke();
        }
        
        /// <summary>
        /// Check if Unity IAP is Ready
        /// </summary>
        /// <returns></returns>
        public bool IsIAPReady()
        {
            return _iapReady;
        }
        
        /// <summary>
        /// On IAP Initialization Error
        /// </summary>
        /// <param name="error"></param>
        public void OnInitializeFailed (InitializationFailureReason error)
        {
            _iapReady = false;
            if(OnIAPInitializationError!=null) OnIAPInitializationError.Invoke(error);
        }
        
        /// <summary>
        /// Purchase Product
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="OnComplete"></param>
        /// <param name="OnError"></param>
        public void PurchaseProduct(string productID, Action<string, DateTime, string> OnComplete = null, Action<string, string> OnError = null)
        {
            _controller.InitiatePurchase(productID);
            if(OnComplete!=null) PurchaseCompleteCallbacks.Add(productID, OnComplete);
            if(OnError!=null) PurchaseErrorCallbacks.Add(productID, OnError);
        }
        
        /// <summary>
        /// Purchase Product with Payload
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="payload"></param>
        /// <param name="OnComplete"></param>
        /// <param name="OnError"></param>
        public void PurchaseProduct(string productID, string payload, Action<string, DateTime, string> OnComplete = null, Action<string, string> OnError = null)
        {
            _controller.InitiatePurchase(productID, payload);
            if(OnComplete!=null) PurchaseCompleteCallbacks.Add(productID, OnComplete);
            if(OnError!=null) PurchaseErrorCallbacks.Add(productID, OnError);
        }
        
        /// <summary>
        /// Process Purchase
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public PurchaseProcessingResult ProcessPurchase (PurchaseEventArgs e)
        {
            bool validPurchase = true;
            string productID = e.purchasedProduct.definition.id;
            DateTime purchaseTime = DateTime.Now;
            string transactionID = "";
            
            // Validation only for Android, iOS and OSX
            if (_config.PaymentVerification)
            {
                #if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
                CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                    AppleTangle.Data(), Application.identifier);
                try {
                    IPurchaseReceipt[] result = validator.Validate(e.purchasedProduct.receipt);
                    foreach (IPurchaseReceipt productReceipt in result)
                    {
                        productID = productReceipt.productID;
                        purchaseTime = productReceipt.purchaseDate;
                        transactionID = productReceipt.transactionID;
                    }
                } catch (IAPSecurityException) {
                    if (PurchaseErrorCallbacks.ContainsKey(productID))
                    {
                        PurchaseErrorCallbacks[productID].Invoke(productID, $"Failed to Purchase product {productID}. Invalid Receipt.");
                    }
                    validPurchase = false;
                }
                #endif
            }
            
            
            if (validPurchase) {
                if (PurchaseCompleteCallbacks.ContainsKey(productID))
                {
                    PurchaseCompleteCallbacks[productID].Invoke(productID, purchaseTime, transactionID);
                }
            }

            // Return Complete
            return PurchaseProcessingResult.Complete;
        }
        
        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        public void OnPurchaseFailed (Product i, PurchaseFailureReason p)
        {
            if (PurchaseErrorCallbacks.ContainsKey(i.definition.id))
            {
                PurchaseErrorCallbacks[i.definition.id].Invoke(i.definition.id, $"Failed to Purchase product {i.definition.id}. Error: {p}");
            }
        }
        #endregion
    }
}
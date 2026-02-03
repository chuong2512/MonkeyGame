using UnityEngine.Purchasing.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using Jackal;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPKey
{
    public const string PACK0 = "add0";
    public const string PACK1 = "add1";
    public const string PACK2 = "add3";
    public const string PACK3 = "add5";
    public const string PACK4 = "add10";
    public const string PACK5 = "add20";
    public const string PACK6 = "add30";
    public const string PACK7 = "add50";
}

public class IAPManager : PersistentSingleton<IAPManager>
{
    public static Action OnPurchaseSuccess;

    private bool _isBuyFromShop;

    private StoreController m_StoreController;

    async void Start()
    {
        try
        {
            m_StoreController = UnityIAPServices.StoreController();

            m_StoreController.OnPurchasePending += OnPurchasePending;

            await m_StoreController.Connect();

            m_StoreController.OnProductsFetched += OnProductsFetched;
            m_StoreController.OnPurchasesFetched += OnPurchasesFetched;

            var initialProductsToFetch = new List<ProductDefinition> { };


            initialProductsToFetch.Add(new(IAPKey.PACK1, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK2, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK3, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK4, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK5, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK7, ProductType.Consumable));
            initialProductsToFetch.Add(new(IAPKey.PACK6, ProductType.Consumable));


            m_StoreController.FetchProducts(initialProductsToFetch);

            m_StoreController.OnPurchaseFailed += OnPurchaseFailed;
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirm;

            await m_StoreController.Connect();
        }
        catch (Exception e)
        {
            throw; // TODO handle exception
        }
    }

    private void OnPurchaseConfirm(Order obj)
    {
        OnPurchaseSuccess?.Invoke();
    }


    public void BuyProductID(string productId)
    {
        _isBuyFromShop = true;

#if UNITY_EDITOR
        OnPurchaseComplete(productId);
#else
			m_StoreController.PurchaseProduct(productID);
#endif
    }


    private void OnPurchaseComplete(string productId)
    {
        OnPurchaseSuccess?.Invoke();
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}",
            product.definition.storeSpecificId, failureReason));
    }

    void OnPurchaseFailed(FailedOrder obj)
    {
    }

    void OnPurchasePending(PendingOrder obj)
    {
    }

    void OnProductsFetched(List<Product> products)
    {
        // Handle fetched products  
        m_StoreController.FetchPurchases();
    }

    void OnPurchasesFetched(Orders orders)
    {
        // Process purchases, e.g. check for entitlements from completed orders  
    }

    private static void Log(Product product, string where)
    {
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        OnPurchaseFailed(product, failureDescription.message);
    }

    private void OnPurchaseFailed(Product product, string msg)
    {
    }
}
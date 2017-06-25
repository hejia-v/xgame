package com.hejia.android.sdkprotocols.payment;

import android.os.Parcel;
import android.os.Parcelable;


public class BuyInfo implements Parcelable {

    private int count;
    private String payDescription;
    private String productId;
    private String productName;
    private double productOrginalPrice;
    private double productPrice;
    private String orderId;
    private int playerId;
    private String playerName;
    private String realmName;
    private String shopId;
    private String token;       // used in GooglePlay BillingPay


    public BuyInfo(String orderId, String itemId, String itemName, float price, int quantity, String shop_Id) {
        this(orderId, itemId, itemName, price, quantity, 0, "", "", shop_Id);
    }


    public BuyInfo(String orderId, String itemId, String itemName, float price, int quantity, int playerId, String playerName, String realmName, String shopId) {
        this.orderId = orderId;
        this.productId = itemId;
        this.productName = itemName;
        this.payDescription = itemName;
        this.productOrginalPrice = price;
        this.productPrice = price;
        this.count = quantity;
        this.playerId = playerId;
        this.playerName = playerName;
        this.realmName = realmName;
        this.shopId = shopId;
        this.token = "";
    }


    public final int getCount() {
        return count;
    }


    public final String getPayDescription() {
        return payDescription;
    }

    public final void setPayDescription(String payDescription) {
        this.payDescription = payDescription;
    }

    public final String getProductId() {
        return productId;
    }

    public final void setProductId(String productId) {
        this.productId = productId;
    }

    public final String getProductName() {
        return productName;
    }

    public final void setProductName(String productName) {
        this.productName = productName;
    }

    public final double getProductOrginalPrice() {
        return productOrginalPrice;
    }

    public final void setProductOrginalPrice(double productOrginalPriceString) {
        this.productOrginalPrice = productOrginalPriceString;
    }

    public final double getProductPrice() {
        return productPrice;
    }

    public final void setProductPrice(double productPrice) {
        this.productPrice = productPrice;
    }

    public final String getOrderId() {
        return orderId;
    }

    public final void setOrderId(String orderId) {
        this.orderId = orderId;
    }

    public final int getPlayerId() {
        return playerId;
    }

    public final void setPlayerId(int playerId) {
        this.playerId = playerId;
    }

    public final String getPlayerName() {
        return playerName;
    }

    public final void setPlayerName(String playerName) {
        this.playerName = playerName;
    }

    public final String getRealmName() {
        return realmName;
    }

    public final void setRealmName(String realmName) {
        this.realmName = realmName;
    }

    public final String getShopId() {
        return shopId;
    }

    public final String getToken() {
        return token;
    }

    public final void setToken(String token) {
        this.token = token;
    }


    @Override
    public int describeContents() {
        return 0;
    }


    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeString(orderId);
        dest.writeString(productId);
        dest.writeString(productName);
        dest.writeString(payDescription);
        dest.writeDouble(productOrginalPrice);
        dest.writeDouble(productPrice);
        dest.writeInt(count);
        dest.writeInt(playerId);
        dest.writeString(playerName);
        dest.writeString(realmName);
        dest.writeString(shopId);
    }


    private BuyInfo(Parcel in) {
        orderId = in.readString();
        productId = in.readString();
        productName = in.readString();
        payDescription = in.readString();
        productOrginalPrice = in.readDouble();
        productPrice = in.readDouble();
        count = in.readInt();
        playerId = in.readInt();
        playerName = in.readString();
        realmName = in.readString();
        shopId = in.readString();
    }

    public static final Parcelable.Creator<BuyInfo> CREATOR = new Parcelable.Creator<BuyInfo>() {
        public BuyInfo createFromParcel(Parcel in) {
            return new BuyInfo(in);
        }

        public BuyInfo[] newArray(int size) {
            return new BuyInfo[size];
        }
    };
}

package com.hejia.android.sdkprotocols.payment;

import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;


public interface IPayable extends IThirdPart {

    public boolean handleUnfinishedPayment();

    public boolean pay(BuyInfo buyInfo, IPaymentListener listener);

    public String getExtendStr();
}
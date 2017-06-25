package com.hejia.android.sdkprotocols.payment;

import android.os.Bundle;

public interface IPaymentListener {

    public void onPaymentFinished(final int provider, final int result, final Bundle extras);

}
package com.hejia.android.sdkprotocols.uninstall;

import android.util.SparseArray;

import com.hejia.android.clientfoundation.CalledByJNI;
import com.hejia.android.sdkprotocols.thirdparty.IThirdPart;
import com.hejia.android.sdkprotocols.thirdparty.ThirdPartManager;

@CalledByJNI
public class UninstallManager {

    public static void setUninstallOpenUrl(final String url) {

        SparseArray<IThirdPart> thirdpart = ThirdPartManager.getAllThirdPartys();

        for (int i = 0; i < thirdpart.size(); ++i) {
            if (thirdpart.valueAt(i) instanceof IUninstallable) {
                IUninstallable item = (IUninstallable) thirdpart.valueAt(i);
                item.setUninstallOpenUrl(url);
            }
        }
    }
}

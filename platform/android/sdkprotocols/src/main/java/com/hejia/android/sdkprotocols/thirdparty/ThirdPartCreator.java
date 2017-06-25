package com.hejia.android.sdkprotocols.thirdparty;

public class ThirdPartCreator {
    public static IThirdPart create(String className) {
        try {
            return (IThirdPart) Class.forName(className).newInstance();
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (InstantiationException e) {
            e.printStackTrace();
        } catch (IllegalAccessException e) {
            e.printStackTrace();
        }
        return null;
    }
}
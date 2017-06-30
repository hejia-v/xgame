package com.hejia.android.clientfoundation;

import android.content.res.AssetManager;
import android.util.Log;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

public class ResourcesManager {

    private static final String TAG = ResourcesManager.class.getSimpleName();

    public static String readFile(String fileName) {
        String jsonString = "";
        BufferedReader reader = null;
        try {
            AssetManager assertManager = AppContext.getInstance().getAssets();
            InputStreamReader inputStreamReader = new InputStreamReader(assertManager.open(fileName), "UTF-8");
            reader = new BufferedReader(inputStreamReader);
            String tempString = null;
            while ((tempString = reader.readLine()) != null) {
                jsonString += tempString;
            }
            reader.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        Log.i(TAG, "readFile: fileName:" + fileName + " content:" + jsonString);
        return jsonString;
    }
}
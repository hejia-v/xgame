package com.hejia.android.clientfoundation;

import android.content.res.AssetManager;
import android.util.Log;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;


public class FileParser {
    private static final String TAG = FileParser.class.getSimpleName();
    private static String mContent = null;

    public FileParser(String fileName) {
        mContent = readFile(fileName);
    }

    public String readFromFile() {
        return mContent;
    }

    private String readFile(String fileName) {
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

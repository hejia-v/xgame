package com.hejia.android.clientfoundation;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Iterator;


public class JsonParser {
    private static final String TAG = JsonParser.class.getSimpleName();
    private static JSONObject mJsonObject;

    public JsonParser(String jsonFileName) {
        FileParser fileParser = new FileParser(jsonFileName);
        String jsonString = fileParser.readFromFile();
        try {
            if (jsonString.startsWith("\ufeff")) {
                jsonString = jsonString.substring(1);
            }
            mJsonObject = new JSONObject(jsonString);
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    private JSONObject getSubJsonByKey(String key) {
        JSONObject subJsonObject = null;
        try {
            subJsonObject = mJsonObject.getJSONObject(key);
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return subJsonObject;
    }

    public String getStringByKeyInSubJson(String jsonKey, String stringKey) {
        String value = null;
        JSONObject subJsonObject = getSubJsonByKey(jsonKey);
        try {
            value = subJsonObject.getString(stringKey);
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return value;
    }

    public int getIntByKeyInSubJson(String jsonKey, String intKey) {
        int value = -1;
        JSONObject subJsonObject = getSubJsonByKey(jsonKey);
        try {
            value = subJsonObject.getInt(intKey);
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return value;
    }

    public ArrayList<Integer> getAllKeysInSubJson(String jsonKey) {
        JSONObject subJsonObject = getSubJsonByKey(jsonKey);
        Iterator<?> it = subJsonObject.keys();
        ArrayList<Integer> listKey = new ArrayList<Integer>();
        int i = 0;
        while (it.hasNext()) {
            listKey.add(Integer.valueOf(it.next().toString()));
        }
        return listKey;
    }

    public ArrayList<Integer> getSubJsonArrayValue(String jsonKey) {
        ArrayList<Integer> list = new ArrayList<Integer>();
        try {
            JSONArray array = mJsonObject.getJSONArray(jsonKey);
            System.out.println("=================" + array);
            for (int i = 0; i < array.length(); i++) {
                list.add(array.getInt(i));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }

        return list;
    }

    public JSONObject getValue(String key) {
        JSONObject value = null;
        try {
            value = mJsonObject.getJSONObject(key);
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return value;
    }

    public String getSubValue(String key, String subkey) {
        String value = null;
        try {
            value = getValue(key).getString(subkey).toString();
        } catch (JSONException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
        return value;
    }
}

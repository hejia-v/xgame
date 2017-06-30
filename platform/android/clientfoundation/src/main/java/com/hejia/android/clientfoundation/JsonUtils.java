package com.hejia.android.clientfoundation;

import com.alibaba.fastjson.JSON;

/**
 * Created by hejia on 2017/6/28.
 */

public class JsonUtils {
    public static final <T> T parseObject(String text, Class<T> clazz) {
        return JSON.parseObject(text, clazz);
    }
}

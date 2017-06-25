package com.hejia.android.clientfoundation;

/**
 * Created by hejia on 2017/6/25.
 * <p>
 * Class specified by this annotation will be keep with all members in this class when obfuscating
 */

import java.lang.annotation.ElementType;
import java.lang.annotation.Target;

/**
 * @author: hejia
 * @time: 2017/6/25 12:22
 */
@Target({ElementType.FIELD, ElementType.METHOD, ElementType.TYPE})
public @interface CalledByJNI {

}

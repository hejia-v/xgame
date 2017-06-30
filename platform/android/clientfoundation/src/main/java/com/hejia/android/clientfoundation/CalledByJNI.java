package com.hejia.android.clientfoundation;

/**
 * Class specified by this annotation will be keep with all members in this class when obfuscating
 */

import java.lang.annotation.ElementType;
import java.lang.annotation.Target;


@Target({ElementType.FIELD, ElementType.METHOD, ElementType.TYPE})
public @interface CalledByJNI {

}

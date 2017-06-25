# android studio tips
http://blog.csdn.net/zxc123e/article/details/52327731

纯unity方面的性能测试，就用unity直接打包到真机进行测试。
产品的部署则使用持续集成，先unity导出project, 然后脚本打包


# 生成android的keystore文件

发布一个android软件需要签名才可以，每个app都对应一个keystore

在命令行执行
keytool -genkey -alias android.keystore -keyalg RSA -validity 20000 -keystore android.keystore

> 其中参数-validity为证书有效天数，这里我们写的大些20000天。还有在输入密码时没有回显(尽管输就是啦) 并且退格, tab等都属于密码内容，这个密码在给.apk文件签名的时候需要。输入这个命令之后会提示您输入秘钥库的口令，接着是会提示你输入：姓氏，组织单位名称，组织名称，城市或区域名称，省市，国家、地区代码，密钥口令。按你自己的设置输入对应的数据就完成了

-genkey 生成文件。
-alias 别名。
-keyalg 加密算法。
-validity 有效期。
-keystore 文件名。

生成完成之后在jdk的bin目录之下就可以找到生成的keystore文件了，android.keystore就是刚才生成的文件了，打包android应用的时候就可以直接用了，不过一台电脑生成的keystore只能用一个应用。

查看命令keytool -list -keystore "android.keystore" 输入你设置的keystore密码

查看Android keystore 信息
命令行输入：
keytool -list -v -keystore ~/android.keystore -storepass android
keytool -list -v -keystore %HOME%/android.keystore -storepass 123456


当前keystore
口令 123456


# todo 
如何设置sdk ndk版本

# 参考
[unity-与Android交互(unity5、android studio) - Return - 博客频道 - CSDN.NET](http://blog.csdn.net/yangxuan0261/article/details/52427119)
https://www.zhihu.com/question/42500766
http://www.jianshu.com/p/857fb0d6ba3c
http://blog.csdn.net/yangxuan0261/article/details/52427119
http://blog.csdn.net/yangxuan0261/article/details/52420833
http://blog.csdn.net/ldghd/article/details/54312532

http://blog.csdn.net/onafioo/article/details/52914297
http://www.xuanyusong.com/archives/2418
http://blog.csdn.net/lzdidiv/article/details/53674143
http://www.jianshu.com/p/b00f9b7fdb06

http://blog.csdn.net/qq_27802721/article/details/52431376
http://blog.csdn.net/onafioo/article/details/51841368
http://blog.csdn.net/maomaoxiaohuo/article/details/51556749

android studio
http://blog.csdn.net/a772890398/article/details/50544172
http://www.cnblogs.com/smyhvae/p/4392611.html
https://stackoverflow.com/questions/26962948/how-to-update-imported-modules-with-code-modification-from-the-their-external-li
http://blog.csdn.net/shineflowers/article/details/45042485
http://blog.csdn.net/song19891121/article/details/51782968
http://blog.csdn.net/voiceofnet/article/details/45197883


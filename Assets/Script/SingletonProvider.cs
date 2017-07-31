
// new()是where字句的构造函数约束，带有new()约束的任何类型都必须有可访问的public无参构造函数。
// 正常来说C#创建的类默认都有一个无参的构造函数，即使没有写。
// 但是如果写了一个有参数的构造函数后，那么就没有默认无参的那个了，就需要自己手动写一个。

public class SingletonProvider<T> where T : new()
{
    SingletonProvider() { }

    public static T Instance
    {
        get { return SingletonCreator.instance; }
    }

    class SingletonCreator
    {
        static SingletonCreator() { }

        internal static readonly T instance = new T();
    }
}

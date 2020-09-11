using System;

public abstract class Singleton<T>
where T : Singleton<T>, new()
{
    private static T singleElement;
    private static bool isInstansiate = false;

    protected Singleton()
    {
        if (isInstansiate)
        {
            throw new Exception("SingletonException - use GetElement");
        }
        else
        {
            isInstansiate = true;
        }
    }

    public static T GetElement()
    {
        if (!isInstansiate)
        {
            singleElement = new T();
            isInstansiate = true;
        }

        return singleElement;
    }
}
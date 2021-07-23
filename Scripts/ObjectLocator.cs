using System;
using System.Collections.Generic;

public static class ObjectLocator
{
    private static Dictionary<Type, object> _service { get; } = new Dictionary<Type, object>();

    public static void Register<T>(T instance)
    {
        _service.Add(typeof(T), instance);
    }

    public static T Resolve<T>()
    {
        return (T)_service[typeof(T)];
    }
}
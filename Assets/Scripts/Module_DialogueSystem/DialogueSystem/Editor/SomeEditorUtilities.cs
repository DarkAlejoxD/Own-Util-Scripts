using System;
using System.Collections;
using System.Linq;
using System.Reflection;

public static class SomeEditorUtilities
{
    public static IEnumerable GetClasses(Type baseType)
    {
        return Assembly.GetAssembly(baseType).GetTypes().Where
               (t => t.IsClass && !t.IsAbstract && baseType.IsAssignableFrom(t));
    }
}
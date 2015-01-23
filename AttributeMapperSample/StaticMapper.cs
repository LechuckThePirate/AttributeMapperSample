using System;
using System.Linq;
using System.Reflection;
using AttributeMapperSample.Attributes;
using AttributeMapperSample.Classes;

namespace AttributeMapperSample
{
    public static class StaticMapper
    {

        public static bool IsMappableWith(this Type type, Type targetType)
        {
            var attrSource = type.GetCustomAttribute<MappableClassAttribute>(false);
            var attrTarget = targetType.GetCustomAttribute<MappableClassAttribute>();
            return (attrSource != null && attrSource.TargetType == targetType)
                || (attrTarget != null && attrTarget.TargetType == type);
        }

        public static bool IsMappableWith(this object obj, Type targetType)
        {
            return obj.GetType().IsMappableWith(targetType);
        }

        public static TOut MapTo<TOut, TIn>(this TIn objIn, TOut objOut) 
            where TIn : class 
            where TOut : class 
        {

            Type infoType = null;
            var attr = objIn.GetType().GetCustomAttribute<MappableClassAttribute>();
            if (attr != null)
                infoType = objIn.GetType();
            else
            {
                attr = typeof (TOut).GetCustomAttribute<MappableClassAttribute>();
                if (attr != null)
                    infoType = typeof (TOut);
            }

            if (null == infoType )
                throw new FormattedException("The types '{0}' and '{1}' are not mappable.",
                    objIn.GetType().Name, objOut.GetType().Name);

            infoType.GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(MappedPropertyAttribute), false).Any())
                .ToList()
                .ForEach(p => MapValue(infoType, p, objIn, objOut));

            return objOut;
        }

        public static TOut MapTo<TOut>(this object objIn) 
            where TOut : class, new ()
        {
            try
            {
                var objOut = Activator.CreateInstance<TOut>();
                return MapTo(objIn, objOut);
            }
            catch (Exception ex)
            {
                throw new FormattedException("Can't create object from class '{0}'. No empty public constructor?", 
                    ex, typeof(TOut).Name);
            }
        }

        private static void MapValue(Type infoType, PropertyInfo prop, object objIn, object objOut)
        {
            var attr = prop.GetCustomAttribute<MappedPropertyAttribute>();
            if (infoType == objOut.GetType())
            {
                var sourceProp = objIn.GetType().GetProperty(attr.MapTo ?? prop.Name);
                if (sourceProp != null)
                    Map(sourceProp, objIn, prop, objOut);
            }
            else
            {
                var targetProp = objOut.GetType().GetProperty(attr.MapTo ?? prop.Name);
                if (targetProp != null)
                    Map(prop, objIn, targetProp, objOut);
            }
        }

        private static void Map(PropertyInfo pIn, object objIn, PropertyInfo pOut, object objOut)
        {
            if (!pOut.PropertyType.IsAssignableFrom(pIn.PropertyType))
                throw new FormattedException("Can't map value from '{0}' ({1}) to '{2}' ({3}"
                    , pIn.Name, pIn.PropertyType.Name, pOut.Name,pOut.PropertyType.Name);

            try
            {
                pOut.SetValue(objOut,pIn.GetValue(objIn,null));
            }
            catch (Exception ex)
            {
                throw new FormattedException("Error converting data from {0} to {1}", ex, pIn.Name, pOut.Name);
            }
        }

    }
}

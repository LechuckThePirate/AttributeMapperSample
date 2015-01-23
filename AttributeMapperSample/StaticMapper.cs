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
            // One of the types must have MappableClassAttribute pointing to the 
            // other. If not, we can't map that!
            var attrSource = type.GetCustomAttribute<MappableClassAttribute>(false);
            var attrTarget = targetType.GetCustomAttribute<MappableClassAttribute>();
            return (attrSource != null && attrSource.AssociatedType == targetType)
                || (attrTarget != null && attrTarget.AssociatedType == type);
        }

        public static bool IsMappableWith(this object obj, Type targetType)
        {
            return obj.GetType().IsMappableWith(targetType);
        }

        public static TOut MapTo<TOut, TIn>(this TIn objIn, TOut objOut) 
            where TIn : class 
            where TOut : class 
        {
            // Is one class mappable to or from the other?
            if (!objIn.GetType().IsMappableWith(typeof(TOut)))
                throw new FormattedException("The types '{0}' and '{1}' are not mappable.",
                    objIn.GetType().Name, objOut.GetType().Name);

            Type infoType = null;
            // The class defining the mappings is the "In" or the "Out" object?
            var attr = objIn.GetType().GetCustomAttribute<MappableClassAttribute>();
            if (attr != null)
                infoType = objIn.GetType();
            else
            {
                attr = typeof (TOut).GetCustomAttribute<MappableClassAttribute>();
                if (attr != null)
                    infoType = typeof (TOut);
            }

            // Let's iterate the properties of the defining class searching for properties
            // with the attribute to map them
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
            // Get our custom attribute
            var attr = prop.GetCustomAttribute<MappedPropertyAttribute>();
            // Check wether the mapping defining class is objIn or objOut, to know
            // the direction of the mapping... set the source and target properties
            // and call map to copy the value
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
            // First of all... the types should be compatibles.
            // Here you got a nice work to make the method able to convert. You could
            // add a ConverterClass property to the attribute an invoke it here...
            if (!pOut.PropertyType.IsAssignableFrom(pIn.PropertyType))
                throw new FormattedException("Can't map value from '{0}' ({1}) to '{2}' ({3}"
                    , pIn.Name, pIn.PropertyType.Name, pOut.Name,pOut.PropertyType.Name);
            // Assign the value
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

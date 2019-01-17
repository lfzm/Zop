using System;
using System.Reflection;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// Some helper methods for entities.
    /// </summary>
    public static class EntityHelper
    {
        public static bool IsEntity(Type type)
        {
            return IsAssignableToGenericType(type, typeof(IEntity<>));
        }

        public static Type GetPrimaryKeyType<TEntity>()
        {
            return GetPrimaryKeyType(typeof(TEntity));
        }

        /// <summary>
        /// Gets primary key type of given entity type
        /// </summary>
        public static Type GetPrimaryKeyType(Type entityType)
        {
            foreach (var interfaceType in entityType.GetTypeInfo().GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                {
                    return interfaceType.GenericTypeArguments[0];
                }
            }

            throw new ZopException("Can not find primary key type of given entity type: " + entityType + ". Be sure that this entity type implements IEntity<TPrimaryKey> interface");
        }

        /// <summary>
        /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
        /// </summary>
        /// <param name="givenType">Type to check</param>
        /// <param name="genericType">Generic type</param>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var givenTypeInfo = givenType.GetTypeInfo();

            if (givenTypeInfo.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.GetTypeInfo().IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenTypeInfo.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenTypeInfo.BaseType, genericType);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeChallenge
{
    public static class SearchEngine
    {
        public static object[] Find(ZenDbContext dbContext, string tableName, string fieldName, object textToFind)
        {
            return Find(dbContext, tableName, dbContext.GetListFromTableName(tableName), fieldName, "=", textToFind);
        }
        public static object[] Find(ZenDbContext dbContext, string tableName, string fieldName, string op, object textToFind)
        {
            return Find(dbContext, tableName, dbContext.GetListFromTableName(tableName), fieldName, op, textToFind);
        }
        public static object[] Find(ZenDbContext dbContext, string tableName, IEnumerable entities, string fieldName, object textToFind)
        {
            return Find(dbContext, tableName, entities, fieldName, "=", textToFind);
        }
        public static object[] Find(ZenDbContext dbContext, string tableName, IEnumerable entities, string fieldName, string op, object textToFind)
        {
            PropertyInfo prop = null;
            object value;
            object valueToFind = textToFind;
            var foundEntities = new List<object>();
            bool isMatch;
            //  If no field specified, return all items.
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                foreach (var entity in entities)
                {
                    ExpandForeignRefs(dbContext, entity);
                    foundEntities.Add(entity);
                }
            }
            else if (fieldName.Equals("_id", StringComparison.CurrentCultureIgnoreCase) && op.Equals("="))
            {
                var singleResult = FindByKey(dbContext, tableName, textToFind);
                if(singleResult != null)
                {
                    ExpandForeignRefs(dbContext, singleResult);
                    foundEntities.Add(singleResult);
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    if (prop == null)
                    {
                        prop = entity.GetType().GetProperty(fieldName, BindingFlags.GetProperty | BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                        if (prop == null)
                        {
                            throw new Exception(string.Format("Unknown field name '{0}' in data model class '{1}'", fieldName, entity.GetType().Name));
                        }
                        try
                        {
                            if (prop.PropertyType.Equals(typeof(bool)))
                            {
                                if (!textToFind.GetType().Equals(typeof(bool)))
                                {
                                    valueToFind = Convert.ToBoolean(textToFind);
                                }
                            }
                            else if (prop.PropertyType.Equals(typeof(int)))
                            {
                                if (!textToFind.GetType().Equals(typeof(int)))
                                {
                                    valueToFind = Convert.ToInt32(textToFind);
                                }
                            }
                            else if (prop.PropertyType.Equals(typeof(DateTime)))
                            {
                                if (!textToFind.GetType().Equals(typeof(DateTime)))
                                {
                                    valueToFind = Convert.ToDateTime(textToFind);
                                }
                            }
                        }
                        catch
                        {
                            throw new Exception(string.Format("Error converting {0} to type {1}", textToFind, prop.PropertyType.Name));
                        }
                    }
                    value = prop.GetValue(entity);
                    if (value != null)
                    {
                        isMatch = false;
                        if (op.Equals("=") && value.Equals(valueToFind))
                        {
                            isMatch = true;
                        }
                        else if (op.Equals(">"))
                        {
                            //  Casting is slow. Have to figure out a way to do this without casting.
                            if (prop.PropertyType.Equals(typeof(int)) && (int)value > (int)valueToFind)
                            {
                                isMatch = true;
                            }
                            else if (prop.PropertyType.Equals(typeof(DateTime)) && (DateTime)value > (DateTime)valueToFind)
                            {
                                isMatch = true;
                            }
                        }
                        else if (op.Equals("<"))
                        {
                            //  Casting is slow. Have to figure out a way to do this without casting.
                            if (prop.PropertyType.Equals(typeof(int)) && (int)value < (int)valueToFind)
                            {
                                isMatch = true;
                            }
                            else if (prop.PropertyType.Equals(typeof(DateTime)) && (DateTime)value < (DateTime)valueToFind)
                            {
                                isMatch = true;
                            }
                        }
                        if (isMatch)
                        {
                            ExpandForeignRefs(dbContext, entity);
                            foundEntities.Add(entity);
                        }
                    }
                }
            }
            return foundEntities.ToArray();
        }
        /// <summary>
        /// This will expand foreign key references of the passed entity, loading foreign key
        /// references into a list (for collections) or an instance (for non-collections). 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static object ExpandForeignRefs(ZenDbContext dbContext, object entity)
        {
            //  If this entity has already had its references expanded, do not process again.
            var expandedRef = entity as IIsExpanded;
            if (expandedRef != null)
            {
                if (expandedRef.IsExpanded)
                {
                    return entity;
                }
                else
                {
                    expandedRef.IsExpanded = true;
                }
            }
            ForeignKeyAttribute attribute;
            object tempList;
            object[] results;
            MethodInfo method;
            PropertyInfo idProp = entity.GetType().GetProperty("_id", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty))
            {
                attribute = prop.GetCustomAttribute<ForeignKeyAttribute>();
                if (attribute != null)
                {
                    //  This is a list of related entities.
                    if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType))
                    {
                        //  Find the related entities, using the foreign entity field name from the attribute
                        //  and the _id field from the current entity.
                        results = Find(dbContext, dbContext.GetEntityNameFromModelType(prop.PropertyType.GenericTypeArguments[0]), attribute.Name, idProp.GetValue(entity).ToString());
                        if (results.Length != 0)
                        {
                            tempList = prop.GetValue(entity);
                            method = tempList.GetType().GetMethod("Add");
                            foreach (var result in results)
                            {
                                method.Invoke(tempList, new object[] { result });
                            }
                        }
                    }
                    else  //    This is a single reference to a foreign entity.
                    {
                        var tableName = dbContext.GetEntityNameFromModelType(prop.PropertyType);
                        //  Find the related entity, using the value of the property specified in the attribute (from the current entity)
                        //  and the _id field of the related entity.
                        results = Find(dbContext, tableName, "_id", entity.GetType().GetProperty(attribute.Name, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).GetValue(entity).ToString());
                        if (results.Length != 0)
                        {
                            prop.SetValue(entity, results[0]);
                        }
                    }
                }
            }
            return entity;
        }
        public static object FindByKey(ZenDbContext dbContext, string tableName, object textToFind)
        {
            var entityDef = dbContext.GetEntityDefinition(tableName);
            return entityDef.FindByKey(textToFind);
        }
    }
}
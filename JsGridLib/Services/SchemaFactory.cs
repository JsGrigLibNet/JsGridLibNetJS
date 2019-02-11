namespace JsGridLib.Services
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using JsGridLib.Models;

    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            return str;
        }
    }

    public class SchemaFactory
    {
        public SchemaSetUp SchemaFromType<T>(bool includeIdfield)
            where T : new()
        {
            Type modelType = typeof(T);
            object model = new T();
            var result = new SchemaSetUp();

            try
            {
                var clientColumnSetUp = new Dictionary<string, dynamic>();

                List<JsGridField> schema = this.CreateJsGridProperties(modelType, null, model).Select(
                    x =>
                    {
                        string title = Regex.Replace(x.PropertyName, @"\p{Lu}", m => "\t" + m.Value).Trim();
                        string urlRouteName = title.ToLower();
                        urlRouteName = title.Replace("\t", "");
                        var schemaItem =
                            new JsGridField
                            {
                                PropertyName = x.PropertyName,
                                isPrimaryKey = x.PropertyName == "Id",
                                visible = x.PropertyName != "Id" && x.PropertyName != "IdForeign",
                                displayAsCheckBox = x.displayAsCheckBox,
                                UIRoute = x.PropertyName,
                                Title = title,
                                ClientType = x.ClientType,
                                field = x.PropertyName, //.ToCamelCase(),
                                DefaultValues = x.DefaultValues,
                                OriginalType = x.OriginalType,
                                PropertyValue = x.PropertyValue,
                                ValueField = x.ValueField,
                                TextField = x.TextField,
                                editType = x.editType,
                                width = 100,
                                Sorting = false,
                                Items = x.Items
                            };

                        if (schemaItem.DefaultValues != null && schemaItem.DefaultValues.Count > 0)
                        {
                            var obj = new Dictionary<string, List<string>>();
                            obj[schemaItem.field] = schemaItem.DefaultValues;
                            clientColumnSetUp[urlRouteName] = obj;
                        }
                        else
                        {
                            clientColumnSetUp[urlRouteName] = schemaItem.field;
                        }

                        return schemaItem;
                    }).ToList();

                result.Fields = schema.Where(x => includeIdfield || x.field != "id" && x.field != "idforeign").Select(
                    x =>
                    {
                        dynamic data = new ExpandoObject();
                        data.field = x.field;
                        data.editType = x.editType;
                        data.headerText = x.Title;
                        if (x.format != null)
                            data.format = x.format;
                        data.width = x.width;
                        // data.ValueField = x.ValueField;
                        // data.textField = x.TextField;
                        // data.Items = x.Items;
                        data.textAlign = "Left";
                        data.isPrimaryKey = x.isPrimaryKey;
                        data.visible = x.visible;
                        data.displayAsCheckBox = x.displayAsCheckBox;
                        return data;
                    }).ToList();
                //result.Fields.Add(new { Type = JsGridFieldType.Control.ToString().ToLower() });
                result.ClientColumnSetUp = clientColumnSetUp;
            }
            catch (Exception e)
            {
                result.SchemaCreationException = e;
            }

            return result;
        }

        bool PropertyInfoIsACollection(PropertyInfo p)
        {
            Type tColl = typeof(ICollection<>);
            Type t = p.PropertyType;
            return t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) || t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl);
        }

        bool PropertyInfoIsADictionary(PropertyInfo p)
        {
            Type tColl = typeof(Dictionary<,>);
            Type t = p.PropertyType;
            return t.IsGenericType && tColl.IsAssignableFrom(t.GetGenericTypeDefinition()) || t.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == tColl);
        }

        IEnumerable<JsGridField> CreateJsGridProperties(Type type, string parentName = null, object model = null)
        {
            parentName = parentName ?? type.Name;
            var result = new List<JsGridField>();
            PropertyInfo[] myPropertyInfo = type.GetProperties();
            foreach (PropertyInfo info in myPropertyInfo)
            {
                object PropertyValue = model == null ? null : info.GetValue(model);
                string propertyName = info.PropertyType.Name.ToLower();

                if (typeof(Enum).IsAssignableFrom(info.PropertyType))
                {
                    int i = 0;
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.Enum,
                            PropertyName = info.Name,
                            DefaultValues = Enum.GetNames(info.PropertyType).ToList(),
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue,
                            editType = JsGridFieldType.Select.ToString().ToLower(),
                            ValueField = "id",
                            TextField = "name",
                            Items = Enum.GetNames(info.PropertyType).ToList().Select(
                                x => new DropDownItem
                                {
                                    Name = x,
                                    Id = i++
                                }).ToList()
                        });
                }
                else if (this.PropertyInfoIsACollection(info))
                {
                    if (this.PropertyInfoIsADictionary(info))
                        result.Add(
                            new JsGridField
                            {
                                ClientType = JsGridObjectTypes.Dictionary,
                                PropertyName = info.Name,
                                OriginalType = info.PropertyType,
                                PropertyValue = PropertyValue,
                                editType = JsGridFieldType.Select.ToString().ToLower()
                            });
                    else
                        result.Add(
                            new JsGridField
                            {
                                ClientType = JsGridObjectTypes.Array,
                                PropertyName = info.Name,
                                OriginalType = info.PropertyType,
                                PropertyValue = PropertyValue,
                                editType = JsGridFieldType.Select.ToString().ToLower()
                            });
                }
                else if (info.PropertyType.Assembly == Assembly.GetExecutingAssembly())
                {
                    result.AddRange(this.CreateJsGridProperties(info.PropertyType, parentName + "." + info.Name, PropertyValue));
                }
                else if (propertyName.Contains("bool"))
                {
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.Boolean,
                            PropertyName = info.Name,
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue,
                            editType = JsGridFieldType.booleanedit.ToString().ToLower(),
                            displayAsCheckBox = true
                        });
                }
                else if (propertyName.Contains("int") || propertyName.Contains("float") || propertyName.Contains("double"))
                {
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.Number,
                            PropertyName = info.Name,
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue,
                            editType = JsGridFieldType.numericedit.ToString().ToLower()
                        });
                }
                else if (propertyName.Contains("date"))
                {
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.DateTime,
                            PropertyName = info.Name,
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue,
                            format = new
                            {
                                type = "dateTime",
                                format = "M/d/y hh:mm a"
                            },
                            editType = JsGridFieldType.datetimepickeredit.ToString().ToLower()
                        });
                }
                else if (propertyName.Contains("decimal"))
                {
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.Money,
                            PropertyName = info.Name,
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue,
                            editType = JsGridFieldType.numericedit.ToString().ToLower()
                        });
                }
                else
                {
                    result.Add(
                        new JsGridField
                        {
                            ClientType = JsGridObjectTypes.String,
                            PropertyName = info.Name,
                            OriginalType = info.PropertyType,
                            PropertyValue = PropertyValue
                            //  editType = JsGridFieldType.Text.ToString().ToLower()
                        });
                }
            }

            return result;
        }
    }
}
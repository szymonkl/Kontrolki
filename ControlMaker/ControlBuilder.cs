using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;
using Reflection;
using Label = System.Windows.Forms.Label;


namespace ControlMaker
{
    public class ControlBuilder
    {

        public static int VerticalDistance { get; set; } = -5;
        public static int HorizontalDistance { get; set; } = 10;
        public static int LeftMargin { get; set; } = 15;
        public static int RightMargin { get; set; } = 20;
        public static int TopMargin { get; set; } = 20;
        public static int BottomMargin { get; set; } = 20;

        public static int GroupBoxHeight = 25;
        public static int GroupBoxWidth { get; set; } = 20;


        public Control BuildControlForProperty(object controlObject, PropertyInfo property)
        {
            var control = new Control();
            Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(ControlsAttribute));
            if (attributes.Length > 0)
            {
                Attribute attribute = attributes[0];
                var controlAttribute = (ControlsAttribute) attribute;
                switch (controlAttribute.ControlType)
                {
                    case ControlsAttribute.ControlTypes.Label:
                        control = CreateLabel(property);
                        break;
                    case ControlsAttribute.ControlTypes.TextBox:
                        control = CreateTextBox(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.LabelTextbox:
                        control = CreateLabelAndTextBox(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.DropDownList:
                        control = CreateDropDownList(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.LabelDropDownList:
                        control = CreateLabelAndDropDownList(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.Checkbox:
                        control = CreateCheckBox(controlObject, property);
                        break;   
                    case ControlsAttribute.ControlTypes.LabelCheckBox:
                        control = CreateLabelAndCheckBox(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.DateTimePicker:
                        control = CreateTimePicker(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.LabelDateTimePicker:
                        control = CreateLabelTimePicker(controlObject, property);
                        break;
                    case ControlsAttribute.ControlTypes.TimeRangePicker:
                        control = CreateTimeRangePicker(controlObject, property);
                        break;

                }
            }
            else
            {
                control = CreateLabelWithPropertyValue(controlObject, property);
            }
            return control;
            
        }

        private static Control CreateTimeRangePicker(object controlObject, PropertyInfo property)
        {
            return TimeRangeBuilder.CreateTimeRangeControl(controlObject, property);
        }

        private  static Control CreateLabelTimePicker(object controlObject, PropertyInfo property)
        {
            var groupBox = new GroupBox();

            groupBox.Name = ControlNameBuilder<GroupBox>.BuildName(property.Name);
            if (property.PropertyType == typeof(DateTime))
            {
                groupBox = CreateControlGroup<Label, DateTimePicker>(controlObject, property); 
            }

            return groupBox;
        }

        private static Control CreateTimePicker(object controlObject, PropertyInfo property)
        {
            DateTimePicker dateTimePicker = new DateTimePicker();
            dateTimePicker.Name = ControlNameBuilder<DateTimePicker>.BuildName(property.Name);
            dateTimePicker.Format = DateTimePickerFormat.Time;
            dateTimePicker.ShowUpDown = true;
            if (property.PropertyType == typeof(DateTime))
            {
                dateTimePicker.Value = (DateTime)property.GetValue(controlObject); 
            }
            return dateTimePicker;
        }


        public void CreateListViewInGroupBox(object controlObject, GroupBox groupBox)
        {
            ListView listView = new ListView();
            
            var property = GetPropertyForListView(controlObject);

            if (property != null)
            {
                listView.Name = ControlNameBuilder<ListView>.BuildName(property.Name);
                listView = CreateListView(controlObject, property);
            }
            SetListViewProperties(listView);
            groupBox.Controls.Add(listView);


        }

        private static void SetListViewProperties(ListView listView)
        {
            listView.BorderStyle = BorderStyle.Fixed3D;
            listView.GridLines = true;
            listView.View = View.Details;
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            
        }

        

        private static PropertyInfo GetPropertyForListView(object controlObject)
        {
            Type type = controlObject.GetType();
            var properties = type.GetProperties().ToList();
            var property =
                properties.FirstOrDefault(
                    predicate =>
                        predicate.GetCustomAttribute<ControlsAttribute>().ControlType ==
                        ControlsAttribute.ControlTypes.ListView ||
                        predicate.GetCustomAttribute<ControlsAttribute>().ControlType ==
                        ControlsAttribute.ControlTypes.FilterableListView);
            return property;
        }

        private ListView CreateListView(object controlObject, PropertyInfo property)
        {
            ListView listView = new ListView();
            if (_checkPropertyType.Invoke(property) && _checkPropertyArguments.Invoke(property))
            {
                    CreateColumsInListView(property, listView);
                    FillListViewWithData(controlObject, property, listView);
            }
            return listView;
           
        }
        private static void CreateColumsInListView(PropertyInfo property, ListView listView)
        {
            Type type = property.PropertyType.GetGenericArguments()[0];
            var objectProperties = type.GetProperties();
            foreach (var objectProperty in objectProperties)
            {
                Attribute attribute = objectProperty.GetCustomAttribute(typeof(NazwaAttribute));
                string columnName = ((NazwaAttribute) attribute).DisplayName;
                listView.Columns.Add(columnName);
            }
        }
        private static void FillListViewWithData(object controlObject, PropertyInfo property, ListView listView)
        {
            var listWithRowDataObject = property.GetValue(controlObject);
            if (listWithRowDataObject != null)
            {
                List<object> listWithRowData =
                    (listWithRowDataObject as IEnumerable<object>).ToList();

                foreach (var rowDataObject in listWithRowData)
                {
                    ListViewItem listViewItem = null;
                    var isFirstParameter = true;
                    foreach (var columnData in rowDataObject.GetType().GetProperties())
                    {
                        if (isFirstParameter)
                        {
                            listViewItem = new ListViewItem(columnData.GetValue(rowDataObject).ToString());
                            isFirstParameter = false;
                        }
                        else
                        {
                            listViewItem.SubItems.Add(columnData.GetValue(rowDataObject).ToString());
                        }
                    }
                    if (listViewItem != null) listView.Items.Add(listViewItem);
                }
            }
        }

        private readonly Predicate<PropertyInfo> _checkPropertyType =
            property =>
                property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                property.PropertyType.GenericTypeArguments.Length > 0;

        private readonly Predicate<PropertyInfo> _checkPropertyArguments =
            property => property.PropertyType.GetGenericArguments().Length > 0;

        private static readonly Predicate<PropertyInfo> _checkType =
            property =>
                property.PropertyType.IsGenericType &&
                property.PropertyType.GetGenericTypeDefinition() == typeof(List<>) &&
                property.PropertyType.GenericTypeArguments.Length > 0;

        private static readonly Predicate<PropertyInfo> _checkArguments =
            property => property.PropertyType.GetGenericArguments().Length > 0;


        private static Label CreateLabel(PropertyInfo property)
        {
            var label = new Label();
            label.Name = ControlNameBuilder<Label>.BuildName(property.Name);
            Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(NazwaAttribute));
            if (attributes.Length > 0)
            {
                var attribute = attributes[0];
                label.Text = ((NazwaAttribute)attribute).DisplayName;
            }
            else
            {
                label.Text = property.Name;
            }
            return label;
        }
        private static GroupBox CreateLabelAndTextBox(object controlObject, PropertyInfo property)
        {
            var groupBox = new GroupBox();

            groupBox.Name = ControlNameBuilder<GroupBox>.BuildName(property.Name);
            groupBox.Visible = true;
            groupBox = CreateControlGroup<Label,TextBox>(controlObject, property);

            return groupBox;
        }
        private static TextBox CreateTextBox(object controlObject, PropertyInfo property) 
        {
            TextBox textBox = new TextBox();
            textBox.Name = ControlNameBuilder<TextBox>.BuildName(property.Name);
            textBox.Text = property.GetValue(controlObject).ToString();
            return textBox;
            
        }
        private static Label CreateLabelWithPropertyValue(object controlObject, PropertyInfo property)
        {
            var label = new Label();
            label.Name = ControlNameBuilder<Label>.BuildName(property.Name);
            label.Text = property.GetValue(controlObject).ToString();
            return label;
        }
        private static ComboBox CreateDropDownList(object controlObject, PropertyInfo property)
        {
            var comboBox = new ComboBox
            {
                Name = ControlNameBuilder<ComboBox>.BuildName(property.Name),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            try
            {
                if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    comboBox.DataSource = property.GetValue(controlObject);
                }
            }
            catch (Exception)
            {
                comboBox.DataSource = "Nie można załadować danych";
            }
            return comboBox;



        }
        private static GroupBox CreateLabelAndDropDownList(object controlObject, PropertyInfo property)
        {
            var groupBox = new GroupBox();

            groupBox.Name = ControlNameBuilder<GroupBox>.BuildName(property.Name);
            groupBox.Visible = true;
            groupBox = CreateControlGroup<Label, ComboBox>(controlObject, property);

            return groupBox;
        }
        private static CheckBox CreateCheckBox(object controlObject, PropertyInfo property)
        {
            CheckBox checkBox = new CheckBox {Name = ControlNameBuilder<CheckBox>.BuildName(property.Name)};

            if (property.PropertyType == typeof(bool))
            {
                checkBox.Checked = (bool)property.GetValue(controlObject);
            }
            return checkBox;
        }
        private static GroupBox CreateLabelAndCheckBox(object controlObject, PropertyInfo property)
        {
            var groupBox = new GroupBox
            {
                Name = ControlNameBuilder<GroupBox>.BuildName(property.Name),
                Visible = true
            };
            
            if (property.PropertyType == typeof(bool))
            {
               groupBox = CreateControlGroup<Label, CheckBox>(controlObject, property);
            }

            return groupBox;

        }


        private static GroupBox CreateControlGroup<TControlType1, TControlType2>(object controlObject, PropertyInfo property)
        {
            GroupBox groupBox = new GroupBox();
            var control1 = CreateControl<TControlType1>(controlObject, property);
            var control2 = CreateControl<TControlType2>(controlObject, property);
           
            var control1Location = new Point(LeftMargin, TopMargin);
            var control2Location = new Point(LeftMargin + HorizontalDistance + control1.ClientSize.Width, control1Location.Y + VerticalDistance);
            control1.Location = control1Location;
            control2.Location = control2Location;
            groupBox.Controls.Add(control1);
            groupBox.Controls.Add(control2);
            groupBox.Height = control1.Height + GroupBoxHeight;
            groupBox.Width = LeftMargin + control1.Width + HorizontalDistance + control2.Width + GroupBoxWidth +
                             RightMargin;
           
            return groupBox;
        }
        private static Control CreateControl<TControlType>(object controlObject, PropertyInfo property)
        {
            var control = new Control();
            if (typeof(TControlType) == typeof(Label)) control = CreateLabel(property);
            if (typeof(TControlType) == typeof(TextBox)) control = CreateTextBox(controlObject, property);
            if(typeof(TControlType) == typeof(CheckBox)) control = CreateCheckBox(controlObject, property);
            if (typeof(TControlType) == typeof(ComboBox)) control = CreateDropDownList(controlObject, property);
            if (typeof(TControlType) == typeof(DateTimePicker)) control = CreateTimePicker(controlObject, property);
            
            
            return control;
        }


        private object _controlObject;
        private GroupBox _filterableGroupBox;
        private readonly ViewUpdater _viewUpdater = new ViewUpdater();

        public void CreateFilterableListView(object controlObject, GroupBox groupBox)
        {
            _controlObject = controlObject;
            _filterableGroupBox = groupBox;
            Type type = controlObject.GetType();
            var properties = type.GetProperties().ToList();
            var property = properties.Find(p => p.GetCustomAttribute<ControlsAttribute>().ControlType == ControlsAttribute.ControlTypes.FilterableListView);
            if (_checkPropertyType.Invoke(property) && _checkPropertyArguments.Invoke(property))
            {
                //Create name combobox
                Type listObjectType = property.PropertyType.GetGenericArguments()[0];
                var objectProperties = listObjectType.GetProperties();
                var dropDownListData = new SortedDictionary<string, string>();
                foreach (var objectProperty in objectProperties)
                {
                    string propertyName = objectProperty.Name;
                    string propertyDisplayName = objectProperty.GetCustomAttribute<NazwaAttribute>().DisplayName;
                    dropDownListData.Add(propertyName, propertyDisplayName);
                }
                ComboBox nameComboBox = new ComboBox
                {
                    Name = ControlNameBuilder<ComboBox>.BuildFilterName(property.Name),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DataSource = new BindingSource(dropDownListData, null),
                    ValueMember = "Key",
                    DisplayMember = "Value"
                };

                //Create comparison combobox
               
                ResourceSet resourceSet =
                    ComparisonTypeDictionary.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

                var comparisonTypeDictionary = new SortedDictionary<string, string>();
                foreach (DictionaryEntry resource in resourceSet)
                {
                    comparisonTypeDictionary.Add(resource.Key.ToString(), resource.Value.ToString());
                }
                ComboBox comparisonCombobox = new ComboBox
                {
                    Name = ControlNameBuilder<ComboBox>.BuildFilterComparerName(property.Name),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DataSource = new BindingSource(comparisonTypeDictionary, null),
                    ValueMember = "Key",
                    DisplayMember = "Value"
                };

                //Create value textbox
                TextBox valueTextBox = new TextBox
                {
                    Name = ControlNameBuilder<TextBox>.BuildFilterValueName(property.Name)
                };
                //Create button
                Button button = new Button
                {
                    Name = ControlNameBuilder<Button>.BuildFilterButtonName(property.Name),
                    //Text = ControlNames.Filter,
                    AutoSize = true
                };
                button.Click += Button_Click;

                
                groupBox.Controls.Add(nameComboBox);
                groupBox.Controls.Add(comparisonCombobox);
                groupBox.Controls.Add(valueTextBox);
                groupBox.Controls.Add(button);
                ControlPositioner.ToLeft(nameComboBox, comparisonCombobox, valueTextBox, button);
                
            }

            ListView listView = CreateListView(controlObject, GetPropertyForListView(controlObject));
            groupBox.Controls.Add(listView);
            SetListViewProperties(listView);
            ControlPositioner.TopMargin = 70;
            ControlPositioner.ToLeft(listView);
            ControlPositioner.SetControlSize(listView);
            //CreateListViewInGroupBox(controlObject, groupBox);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            BuildFilters(_controlObject, _filterableGroupBox);
        }

        private static PropertyInfo GetPropertyToFiltering(object controlObject, ComboBox cbProperty)
        {
            PropertyInfo selectedProperty = null;
            PropertyInfo objectProperty = GetPropertyForListView(controlObject);
            if (_checkType.Invoke(objectProperty) && _checkArguments.Invoke(objectProperty))
            {
                //Create name combobox
                Type listObjectType = objectProperty.PropertyType.GetGenericArguments()[0];
                var objectPropertyProperties = listObjectType.GetProperties();
                selectedProperty =
                    objectPropertyProperties.First(
                        p => p.Name == cbProperty.SelectedValue.ToString());

            }
            return selectedProperty;
        }

        private static object GetListObject(object controlObject)
        {
            PropertyInfo selectedProperty = null;
            PropertyInfo objectProperty = GetPropertyForListView(controlObject);
            if (_checkType.Invoke(objectProperty) && _checkArguments.Invoke(objectProperty))
            {
                return objectProperty.PropertyType.GetGenericArguments()[0];
            }
            return selectedProperty;
        }

        private static string GetComparisonType(ComboBox cbComparison)
        {
            return cbComparison.SelectedValue.ToString();
        }

        private static ComboBox GetPropertyCombobox(string propertyName, GroupBox groupBox)
        {
            return (ComboBox)groupBox.Controls.Find(ControlNameBuilder<ComboBox>.BuildFilterName(propertyName), false)[0];
        }

        private static TextBox GetValueTextBox(string propertyName, GroupBox groupBox)
        {
            return (TextBox) groupBox.Controls.Find(ControlNameBuilder<TextBox>.BuildFilterValueName(propertyName), false)[0];
        }

        private static ComboBox GetComparisonComboBox(string propertyName, GroupBox groupBox)
        {
            return (ComboBox)groupBox.Controls.Find(ControlNameBuilder<TextBox>.BuildFilterComparerName(propertyName), false)[0];
        }


        public static void BuildFilters(object controlObject, GroupBox groupBox)
        {
            object listObject = GetListObject(controlObject);
            
            ParameterExpression left = Expression.Parameter(listObject.GetType(), "ObjectArgument");
            var propertyList = GetPropertyForListView(controlObject);
            ComboBox propertyNameComboBox = GetPropertyCombobox(propertyList.Name, groupBox);
            string propertyFilteringName = GetPropertyToFiltering(controlObject, propertyNameComboBox).Name;
            Expression leftProperty = Expression.Property(left, listObject.GetType().GetProperty(propertyFilteringName));
            ParameterExpression right = Expression.Parameter(typeof(string), GetValueTextBox(propertyList.Name, groupBox).Text);
            BinaryExpression filterExpression =
                BinaryExpressionBuilder(GetComparisonType(GetComparisonComboBox(propertyList.Name, groupBox)), left,
                    right);
            IQueryable<object> list = (propertyList.GetValue(controlObject) as IQueryable<object>);

            MethodCallExpression filterExpressionCall = Expression.Call(typeof(Queryable), "Where",
                new Type[] {list.ElementType}, list.Expression,
                Expression.Lambda<Func<object, string, bool>>(filterExpression, new ParameterExpression[] {left, right}));
            IQueryable<object> results = list.Provider.CreateQuery<object>(filterExpressionCall);

            
        }

        private static BinaryExpression BinaryExpressionBuilder(string expressionType, ParameterExpression left, ParameterExpression right)
        {
            BinaryExpression expression = null;
            if (expressionType == ComparisonTypeDictionary.AreEqual)
            {
                expression = Expression.Equal(left, right);
            }
            else if (expressionType == ComparisonTypeDictionary.AreNotEqual)
            {
                expression = Expression.NotEqual(left, right);
            }
            else if (expressionType == ComparisonTypeDictionary.GreatherOrEqual)
            {
                expression = Expression.GreaterThanOrEqual(left, right);
            }
            else if (expressionType == ComparisonTypeDictionary.GreatherThan)
            {
                expression = Expression.GreaterThan(left, right);
            }
            else if (expressionType == ComparisonTypeDictionary.LessOrEqual)
            {
                expression = Expression.LessThanOrEqual(left, right);
            }
            else if (expressionType == ComparisonTypeDictionary.LessThan)
            {
                expression = Expression.LessThan(left, right);
            }
            return expression;

        }
        
    }
}



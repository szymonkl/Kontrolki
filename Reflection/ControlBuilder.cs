using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

using System.Data;
using System.Globalization;
using System.IO;


namespace Reflection
{
    public class ControlBuilder 
    {
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

                   
                }
            }
            else
            {
                control = CreateLabelWithPropertyValue(controlObject, property);
            }
            return control;
            
        }

        
        public void CreateListViewInGroupBox(object controlObject, GroupBox groupBox)
        {
            ListView listView = new ListView();
            
            Type type = controlObject.GetType();
            var properties = type.GetProperties().ToList();
            var property =
                properties.FirstOrDefault(
                    predicate =>
                        predicate.GetCustomAttribute<ControlsAttribute>().ControlType ==
                        ControlsAttribute.ControlTypes.ListView ||
                        predicate.GetCustomAttribute<ControlsAttribute>().ControlType ==
                        ControlsAttribute.ControlTypes.FilterableListView);

            if (property != null)
            {
                listView.Name = ControlNameBuilder<ListView>.BuildName(property.Name);
                listView = CreateListView(controlObject, property);
            }
            listView.BorderStyle = BorderStyle.Fixed3D;
            
            
            //listView.Size = new Size(groupBox.ClientSize.Width, groupBox.ClientSize.Height);
            listView.GridLines = true;
            listView.View = View.Details;
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            groupBox.Controls.Add(listView);


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
           
            var control1Location = new Point(groupBox.Padding.Left, groupBox.Padding.Top + 10);
            var control2Location = new Point(control1Location.X + control1.Width, control1Location.Y);
            control1.Location = control1Location;
            control2.Location = control2Location;
            groupBox.Controls.Add(control1);
            groupBox.Controls.Add(control2);
            groupBox.AutoSize = true;
            groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            return groupBox;
        }
        private static Control CreateControl<TControlType>(object controlObject, PropertyInfo property)
        {
            var control = new Control();
            if (typeof(TControlType) == typeof(Label)) control = CreateLabel(property);
            if (typeof(TControlType) == typeof(TextBox)) control = CreateTextBox(controlObject, property);
            if(typeof(TControlType) == typeof(CheckBox)) control = CreateCheckBox(controlObject, property);
            if (typeof(TControlType) == typeof(ComboBox)) control = CreateDropDownList(controlObject, property);
            return control;
        }
        

         
        public void UpdateTextBoxes(object controlObject, GroupBox groupBox)
        {
            foreach (var property in controlObject.GetType().GetProperties())
            {
                UpdateTextBoxWithValues(controlObject, property, groupBox);
            }
        }
        private void UpdateTextBoxWithValues(object controlObject, PropertyInfo property, GroupBox groupBox)
        {
            var controls = groupBox.Controls.Find(ControlNameBuilder<TextBox>.BuildName(property.Name), true);
            if (controls.Length > 0)
            {
                ((TextBox) controls[0]).Text = property.GetValue(controlObject).ToString();
            }
        }
        public void UpdateObjectFromTextBoxes(object targetObject, GroupBox groupBox)
        {
            var properies = targetObject.GetType().GetProperties();
            var allGroupBoxesControls = new List<Control>();
            foreach (Control control in groupBox.Controls)
            {
                allGroupBoxesControls.AddRange(control.Controls.OfType<Control>());
            }

            foreach (Control control in allGroupBoxesControls)
            {
                if (control is TextBox)
                {
                   var property = properies.First(
                        p => p.Name == ControlNameBuilder<TextBox>.GetPropertyNameFromControlName(control.Name));
                    if (property != null)
                    {
                        if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(targetObject, control.Text);
                        }
                        else if(property.PropertyType == typeof(int))
                        {
                            property.SetValue(targetObject, Convert.ToInt32(control.Text));
                        }
                    }
                }
            }
        }

        public void CreateFilterableListView(object controlObject, GroupBox groupBox)
        {
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
               
                
                //var resourceReader = new ResXResourceReader(Path.GetFullPath("ComparisonTypeDictionary.resx"));
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
                    Text = Dictionary.Filter,
                    AutoSize = true
                };
                button.Click += Button_Click;

                
                groupBox.Controls.Add(nameComboBox);
                groupBox.Controls.Add(comparisonCombobox);
                groupBox.Controls.Add(valueTextBox);
                groupBox.Controls.Add(button);
                ControlPositioner.ToLeft(nameComboBox, comparisonCombobox, valueTextBox, button);
                
            }



            //CreateListViewInGroupBox(controlObject, groupBox);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.NullableBinding
{
    /// <summary>
    /// A set of extensions for use with the nullable data binding helper methods.
    /// </summary>
    public static class BindingExtensions
    {
        /// <summary>
        /// Adds a standard data-binding to a WinForms control.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSourceProp"></typeparam>
        /// <typeparam name="TControlValueMember"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        /// <param name="controlValueMemberExpression"></param>
        /// <param name="dataMemberExpression"></param>
        public static Binding AddDataBinding<TControl, TSource, TSourceProp, TControlValueMember>(this TControl control,
            BindingSourceGeneric<TSource> dataSource,
            Expression<Func<TControl, TControlValueMember>> controlValueMemberExpression,
            Expression<Func<TSource, TSourceProp>> dataMemberExpression)
            where TControl : Control
            where TSource : class
        {
            return AddDataBinding(control, controlValueMemberExpression, dataSource, dataMemberExpression, false);
        }

        /// <summary>
        /// Adds a standard data-binding to a WinForms control, using the <see cref="Control.Text"/> property as the property to bind to.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMemberExpression"></param>
        public static Binding AddTextDataBinding<TControl, TSource>(this TControl control,
            BindingSourceGeneric<TSource> dataSource,
            Expression<Func<TSource, string>> dataMemberExpression)
            where TControl : Control
            where TSource : class
        {
            return AddDataBinding(control, _ => _.Text, dataSource, dataMemberExpression, false);
        }

        /// <summary>
        /// Adds a nullable data-binding to a WinForms control.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TSourceProp"></typeparam>
        /// <typeparam name="TControlValueMember"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        /// <param name="valueMemberExpression"></param>
        /// <param name="dataMemberExpression"></param>
        public static Binding AddNullableDataBinding<TControl, TSource, TSourceProp, TControlValueMember>(this TControl control,
            BindingSourceGeneric<TSource> dataSource,
            Expression<Func<TControl, TControlValueMember>> valueMemberExpression,
            Expression<Func<TSource, TSourceProp>> dataMemberExpression)
            where TControl : Control
            where TSource : class
        {
            return AddDataBinding(control, valueMemberExpression, dataSource, dataMemberExpression, true);
        }

        /// <summary>
        /// Adds a nullable data-binding to a WinForms control, using the <see cref="Control.Text"/> property as the property to bind to.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="control"></param>
        /// <param name="dataSource"></param>
        /// <param name="dataMemberExpression"></param>
        public static Binding AddNullableTextDataBinding<TControl, TSource>(this TControl control,
            BindingSourceGeneric<TSource> dataSource,
            Expression<Func<TSource, string>> dataMemberExpression)
            where TControl : Control
            where TSource : class
        {
            return AddDataBinding(control, _ => _.Text, dataSource, dataMemberExpression, true);
        }

        /// <summary>
        /// Adds a data-binding to a WinForms control with support for nullable data bindings.
        /// </summary>
        /// <typeparam name="TControl">The type of the control to which the data binding is to be added.</typeparam>
        /// <typeparam name="TSource">The type of the data source object from the binding source object.</typeparam>
        /// <typeparam name="TSourceProp">The type of the property in the data source object.</typeparam>
        /// <typeparam name="TControlValueMember">The type of the control's value member property.</typeparam>
        /// <param name="control">The control to which the data binding is to be added.</param>
        /// <param name="dataSourceValueMemberExpression">A lambda expression representing the control's value member property (E.g. "control.Text").</param>
        /// <param name="dataSource">The generic binding source containing the data-source object.</param>
        /// <param name="dataMemberExpression">A lambda expression representing the data source object's property to be bound.</param>
        /// <param name="isNullable">A boolean indicating whether the binding supports null values.</param>
        /// <exception cref="ArgumentException">Thrown when either of the provided expressions does not represent a property.</exception>
        /// <remarks>
        /// If the <paramref name="isNullable"/> parameter is set to true, the method adds parse event handlers that convert empty Guids or empty/null strings into nulls.
        /// </remarks>
        public static Binding AddDataBinding<TControl, TSource, TSourceProp, TControlValueMember>(
            this TControl control,
            Expression<Func<TControl, TControlValueMember>> dataSourceValueMemberExpression,
            BindingSourceGeneric<TSource> dataSource,
            Expression<Func<TSource, TSourceProp>> dataMemberExpression,
            bool isNullable)
            where TControl : Control
            where TSource : class
        {
            // Extract the property name of the control e.g. "EditValue" or "SelectedValue" or "Text"
            var valueMember = GetPropertyName(dataSourceValueMemberExpression);

            // Extract property names from expressions
            var dataMember = GetPropertyName(dataMemberExpression);

            // Add data binding
            var binding = isNullable
                ? new NullableBinding(valueMember, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged)
                : new Binding(valueMember, dataSource, dataMember, true, DataSourceUpdateMode.OnPropertyChanged, null);

            if (isNullable)
            {
                // Add parse event handler if its a nullable binding.
                binding.Format += (sender, e) =>
                {
                    if (e.Value is Guid g && g == Guid.Empty || string.IsNullOrEmpty(e.Value?.ToString()))
                    {
                        e.Value = null;
                    }
                };
                // Add parse event handler if its a nullable binding.
                binding.Parse += (sender, e) =>
                {
                    if (e.Value is Guid g && g == Guid.Empty || string.IsNullOrEmpty(e.Value?.ToString()))
                    {
                        e.Value = null;
                    }
                };
            }

            control.DataBindings.Add(binding);

            return binding;
        }

        /// <summary>
        /// Gets the name of the property represented by a lambda expression.
        /// </summary>
        /// <typeparam name="TSource">The type of the parameter in the lambda expression.</typeparam>
        /// <typeparam name="TProperty">The type of the property in the lambda expression.</typeparam>
        /// <param name="propertyLambda">A lambda expression that represents a property.</param>
        /// <returns>A string that represents the property name, without the parameter name.</returns>
        /// <exception cref="ArgumentException">Thrown when the expression does not represent a property.</exception>
        private static string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
        {
            // Check if the body of the lambda is a MemberExpression
            if (propertyLambda.Body is not MemberExpression member)
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");

            // Check if the member of the expression is a property
            if (member.Member is not PropertyInfo)
            {
                throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
            }

            // Convert the body of the lambda to a string
            var memberExpression = propertyLambda.Body.ToString();

            // Remove the parameter name from the string
            var withoutParameter = memberExpression.Substring(memberExpression.IndexOf('.') + 1);

            return withoutParameter;
        }
    }
}

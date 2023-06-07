# WinForms.NullableBinding

This C# helper library provides an easy way to add nullable data binding support to WinForms applications, particularly when using DevExpress controls. It simplifies data binding setup by offering strongly typed data binding expressions.

## Features

- **Strongly Typed Expressions:** Avoid the risk of typos in your data binding expressions and gain the benefits of compile-time checks and IntelliSense autocompletion in your IDE.
- **Nullable Data Binding:** This library supports standard data binding, but more importantly, it allows for `null` values to be assigned to and retrieved from the bound data member.

## Usage Example

Traditional approach to adding a data binding:

```csharp
txtTotalInclGst.DataBindings.Add(nameof(txtTotalInclGst.EditValue), _bindingSource, nameof(_transaction.TotalIncludingFreightAndTax), true, DataSourceUpdateMode.OnPropertyChanged);
```

With `WinForms.NullableBinding`, you can add a data binding in a strongly typed and potentially nullable way:

```csharp
txtTotalInclGst.AddDataBinding(_bindingSource, a => a.TotalIncludingFreightAndTax);
```

In this example, `_bindingSource` is a `BindingSourceGeneric<T>`.

## How to Use

Simply include the provided extension methods in your project, and make sure to import the corresponding namespace in your code files. You can then use the `AddDataBinding` and `AddNullableDataBinding` extension methods for data binding in place of traditional string-based methods.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.
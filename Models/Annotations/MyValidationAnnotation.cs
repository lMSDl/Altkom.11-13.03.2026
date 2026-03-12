using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Annotations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MyValidationAnnotation : ValidationAttribute
    {
        public string Value { get; set; } = string.Empty;

        public override bool IsValid(object? value)
        {
            return (value?.ToString().Contains(Value) ?? false);
        }

        public override string FormatErrorMessage(string name)
        {
            return $"The field {name} must contain the value '{Value}'.";
        }
    }
}

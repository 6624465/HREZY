using System;
using System.Globalization;
using System.Web.Mvc;
namespace HR.Web.Helpers
{
    public class MyStringModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value != null && !string.IsNullOrWhiteSpace(value.AttemptedValue))
            {
                return value.AttemptedValue.ToString().ToUpper();
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
using FluentValidation;
using Nop.Plugin.Customers.NopChat.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Customers.NopChat.Validators
{
    public partial class NopChatMessageValidator : BaseNopValidator<NopChatMessageModel>
    {
        public NopChatMessageValidator(ILocalizationService localizationService)
        {
            //// Latitude
            //RuleFor(model => model.Latitude)
            //    .InclusiveBetween(-90, 90)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Latitude.InvalidRange"))
            //    .When(model => model.Latitude.HasValue);
            //RuleFor(model => model.Latitude)
            //    .Must(latitude => latitude.HasValue)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Latitude.IsNullWhenLongitudeHasValue"))
            //    .When(model => model.Longitude.HasValue);
            //RuleFor(model => model.Latitude)
            //    .ScalePrecision(8, 18)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Latitude.InvalidPrecision"));

            //// Longitude
            //RuleFor(model => model.Longitude)
            //    .InclusiveBetween(-180, 180)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Longitude.InvalidRange"))
            //    .When(model => model.Longitude.HasValue);
            //RuleFor(model => model.Longitude)
            //    .Must(longitude => longitude.HasValue)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Longitude.IsNullWhenLatitudeHasValue"))
            //    .When(model => model.Latitude.HasValue);
            //RuleFor(model => model.Longitude)
            //    .ScalePrecision(8, 18)
            //    .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Customers.NopChat.Fields.Longitude.InvalidPrecision"));
        }
    }
}

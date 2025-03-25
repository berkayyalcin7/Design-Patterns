using WebApp.Template.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.Template.UserCards
{
    //<user-card app-user= />
    public class UserCardTagHelper:TagHelper
    {
        public AppUser AppUser {  get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserCardTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userTemplate;

            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userTemplate = new PrimeUserCardTemplate();

            }
            else
            {
                userTemplate = new DefaultUserCardTemplate();
            }

            userTemplate.SetUser(AppUser);

            output.Content.SetHtmlContent(userTemplate.Build());
        }
    }
}

using WebApp.Template.Models;
using Microsoft.Identity.Client;
using System.Text;

namespace WebApp.Template.UserCards
{
    public abstract class UserCardTemplate
    {
        protected AppUser AppUser { get; set; }

        public void SetUser(AppUser appUser)
        {
            AppUser = appUser;
        }

        public string Build()
        {
            if (AppUser == null)
            {
                throw new ArgumentNullException(nameof(AppUser));
            }

            var sb = new StringBuilder();

            sb.Append("<div class=\"card\" style=\"width: 18rem;\">");
            sb.Append(SetPicture());
            sb.Append("<div class=\"card-body\">");
            sb.Append($"<h5 class=\"card-title\">{AppUser.UserName}</h5>");
            sb.Append($"<p class=\"card-text text-muted\">{AppUser.Title ?? "Unvan Belirtilmemiş"}</p>");
            sb.Append($"<p class=\"card-text\"><small>Email: {AppUser.Email}</small></p>");
            sb.Append(SetFooter());
            sb.Append("</div>");
            sb.Append("</div>");

            return sb.ToString();
        }

        protected abstract string SetFooter();

        protected abstract string SetPicture();
    }
}

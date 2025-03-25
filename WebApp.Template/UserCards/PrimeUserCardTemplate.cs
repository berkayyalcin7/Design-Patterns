using System.Text;

namespace WebApp.Template.UserCards
{
    public class PrimeUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            var sb = new StringBuilder();

            sb.Append("<a href=\"#\" class=\"btn btn-primary\">Profili Görüntüle</a>");

            return sb.ToString();
        }

        protected override string SetPicture()
        {
            return $"<img src='{AppUser.PictureUrl}' class='card-img-top' alt='User Avatar'>";
        }
    }
}

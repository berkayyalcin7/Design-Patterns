namespace WebApp.Template.UserCards
{
    public class DefaultUserCardTemplate : UserCardTemplate
    {
        protected override string SetFooter()
        {
            return string.Empty;
        }

        protected override string SetPicture()
        {
            return $"<img src='https://cdn-icons-png.flaticon.com/512/149/149071.png' class='card-img-top' alt='User Avatar'>";
        }
    }
}

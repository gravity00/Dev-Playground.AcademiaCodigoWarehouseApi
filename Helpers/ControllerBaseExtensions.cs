namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerBaseExtensions
    {
        public static string GetUserName(this ControllerBase controller){
            if (controller == null)
            {
                throw new System.ArgumentNullException(nameof(controller));
            }

            return controller.User.Identity.Name ?? "anonymous";
        }
    }
}
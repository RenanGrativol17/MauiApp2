
namespace MauiApp2.Helpers
{
    public static class ViewExtensions
    {
        public static T FindParentOfType<T>(this BindableObject bindable) where T : BindableObject
        {
            var parent = bindable.BindingContext as BindableObject;
            while (parent != null)
            {
                if (parent is T result)
                {
                    return result;
                }
                parent = parent.BindingContext as BindableObject;
            }
            return null;
        }
    }
}
namespace WebMvcUI.Areas.Admin.Models
{
    public class ContentHeaderViewModel
    {
        public string Title { get; set; }

        public List<BreadcrumbViewModel> Breadcrumbs { get; set; } = new List<BreadcrumbViewModel>();
    }

    public class BreadcrumbViewModel
    {
        public string Title { get; set; } = "Default";
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string AreaName { get; set; }
        public bool IsActive { get; set; }
    }

}

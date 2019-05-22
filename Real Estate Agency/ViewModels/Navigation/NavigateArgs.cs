namespace REA.ViewModels.Navigation
{
    class NavigateArgs
    {
        public string Url { get; set; }

        public NavigateArgs()
        {

        }

        public NavigateArgs(string url)
        {
            Url = url;
        }
    }
}

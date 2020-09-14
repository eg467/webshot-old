namespace Webshot
{
    public class Options
    {
        public SpiderOptions SpiderOptions { get; set; } =
            new SpiderOptions();

        public ScreenshotOptions ScreenshotOptions { get; set; } =
            new ScreenshotOptions();

        public ViewerOptions ViewerOptions { get; set; } =
          new ViewerOptions();

        public ProjectCredentials Credentials { get; set; } = new ProjectCredentials();

        public SchedulerOptions SchedulerOptions { get; set; } = new SchedulerOptions();
    }
}
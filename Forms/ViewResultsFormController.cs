using System;
using System.Collections.Generic;
using System.Linq;

namespace Webshot
{
    public class ViewResultsFormController : IFormController<ViewResultsForm>
    {
        private readonly FormCreator<
            ViewResultsForm,
            ViewResultsFormController>
            _formCreator;

        public ViewResultsForm Form => _formCreator.Form;

        private Project Project => _debouncedProject.Project;
        private readonly Dictionary<string, ScreenshotResults> _results;

        private readonly DebouncedProject _debouncedProject;

        public ViewResultsFormController(DebouncedProject project, Dictionary<string, ScreenshotResults> results)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            _debouncedProject = project;
            _results = results ?? new Dictionary<string, ScreenshotResults>();
            _formCreator = new FormCreator<
                ViewResultsForm,
                ViewResultsFormController>(this);
        }

        public ViewResultsForm CreateForm()
        {
            _formCreator.CreateForm();
            WireEvents();
            return Form;
        }

        private void UnwireEvents()
        {
            if (Form == null)
            {
                return;
            }

            Form.Load -= Form_Load;
            Form.SessionSelected -= Form_SessionSelected;
            Form.OptionsChanged -= Form_OptionsChanged;
            Form.ScreenshotSelected -= Form_ScreenshotSelected;
        }

        private void WireEvents()
        {
            if (Form == null) return;

            Form.Load += Form_Load;
            Form.SessionSelected += Form_SessionSelected;
            Form.OptionsChanged += Form_OptionsChanged;
            Form.ScreenshotSelected += Form_ScreenshotSelected;
        }

        private void Form_SessionSelected(object sender, EventArgs e)
        {
            string key = Form.SelectedSession;
            ScreenshotResults screenshots = _results[key];
            Form.ShowSessionScreenshots(screenshots);
        }

        private void Form_ScreenshotSelected(object sender, EventArgs e)
        {
            Form.DisplayedImage =
                Form.SelectedImage != null
                ? Project.Store.GetImage(Form.SelectedSession, Form.SelectedImage)
                : null;
        }

        private void Form_OptionsChanged(object sender, EventArgs e)
        {
            _debouncedProject.Save();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Form.Options = Project.Options.ViewerOptions;
            Form.SessionIds = _results.Keys;
            Form.SelectedSession = _results.Keys.FirstOrDefault();
        }
    }
}
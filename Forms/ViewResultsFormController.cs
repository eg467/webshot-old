using System;
using System.Collections.Generic;
using System.Drawing;
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

        private readonly DebouncedProject _debouncedProject;

        public ViewResultsFormController(DebouncedProject project)
        {
            _debouncedProject = project;
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
            Form.OptionsChanged -= Form_OptionsChanged;
            Form.ScreenshotSelected -= Form_ScreenshotSelected;
        }

        private void WireEvents()
        {
            if (Form == null)
            {
                return;
            }

            Form.Load += Form_Load;
            Form.OptionsChanged += Form_OptionsChanged;
            Form.ScreenshotSelected += Form_ScreenshotSelected;
        }

        private void Form_ScreenshotSelected(object sender, EventArgs e)
        {
            Form.DisplayedImage =
                Form.SelectedImage != null
                ? Project.Store.GetImage(Form.SelectedImage)
                : null;
        }

        private void Form_OptionsChanged(object sender, EventArgs e)
        {
            _debouncedProject.Save();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Form.Results = Project.Output.Screenshots;
            Form.Options = Project.Options.ViewerOptions;
        }
    }
}
using System;
using System.Windows.Forms;

namespace Webshot.Forms
{
    public class OptionsFormController : IFormController<OptionsForm>
    {
        public Project Project { get; set; }
        public Options Options => Project.Options;

        public OptionsForm Form => _formCreator.Form;

        private readonly FormCreator<OptionsForm, OptionsFormController> _formCreator;

        public OptionsFormController()
        {
            _formCreator = new FormCreator<OptionsForm, OptionsFormController>(this);
        }

        public OptionsFormController(Project project) : this()
        {
            Project = project;
        }

        private void WireEvents()
        {
            if (Form == null)
            {
                return;
            }

            Form.Load += Form_Load;
            Form.Save += Form_Save;
        }

        private void UnwireEvents()
        {
            if (Form == null)
            {
                return;
            }

            Form.Load -= Form_Load;
            Form.Save -= Form_Save;
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Form.Options = Project.Options;
            Form.ProjectName = Project.Name;
        }

        private void Form_Save(object sender, EventArgs e)
        {
            Project.Options = Form.Options;
            Project.Name = Form.ProjectName;
            Project.Save();
            MessageBox.Show("Project Saved.");
        }

        public OptionsForm CreateForm()
        {
            _formCreator.CreateForm();
            WireEvents();
            return Form;
        }
    }
}
using System;
using System.Windows.Forms;

namespace Webshot
{
    internal class FormCreator<TForm, TController>
        where TForm : Form
        where TController : IFormController<TForm>
    {
        public TForm Form { get; set; }
        private readonly TController _controller;
        private readonly Action<TForm> _initializer;

        /// <summary>
        /// Creates a controlled form.
        /// </summary>
        /// <param name="controller">The form's controller.</param>
        /// <param name="initializer">An action to perform once the form loads.</param>
        public FormCreator(TController controller, Action<TForm> initializer = null)
        {
            _controller = controller;
            _initializer = initializer;
        }

        public TForm CreateForm()
        {
            this.Form = this.Form ?? Activator.CreateInstance<TForm>();
            if (_initializer != null)
            {
                this.Form.Load += FormLoaded;
            }
            this.Form.FormClosed += FormClosed;
            return this.Form;
        }

        private void FormLoaded(object sender, EventArgs e)
        {
            _initializer((TForm)sender);
        }

        private void FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Form = null;
        }
    }

    public interface IFormController<TForm>
    {
        TForm Form { get; }

        TForm CreateForm();
    }

    public interface IControlledForm<TController>
    {
        TController Controller { get; set; }
    }
}